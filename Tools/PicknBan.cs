using Loly.LeagueClient;
using Loly.Variables;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Loly.Logs;

namespace Loly.Tools;

public class PicknBan
{
    private static bool _pickedChamp;
    private static bool _lockedChamp;
    private static bool _pickedBan;
    private static bool _lockedBan;
    private static bool _cansentChatMessages;
    private static long _champSelectStart;

    public static void HandleChampSelect()
    {
        string[] currentChampSelect = Requests.ClientRequest("GET", "lol-champ-select/v1/session", true);
        if (currentChampSelect[0] != "200") return;
        dynamic currentChampSelectJson = JsonConvert.DeserializeObject(currentChampSelect[1]);
        string currentChatRoom = currentChampSelectJson.chatDetails.multiUserChatId;
        if (Global.LastChatRoom != currentChatRoom || Global.LastChatRoom == "")
        {
            _pickedChamp = false;
            _lockedChamp = false;
            _pickedBan = false;
            _lockedBan = false;
            _cansentChatMessages = false;
            _champSelectStart = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            Log(LogType.PicknBan, "New game start, champ select started now...");
        }

        if (_lockedChamp)
        {
            Log(LogType.PicknBan, "Waiting for game to start...");
            Thread.Sleep(10000);
        }
        else
        {
            int localPlayerCellId = currentChampSelectJson.localPlayerCellId;

            if (Settings.PickChamp.Id == null)
            {
                _pickedChamp = true;
                _lockedChamp = true;
            }

            if (Settings.BanChamp.Id == null)
            {
                _pickedBan = true;
                _lockedBan = true;
            }

            if (Settings.AutoChat && Settings.ChatMessages.Count >= 1)
                if (Global.LastChatRoom != currentChatRoom)
                    _cansentChatMessages = true;

            Global.LastChatRoom = currentChatRoom;

            if (!_pickedChamp || !_lockedChamp || !_pickedBan || !_lockedBan) HandleChampSelectActions(currentChampSelectJson, localPlayerCellId);
            if (_cansentChatMessages) AutoChat.HandleChampSelectAutoChat();
            _cansentChatMessages = false;
        }
    }

    public static void LoadChampionsList()
    {
        if (Global.ChampionsList.Any()) return;
        LoadSummonerId();

        Log(LogType.PicknBan, "Loading champions list of your account...");

        List<ChampItem> champs = new();

        string[] ownedChamps = Requests.WaitSuccessClientRequest("GET", "lol-champions/v1/inventories/" + Global.CurrentSummonerId + "/champions-minimal", true);
        dynamic champsSplit = JsonConvert.DeserializeObject(ownedChamps[1]);

        foreach (dynamic champ in champsSplit)
        {
            if (champ.id == -1) continue;

            string champName = champ.name;
            string champId = champ.id;
            bool champOwned = champ.ownership.owned;
            bool champFreeXboxPass = champ.ownership.xboxGPReward;
            bool champFree = champ.freeToPlay;

            if (champName == "Nunu & Willump") champName = "Nunu";

            bool isAvailable;
            if (champOwned || champFree || champFreeXboxPass)
                isAvailable = true;
            else
                isAvailable = false;

            champs.Add(new ChampItem { Name = champName, Id = champId, Free = isAvailable });
        }

        foreach (ChampItem champ in champs) Global.ChampionsList.Add(champ);
    }

    private static void HandleChampSelectActions(dynamic currentChampSelectJson, int localPlayerCellId)
    {
        if (!Settings.PicknBan) return;

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
        if (!_pickedChamp)
        {
            string champSelectPhase = currentChampSelectJson.timer.phase;
            long currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            if (currentTime - 3000 > _champSelectStart || champSelectPhase != "PLANNING") HoverChampion(actionId, "pick");
        }

        if (!actIsInProgress) return;
        MarkPhaseStart(actionId);
        if (_lockedChamp) return;

        Thread.Sleep(Settings.PickDelay);
        LockChampion(actionId, "pick");
    }

    private static void HandleBanAction(int actionId, bool actIsInProgress, dynamic currentChampSelectJson)
    {
        string champSelectPhase = currentChampSelectJson.timer.phase;

        if (!actIsInProgress || champSelectPhase == "PLANNING") return;
        MarkPhaseStart(actionId);

        if (!_pickedBan) HoverChampion(actionId, "ban");
        if (_lockedBan) return;

        Thread.Sleep(Settings.BanDelay);
        LockChampion(actionId, "ban");
    }

    private static void HoverChampion(int actionId, string actType)
    {
        ChampItem champion = actType == "pick" ? Settings.PickChamp : Settings.BanChamp;
        Log(LogType.PicknBan, $"Hover {champion.Name} champion for {actType}...");

        string[] champSelectAction =
            Requests.ClientRequest("PATCH", "lol-champ-select/v1/session/actions/" + actionId, true, "{\"championId\":" + champion.Id + "}");
        if (champSelectAction[0] != "204") return;
        switch (actType)
        {
            case "pick":
                _pickedChamp = true;
                break;
            case "ban":
                _pickedBan = true;
                break;
        }
    }

    private static void LockChampion(int actionId, string actType)
    {
        ChampItem champion = actType == "pick" ? Settings.PickChamp : Settings.BanChamp;
        Log(LogType.PicknBan, $"Locking {champion.Name} champion for {actType}...");

        string[] champSelectAction =
            Requests.ClientRequest("PATCH", "lol-champ-select/v1/session/actions/" + actionId, true, "{\"completed\":true,\"championId\":" + champion.Id + "}");
        if (champSelectAction[0] != "204") return;
        switch (actType)
        {
            case "pick":
                _lockedChamp = true;
                break;
            case "ban":
                _lockedBan = true;
                break;
        }
    }

    private static void LoadSummonerId()
    {
        if (Global.CurrentSummonerId != "") return;
        Log(LogType.PicknBan, "Getting your summoner id...");
        string[] currentSummoner = Requests.WaitSuccessClientRequest("GET", "lol-summoner/v1/current-summoner", true);
        dynamic currentSummonerSplit = JsonConvert.DeserializeObject(currentSummoner[1]);
        Global.CurrentSummonerId = currentSummonerSplit.summonerId;
    }
}