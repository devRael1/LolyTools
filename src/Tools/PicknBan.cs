using Loly.src.Logs;
using Loly.src.Variables;
using Loly.src.Variables.Class;
using Loly.src.Variables.Enums;
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
                if (action.ActorCellId != ChampSelectResponse.LocalPlayerCellId || action.Completed)
                {
                    continue;
                }

                if (action.Type == "pick")
                {
                    HandlePickAction(action);
                }

                if (action.Type == "ban")
                {
                    HandleBanAction(action);
                }
            }
        }
    }

    private static void MarkPhaseStart(int actionId)
    {
        if (actionId == Global.LastActionId)
        {
            return;
        }

        Global.LastActionId = actionId;
    }

    private static void HandlePickAction(Action action)
    {
        if (ChampSelectSession.CurrentRole.PickChamp.Id == null)
        {
            return;
        }

        if (!ChampSelectSession.HoverPick)
        {
            string champSelectPhase = ChampSelectResponse.Timer.Phase;
            long currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            if (currentTime - 3000 > ChampSelectSession.ChampSelectStart || champSelectPhase != "PLANNING")
            {
                HoverChampion(action.Id, "pick");
            }
        }

        if (!action.IsInProgress)
        {
            return;
        }

        MarkPhaseStart(action.Id);
        if (ChampSelectSession.LockedPick)
        {
            return;
        }

        Thread.Sleep(ChampSelectSession.CurrentRole.PickChamp.Delay);
        LockChampion(action.Id, "pick");
    }

    private static void HandleBanAction(Action action)
    {
        if (ChampSelectSession.CurrentRole.BanChamp.Id == null)
        {
            return;
        }

        string champSelectPhase = ChampSelectResponse.Timer.Phase;

        if (!action.IsInProgress || champSelectPhase == "PLANNING")
        {
            return;
        }

        MarkPhaseStart(action.Id);

        if (!ChampSelectSession.HoverBan)
        {
            HoverChampion(action.Id, "ban");
        }

        if (ChampSelectSession.LockedBan)
        {
            return;
        }

        Thread.Sleep(ChampSelectSession.CurrentRole.BanChamp.Delay);
        LockChampion(action.Id, "ban");
    }

    private static void HoverChampion(int actionId, string actType)
    {
        ChampItem champion = actType == "pick" ? ChampSelectSession.CurrentRole.PickChamp : ChampSelectSession.CurrentRole.BanChamp;
        Logger.Info(LogModule.PickAndBan, $"Hover '{champion.Name}' champion for {actType}");

        string[] champSelectAction = Requests.ClientRequest("PATCH", $"lol-champ-select/v1/session/actions/{actionId}", true, $"{{\"championId\":{champion.Id}}}");
        if (champSelectAction[0] != "204")
        {
            return;
        }

        switch (actType)
        {
            case "pick":
                ChampSelectSession.HoverPick = true;
                break;
            case "ban":
                ChampSelectSession.HoverBan = true;
                break;
        }

        Logger.Info(LogModule.PickAndBan, $"'{champion.Name}' has been hovered for {actType}");
    }

    private static void LockChampion(int actionId, string actType)
    {
        ChampItem champion = actType == "pick" ? ChampSelectSession.CurrentRole.PickChamp : ChampSelectSession.CurrentRole.BanChamp;
        Logger.Info(LogModule.PickAndBan, $"Locking '{champion.Name}' champion for {actType}");

        string[] champSelectAction = Requests.ClientRequest("PATCH", $"lol-champ-select/v1/session/actions/{actionId}", true,
            $"{{\"completed\":true,\"championId\":{champion.Id}}}");
        if (champSelectAction[0] != "204")
        {
            return;
        }

        switch (actType)
        {
            case "pick":
                ChampSelectSession.LockedPick = true;
                break;
            case "ban":
                ChampSelectSession.LockedBan = true;
                break;
        }

        Logger.Info(LogModule.PickAndBan, $"'{champion.Name}' has been locked for {actType}");

    }
}