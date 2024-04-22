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

    public static void HandleChampSelectActions()
    {
        List<List<Action>> champSelectActions = ChampSelectResponse.Actions;
        foreach (List<Action> arrActions in champSelectActions)
        {
            foreach (Action action in arrActions)
            {
                if (action.ActorCellId != ChampSelectResponse.LocalPlayerCellId || action.Completed) continue;
                HandleAction(action, action.Type == "pick" ? ActionType.Pick : ActionType.Ban);
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

    private static ChampItem GetChamp(ActionType actionType)
    {
        return ChampSelectSession.PickBanChampions.GetValueOrDefault(actionType).FirstOrDefault();
    }

    private static void HandleAction(Action action, ActionType actionType)
    {
        if (ChampSelectSession.PickBanChampions.GetValueOrDefault(actionType).Count == 0) return;

        var isHovered = actionType == ActionType.Pick ? ChampSelectSession.HoverPick : ChampSelectSession.HoverBan;
        var isLocked = actionType == ActionType.Pick ? ChampSelectSession.LockedPick : ChampSelectSession.LockedBan;
        var champSelectPhase = ChampSelectResponse.Timer.Phase;

        if (!isHovered)
        {
            var currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            if (currentTime - 3000 > ChampSelectSession.ChampSelectStart || champSelectPhase != "PLANNING") HoverChampion(action.Id, actionType);
        }

        if (!action.IsInProgress) return;

        MarkPhaseStart(action.Id);
        if (isLocked) return;

        ChampItem champion = GetChamp(actionType);
        Thread.Sleep(champion.Delay);
        LockChampion(action.Id, actionType);
    }

    private static void HoverChampion(int actionId, ActionType actionType)
    {
        ChampItem champion = GetChamp(actionType);
        Logger.Info(LogModule.PickAndBan, $"Hover '{champion.Name}' champion for '{actionType}'");

        var champSelectAction = Requests.ClientRequest("PATCH", $"lol-champ-select/v1/session/actions/{actionId}", true, $"{{\"championId\":{champion.Id}}}");
        if (champSelectAction[0] != "204") return;

        switch (actionType)
        {
            case ActionType.Pick:
                ChampSelectSession.HoverPick = true;
                break;
            case ActionType.Ban:
                ChampSelectSession.HoverBan = true;
                break;
        }

        Logger.Info(LogModule.PickAndBan, $"'{champion.Name}' has been hovered for {actionType}");
        Logger.Info(LogModule.PickAndBan, $"Waiting {champion.Delay}ms to '{actionType}' him");
    }

    private static void LockChampion(int actionId, ActionType actionType)
    {
        ChampItem champion = GetChamp(actionType);
        Logger.Info(LogModule.PickAndBan, $"Locking '{champion.Name}' champion for '{actionType}'");

        // TODO : Faire le système pour détecter si on ne peut pas lock le champion car il est déjà lock / ban par un autre joueur (equipe adverse à prendre en compte)
        // Check la tableau "actions" pour analyser les bans et les picks de chaque joueur dans la partie

        var champSelectAction = Requests.ClientRequest("PATCH", $"lol-champ-select/v1/session/actions/{actionId}", true,
            $"{{\"completed\":true,\"championId\":{champion.Id}}}");
        if (champSelectAction[0] != "204") return;

        switch (actionType)
        {
            case ActionType.Pick:
                ChampSelectSession.LockedPick = true;
                break;
            case ActionType.Ban:
                ChampSelectSession.LockedBan = true;
                break;
        }

        Logger.Info(LogModule.PickAndBan, $"'{champion.Name}' has been locked for {actionType}");
    }
}