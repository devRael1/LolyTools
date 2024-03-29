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
                if (action.Type == "pick") HandlePickAction(action);
                if (action.Type == "ban") HandleBanAction(action);
            }
        }
    }

    public static void LoadChampionsList()
    {
        // TODO : Refactor this method
        List<ChampItem> champs = new();

        var ownedChamps = Requests.WaitSuccessClientRequest("GET", "lol-champions/v1/inventories/" + Global.SummonerLogged.SummonerId + "/champions-minimal", true);
        dynamic champsSplit = JsonConvert.DeserializeObject(ownedChamps[1]);
        if (champsSplit == null) return;

        foreach (dynamic champ in champsSplit)
        {
            if (champ.id == -1) continue;

            string champName = champ.name;
            if (champName == "Nunu & Willump") champName = "Nunu";

            champs.Add(new ChampItem
            {
                Name = champName,
                Id = champ.id,
                Free = (bool)(champ.ownership.owned) || (bool)(champ.freeToPlay) || (bool)(champ.ownership.xboxGPReward)
            });
        }

        foreach (ChampItem champ in champs) Global.ChampionsList.Add(champ);
    }

    private static void MarkPhaseStart(int actionId)
    {
        if (actionId == Global.LastActionId) return;
        Global.LastActionId = actionId;
    }

    private static void HandlePickAction(Action action)
    {
        if (ChampSelectSession.CurrentRole.PickChamp.Id == null) return;

        if (!ChampSelectSession.HoverPick)
        {
            var champSelectPhase = ChampSelectResponse.Timer.Phase;
            var currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            if (currentTime - 3000 > ChampSelectSession.ChampSelectStart || champSelectPhase != "PLANNING") HoverChampion(action.Id, ActionType.Pick);
        }

        if (!action.IsInProgress) return;

        MarkPhaseStart(action.Id);
        if (ChampSelectSession.LockedPick) return;

        Thread.Sleep(ChampSelectSession.CurrentRole.PickChamp.Delay);
        LockChampion(action.Id, ActionType.Pick);
    }

    private static void HandleBanAction(Action action)
    {
        if (ChampSelectSession.CurrentRole.BanChamp.Id == null) return;
        var champSelectPhase = ChampSelectResponse.Timer.Phase;
        if (!action.IsInProgress || champSelectPhase == "PLANNING") return;

        MarkPhaseStart(action.Id);

        if (!ChampSelectSession.HoverBan) HoverChampion(action.Id, ActionType.Ban);
        if (ChampSelectSession.LockedBan) return;

        Thread.Sleep(ChampSelectSession.CurrentRole.BanChamp.Delay);
        LockChampion(action.Id, ActionType.Ban);
    }

    private static void HoverChampion(int actionId, ActionType actionType)
    {
        ChampItem champion = actionType == ActionType.Pick ? ChampSelectSession.CurrentRole.PickChamp : ChampSelectSession.CurrentRole.BanChamp;
        Logger.Info(LogModule.PickAndBan, $"Hover '{champion.Name}' champion for {actionType}");

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
        var delay = actionType == ActionType.Pick ? ChampSelectSession.CurrentRole.PickChamp.Delay : ChampSelectSession.CurrentRole.BanChamp.Delay;
        Logger.Info(LogModule.PickAndBan, $"Waiting {delay}ms to '{actionType}' him");
    }

    private static void LockChampion(int actionId, ActionType actionType)
    {
        ChampItem champion = actionType == ActionType.Pick ? ChampSelectSession.CurrentRole.PickChamp : ChampSelectSession.CurrentRole.BanChamp;
        Logger.Info(LogModule.PickAndBan, $"Locking '{champion.Name}' champion for {actionType}");

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