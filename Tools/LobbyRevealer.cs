using Loly.LeagueClient;
using Loly.Variables;
using Newtonsoft.Json;
using static Loly.Logs;

namespace Loly.Tools;

public class LobbyRevealer
{
    private static string _myregion;
    private static string _opggtoken;

    public static void GetAllNames()
    {
        while (true)
        {
            if (!Settings.LobbyRevealer || Global.Session != "Champ Select" || Global.PlayerList.Count > 0)
            {
                Thread.Sleep(5000);
                continue;
            }

            Global.PlayerList.Clear();
            Global.UsernameList.Clear();
            _myregion = GetRegion(Requests.WaitSuccessClientRequest("GET", "/riotclient/get_region_locale", true)[1]).ToLower();
            GetPlayers(Requests.ClientRequest("GET", "/chat/v5/participants/champ-select", false)[1]);
            Thread.Sleep(5000);
        }
    }

    private static string GetRegion(string request)
    {
        return JsonConvert.DeserializeObject<PlayerRegion>(request).Region;
    }

    private static void GetTokenOpGg()
    {
        Log(LogType.LobbyRevealer, "Getting OP.GG Token...");
        string response = Requests.WebRequest("https://www.op.gg/multisearch");
        _opggtoken = Utils.LrParse(response, "\"buildId\":\"", "\",\"assetPrefix") ?? "null";
    }

    private static void GetPlayers(string req)
    {
        Log(LogType.LobbyRevealer, "Getting Players for revealing lobby...");
        Players deserialized = JsonConvert.DeserializeObject<Players>(req);
        List<PlayerIn> participants = deserialized.Participants;
        string names = "";

        foreach (PlayerIn player in participants)
        {
            Player p = new(player.Name, "https://www.op.gg/summoners/" + _myregion + "/" + player.Name);
            names += player.Name;
            if (player != participants.Last()) names += ",";
            Global.PlayerList.Add(p);
        }

        Global.Region = _myregion;

        GetTokenOpGg();

        Log(LogType.LobbyRevealer, "Getting Players Stats with OP.GG...");
        string stats = Requests.WebRequest($"https://www.op.gg/_next/data/{_opggtoken}/multisearch/{_myregion}.json?summoners={names}&region={_myregion}");
        if (stats == null) return;

        dynamic json = JsonConvert.DeserializeObject(stats);
        dynamic summoners = json.pageProps.summoners;

        foreach (dynamic sum in summoners)
        {
            Player player = Global.PlayerList.Find(x => x.Username.ToLower().Equals(sum["name"].ToString().ToLower()));

            player.Level = sum.level;
            dynamic soloTierInfo = sum.solo_tier_info;
            if (soloTierInfo == null) continue;

            player.SoloDuoQ.Division = soloTierInfo.division >= 1 ? soloTierInfo.division : 0;
            player.SoloDuoQ.Tier = soloTierInfo.tier != null ? soloTierInfo.tier : "Unranked";
            player.SoloDuoQ.Lp = soloTierInfo.lp >= 0 ? soloTierInfo.lp : 0;
        }
    }

    public static void GetPlayerStats(Player player)
    {
        string stats = Requests.WebRequest($"https://www.op.gg/_next/data/{_opggtoken}/summoners/{_myregion}/{player.Username}.json?region={_myregion}&summoner={player.Username}");
        if (stats == null) return;

        dynamic json = JsonConvert.DeserializeObject(stats);
        dynamic summoner = json.pageProps.data.league_stats;

        player.SoloDuoQ.Wins = summoner[0].win >= 1 ? summoner[0].win : 0;
        player.SoloDuoQ.Losses = summoner[0].lose >= 1 ? summoner[0].lose : 0;

        player.FlexQ.Wins = summoner[1].win >= 1 ? summoner[1].win : 0;
        player.FlexQ.Losses = summoner[1].lose >= 1 ? summoner[1].lose : 0;
        player.FlexQ.Division = summoner[1].division >= 1 ? summoner[1].division : 0;
        player.FlexQ.Tier = summoner[1].tier != null ? summoner[1].tier : "Unranked";
        player.FlexQ.Lp = summoner[1].lp >= 0 ? summoner[1].lp : 0;
    }
}