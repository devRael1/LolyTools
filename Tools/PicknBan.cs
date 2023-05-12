using Loly.LeagueClient;
using Loly.Variables;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Loly.Tools;

public class PicknBan
{
    private static bool _pickedChamp;
    private static bool _lockedChamp;
    private static bool _pickedBan;
    private static bool _lockedBan;
    private static bool _sentChatMessages;
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
            _sentChatMessages = false;
            _champSelectStart = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        Global.LastChatRoom = currentChatRoom;

        if (_pickedChamp && _lockedChamp && _pickedBan && _lockedBan && _sentChatMessages)
        {
            Thread.Sleep(1000);
        }
        else
        {
            int localPlayerCellId = currentChampSelectJson.localPlayerCellId;

            if (Settings.ChampSelected.Id == null)
            {
                _pickedChamp = true;
                _lockedChamp = true;
            }

            if (Settings.ChampBanned.Id == null)
            {
                _pickedBan = true;
                _lockedBan = true;
            }

            // if (!Global.ChatMessagesEnabled) _sentChatMessages = true;
            // if (Global.ChatMessages.Count == 0) _sentChatMessages = true;

            if (!_pickedChamp || !_lockedChamp || !_pickedBan || !_lockedBan) HandleChampSelectActions(currentChampSelectJson, localPlayerCellId);

            // TODO: Faire le système d'envoie de message automatique
            // if (!sentChatMessages) handleChampSelectChat(currentChatRoom);
        }
    }

    public static void LoadChampionsList()
    {
        if (Global.ChampionsList.Any()) return;
        LoadSummonerId();

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

        champs = champs.OrderBy(c => c.Name).ToList();
        foreach (ChampItem champ in champs) Global.ChampionsList.Add(champ);
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
            int championId = action.championId;
            int actionId = action.id;
            bool actIsInProgress = action.isInProgress;

            if (actorCellId == localPlayerCellId && !completed && type == "pick") HandlePickAction(actionId, championId, actIsInProgress, currentChampSelectJson);
            if (actorCellId == localPlayerCellId && !completed && type == "ban") HandleBanAction(actionId, championId, actIsInProgress, currentChampSelectJson);
        }
    }

    private static void MarkPhaseStart(int actionId)
    {
        if (actionId == Global.LastActionId) return;
        Global.LastActionId = actionId;
        Global.LastActStartTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }

    private static void HandlePickAction(int actionId, int championId, bool actIsInProgress, dynamic currentChampSelectJson)
    {
        if (!_pickedChamp)
        {
            string champSelectPhase = currentChampSelectJson.timer.phase;
            long currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            if (currentTime - 10000 > _champSelectStart || champSelectPhase != "PLANNING")
                HoverChampion(actionId, Settings.ChampSelected.Id, "pick");
        }

        if (!actIsInProgress) return;
        MarkPhaseStart(actionId);
        if (_lockedChamp) return;

        Thread.Sleep(Settings.PickDelay);
        LockChampion(actionId, championId, "pick");
    }

    private static void HandleBanAction(int actionId, int championId, bool actIsInProgress, dynamic currentChampSelectJson)
    {
        string champSelectPhase = currentChampSelectJson.timer.phase;

        if (!actIsInProgress || champSelectPhase == "PLANNING") return;
        MarkPhaseStart(actionId);

        if (!_pickedBan) HoverChampion(actionId, Settings.ChampBanned.Id, "ban");
        if (_lockedBan) return;

        Thread.Sleep(Settings.BanDelay);
        LockChampion(actionId, championId, "ban");
    }

    private static void HoverChampion(int actionId, string currentChamp, string actType)
    {
        string[] champSelectAction = Requests.ClientRequest("PATCH", "lol-champ-select/v1/session/actions/" + actionId, true, "{\"championId\":" + currentChamp + "}");
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

    private static void LockChampion(int actionId, int championId, string actType)
    {
        string[] champSelectAction =
            Requests.ClientRequest("PATCH", "lol-champ-select/v1/session/actions/" + actionId, true, "{\"completed\":true,\"championId\":" + championId + "}");
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
        string[] currentSummoner = Requests.WaitSuccessClientRequest("GET", "lol-summoner/v1/current-summoner", true);
        dynamic currentSummonerSplit = JsonConvert.DeserializeObject(currentSummoner[1]);
        Global.CurrentSummonerId = currentSummonerSplit.summonerId;
    }
}