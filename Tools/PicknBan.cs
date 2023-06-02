using Loly.LeagueClient;
using Loly.Variables;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Loly.Logs;

namespace Loly.Tools;

public class PicknBan
{
    private static bool _hoverPick;
    private static bool _lockedPick;
    private static bool _hoverBan;
    private static bool _lockedBan;
    private static bool _cansentChatMessages;
    private static long _champSelectStart;
    private static InitRole _currentRole;

    public static void HandleChampSelect()
    {
        string[] currentChampSelect = Requests.ClientRequest("GET", "lol-champ-select/v1/session", true);
        if (currentChampSelect[0] != "200") return;

        dynamic currentChampSelectJson = JsonConvert.DeserializeObject(currentChampSelect[1]);
        string currentChatRoom = currentChampSelectJson.chatDetails.multiUserChatId;
        if (Global.LastChatRoom != currentChatRoom || Global.LastChatRoom == "")
        {
            _hoverPick = false;
            _lockedPick = false;
            _hoverBan = false;
            _lockedBan = false;
            _cansentChatMessages = false;
            _champSelectStart = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            Log(LogType.PicknBan, "New game start, champ select started now...");
        }

        if (_lockedPick && _lockedBan)
        {
            Log(LogType.PicknBan, "Waiting for game to start...");
            Thread.Sleep(10000);
        }
        else
        {
            JArray myTeam = currentChampSelectJson.myTeam;
            foreach (dynamic member in myTeam)
            {
                string position = member.assignedPosition;
                string assignedRole = position switch
                {
                    "utility" => "Support",
                    "middle" => "Mid",
                    "jungle" => "Jungle",
                    "bottom" => "Adc",
                    "top" => "Top",
                    _ => "Default"
                };

                if (member.summonerId.ToString() != Global.CurrentSummonerId) continue;
                _currentRole = (InitRole)Settings.LoLRoles.GetType().GetProperty(assignedRole).GetValue(Settings.LoLRoles);
                break;
            }

            int localPlayerCellId = currentChampSelectJson.localPlayerCellId;
            if (_currentRole.PickChamp.Id == null)
            {
                _hoverPick = true;
                _lockedPick = true;
            }

            if (_currentRole.BanChamp.Id == null)
            {
                _hoverBan = true;
                _lockedBan = true;
            }

            if (Settings.AutoChat && Settings.ChatMessages.Count > 0)
                if (Global.LastChatRoom != currentChatRoom)
                    _cansentChatMessages = true;

            Global.LastChatRoom = currentChatRoom;

            if (Settings.AutoChat && _cansentChatMessages) AutoChat.HandleChampSelectAutoChat();
            if (!_hoverPick || !_lockedPick || !_hoverBan || !_lockedBan) HandleChampSelectActions(currentChampSelectJson, localPlayerCellId);
            _cansentChatMessages = false;
        }
    }

    private static void HandleChampSelectActions(dynamic currentChampSelectJson, int localPlayerCellId)
    {
        JArray champSelectActions = currentChampSelectJson.actions;
        foreach (dynamic arrActions in champSelectActions)
        foreach (dynamic action in arrActions)
        {
            int actorCellId = action.actorCellId;
            bool completed = action.completed;
            string type = action.type;
            int actionId = action.id;
            bool actIsInProgress = action.isInProgress;

            if (actorCellId == localPlayerCellId && !completed && type == "pick") HandlePickAction(actionId, actIsInProgress, currentChampSelectJson);
            if (actorCellId == localPlayerCellId && !completed && type == "ban") HandleBanAction(actionId, actIsInProgress, currentChampSelectJson);
        }
    }

    private static void MarkPhaseStart(int actionId)
    {
        if (actionId == Global.LastActionId) return;
        Global.LastActionId = actionId;
    }

    private static void HandlePickAction(int actionId, bool actIsInProgress, dynamic currentChampSelectJson)
    {
        if (_currentRole.PickChamp.Id == null) return;

        if (!_hoverPick)
        {
            string champSelectPhase = currentChampSelectJson.timer.phase;
            long currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            if (currentTime - 3000 > _champSelectStart || champSelectPhase != "PLANNING") HoverChampion(actionId, "pick");
        }

        if (!actIsInProgress) return;
        MarkPhaseStart(actionId);
        if (_lockedPick) return;

        Thread.Sleep(_currentRole.PickChamp.Delay);
        LockChampion(actionId, "pick");
    }

    private static void HandleBanAction(int actionId, bool actIsInProgress, dynamic currentChampSelectJson)
    {
        if (_currentRole.BanChamp.Id == null) return;

        string champSelectPhase = currentChampSelectJson.timer.phase;

        if (!actIsInProgress || champSelectPhase == "PLANNING") return;
        MarkPhaseStart(actionId);

        if (!_hoverBan) HoverChampion(actionId, "ban");
        if (_lockedBan) return;

        Thread.Sleep(_currentRole.BanChamp.Delay);
        LockChampion(actionId, "ban");
    }

    private static void HoverChampion(int actionId, string actType)
    {
        ChampItem champion = actType == "pick" ? _currentRole.PickChamp : _currentRole.BanChamp;
        Log(LogType.PicknBan, $"Hover {champion.Name} champion for {actType}...");

        string[] champSelectAction =
            Requests.ClientRequest("PATCH", "lol-champ-select/v1/session/actions/" + actionId, true, "{\"championId\":" + champion.Id + "}");
        if (champSelectAction[0] != "204") return;
        switch (actType)
        {
            case "pick":
                _hoverPick = true;
                break;
            case "ban":
                _hoverBan = true;
                break;
        }
    }

    private static void LockChampion(int actionId, string actType)
    {
        ChampItem champion = actType == "pick" ? _currentRole.PickChamp : _currentRole.BanChamp;
        Log(LogType.PicknBan, $"Locking {champion.Name} champion for {actType}...");

        string[] champSelectAction =
            Requests.ClientRequest("PATCH", "lol-champ-select/v1/session/actions/" + actionId, true, "{\"completed\":true,\"championId\":" + champion.Id + "}");
        if (champSelectAction[0] != "204") return;
        switch (actType)
        {
            case "pick":
                _lockedPick = true;
                break;
            case "ban":
                _lockedBan = true;
                break;
        }
    }
}