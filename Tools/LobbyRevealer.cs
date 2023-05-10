using Loly.LeagueClient;
using Loly.Variables;
using Newtonsoft.Json;

namespace Loly.Tools;

public class LobbyRevealer
{
    private static string _myregion;
    private static string _opggtoken;

    public static void GetAllNames()
    {
        Global.PlayerList.Clear();
        Global.UsernameList.Clear();
        Ux.GetLeagueAuth();
        _myregion = GetRegion(Requests.WaitSuccessClientRequest("GET", "/riotclient/get_region_locale", true)[1]).ToLower();
        GetPlayers(Requests.WaitSuccessClientRequest("GET", "/chat/v5/participants/champ-select", false)[1]);
    }

    private static string GetRegion(string request)
    {
        return JsonConvert.DeserializeObject<PlayerRegion>(request).Region;
    }

    private static void GetTokenOpGg()
    {
        string response = Requests.WebRequest("https://www.op.gg/multisearch");
        _opggtoken = Utils.LrParse(response, "\"buildId\":\"", "\",\"assetPrefix") ?? "null";
    }

    private static void GetPlayers(string req)
    {
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

        string stats = Requests.WebRequest($"https://www.op.gg/_next/data/{_opggtoken}/multisearch/{_myregion}.json?summoners={names}&region={_myregion}");
        if (stats == null) return;

        dynamic json = JsonConvert.DeserializeObject(stats);
        dynamic summoners = json["pageProps"]["summoners"];

        foreach (dynamic sum in summoners)
        {
            Player player = Global.PlayerList.Find(x => x.Username.ToLower().Equals(sum["name"].ToString().ToLower()));

            player.Level = sum["level"];
            player.SoloDuoQ.Division = sum["solo_tier_info"]["division"] >= 1 ? sum["solo_tier_info"]["division"] : 0;
            player.SoloDuoQ.Tier = sum["solo_tier_info"]["tier"] != null ? sum["solo_tier_info"]["tier"] : "Unranked";
            player.SoloDuoQ.Lp = sum["solo_tier_info"]["lp"] >= 0 ? sum["solo_tier_info"]["lp"] : 0;
        }
    }

    public static void GetPlayerStats(Player player)
    {
        string stats = Requests.WebRequest($"https://www.op.gg/_next/data/{_opggtoken}/summoners/{_myregion}/{player.Username}.json?region={_myregion}&summoner={player.Username}");
        if (stats == null) return;

        dynamic json = JsonConvert.DeserializeObject(stats);
        dynamic summoner = json["pageProps"]["data"]["league_stats"];

        player.SoloDuoQ.Wins = summoner[0]["win"] >= 1 ? summoner[0]["win"] : 0;
        player.SoloDuoQ.Losses = summoner[0]["lose"] >= 1 ? summoner[0]["lose"] : 0;

        player.FlexQ.Wins = summoner[1]["win"] >= 1 ? summoner[1]["win"] : 0;
        player.FlexQ.Losses = summoner[1]["lose"] >= 1 ? summoner[1]["lose"] : 0;
        player.FlexQ.Division = summoner[1]["division"] >= 1 ? summoner[1]["division"] : 0;
        player.FlexQ.Tier = summoner[1]["tier"] != null ? summoner[1]["tier"] : "Unranked";
        player.FlexQ.Lp = summoner[1]["lp"] >= 0 ? summoner[1]["lp"] : 0;
    }
}