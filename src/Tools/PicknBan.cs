using Loly.src.Logs;
using Loly.src.Variables;
using Loly.src.Variables.Class;
using Loly.src.Variables.Enums;

using Newtonsoft.Json;

using Action = Loly.src.Variables.Class.Action;

namespace Loly.src.Tools;

public class PicknBan
{
    public static ChampSelectResponse ChampSelectResponse { get; set; }
    private static ActionType ActionType { get; set; }

    public static void HandleChampSelectActions()
    {
        List<List<Action>> champSelectActions = ChampSelectResponse.Actions;
        foreach (List<Action> arrActions in champSelectActions)
        {
            foreach (Action action in arrActions)
            {
                if (action.ActorCellId != ChampSelectResponse.LocalPlayerCellId || action.Completed) continue;

                ActionType = action.Type == "pick" ? ActionType.Pick : ActionType.Ban;
                HandleAction(action);
            }
        }
    }

    public static void LoadChampionsList()
    {
        Global.ChampionsList.Clear();
        List<Champion> champsSplit = JsonConvert.DeserializeObject<List<Champion>>(
            Requests.WaitSuccessClientRequest("GET", "lol-champions/v1/inventories/" + Global.SummonerLogged.SummonerId + "/champions-minimal", true)[1]
        );

        foreach (Champion champ in champsSplit)
        {
            if (champ.Id == -1) continue;

            Global.ChampionsList.Add(
                new ChampItem
                {
                    Name = champ.Name,
                    Id = champ.Id.ToString(),
                    Usable = champ.Ownership.Owned || champ.FreeToPlay || champ.Ownership.XboxGPReward || champ.Ownership.LoyaltyReward
                }
            );
        }
    }

    private static void MarkPhaseStart(int actionId)
    {
        if (actionId == Global.LastActionId) return;
        Global.LastActionId = actionId;
    }

    private static void HandleAction(Action action)
    {
        if (ChampSelectSession.PickBanChampions.GetValueOrDefault(ActionType).Count == 0) return;

        var isHovered = ActionType == ActionType.Pick ? ChampSelectSession.HoverPick : ChampSelectSession.HoverBan;
        var isLocked = ActionType == ActionType.Pick ? ChampSelectSession.LockedPick : ChampSelectSession.LockedBan;
        var champSelectPhase = ChampSelectResponse.Timer.Phase;

        if (!isHovered)
        {
            var currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            if (currentTime - 3000 > ChampSelectSession.ChampSelectStart || champSelectPhase != "PLANNING") HoverChampion(action.Id);
        }

        if (!action.IsInProgress) return;

        MarkPhaseStart(action.Id);
        if (isLocked) return;

        ChampItem champion = ChampSelectSession.PickBanChampions.GetValueOrDefault(ActionType).FirstOrDefault();
        Thread.Sleep(champion?.Delay ?? 1000);
        LockChampion(action.Id);
    }

    private static void HoverChampion(int actionId)
    {
        ChampItem champion = GetActualChampToUse(false);
        if (champion == null)
        {
            Logger.Info(LogModule.PickAndBan, $"All your champions to '{ActionType}' for the '{ChampSelectSession.CurrentRole}' role are already BAN");
            Logger.Info(LogModule.PickAndBan, $"I can't hover a champ to '{ActionType}' him");
            HoverOrLockChampSession(ActionChampionType.Hover);
            return;
        }

        Logger.Info(LogModule.PickAndBan, $"Hover '{champion.Name}' champion for '{ActionType}'");
        Requests.ClientRequest("PATCH", $"lol-champ-select/v1/session/actions/{actionId}", true, $"{{\"championId\":{champion.Id}}}");
        HoverOrLockChampSession(ActionChampionType.Hover);
        Logger.Info(LogModule.PickAndBan, $"'{champion.Name}' has been hovered for {ActionType}");
        Logger.Info(LogModule.PickAndBan, $"Waiting {champion.Delay}ms to '{ActionType}' him");
    }

    private static void LockChampion(int actionId)
    {
        ChampItem champion = GetActualChampToUse(true);
        if (champion == null)
        {
            Logger.Info(LogModule.PickAndBan, $"All your champions to '{ActionType}' for the '{ChampSelectSession.CurrentRole}' role are already PICK or BAN");
            Logger.Info(LogModule.PickAndBan, $"I can't lock a champ to '{ActionType}' him");
            HoverOrLockChampSession(ActionChampionType.Lock);
            return;
        }

        Logger.Info(LogModule.PickAndBan, $"Locking '{champion.Name}' champion for '{ActionType}'");
        Requests.ClientRequest("PATCH", $"lol-champ-select/v1/session/actions/{actionId}", true, $"{{\"completed\":true,\"championId\":{champion.Id}}}");
        HoverOrLockChampSession(ActionChampionType.Lock);
        Logger.Info(LogModule.PickAndBan, $"'{champion.Name}' has been locked for {ActionType}");
    }

    private static ChampItem GetActualChampToUse(bool lockChamp)
    {
        List<ChampItem> CachedPickBanChampions = ChampSelectSession.PickBanChampions.GetValueOrDefault(ActionType);
        ChampItem firstChamp = CachedPickBanChampions.FirstOrDefault();

    START:
        foreach (List<Action> arrActions in ChampSelectResponse.Actions)
        {
            foreach (Action action in arrActions)
            {
                if (action.ChampionId == Convert.ToInt32(firstChamp?.Id) && !action.IsInProgress && action.Completed)
                {
                    if (!lockChamp && action.Type != "ban") continue;

                    CachedPickBanChampions.Remove(firstChamp);
                    firstChamp = CachedPickBanChampions.FirstOrDefault();
                    if (firstChamp == null) return null;
                    goto START;
                }
            }
        }

        return CachedPickBanChampions.FirstOrDefault();
    }

    private static void HoverOrLockChampSession(ActionChampionType actionChampionType)
    {
        if (actionChampionType == ActionChampionType.Hover)
        {
            if (ActionType == ActionType.Pick) ChampSelectSession.HoverPick = true;
            else ChampSelectSession.HoverBan = true;
        }
        else
        {
            if (ActionType == ActionType.Pick) ChampSelectSession.LockedPick = true;
            else ChampSelectSession.LockedBan = true;
        }
    }
}