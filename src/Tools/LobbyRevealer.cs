using Loly.src.LeagueClient;
using Loly.src.Logs;
using Loly.src.Variables;
using Loly.src.Variables.Class;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Loly.src.Tools;

public class LobbyRevealer
{
    private static string _opggtoken;

    public static void GetLobbyRevealing()
    {
        while (true)
        {
            if (!Settings.LobbyRevealer || Global.Session != "ChampSelect" || Global.FetchedPlayers)
            {
                Thread.Sleep(5000);
                continue;
            }

            Global.PlayerList.Clear();
            if (_opggtoken == null)
            {
                GetTokenOpGg();
            }

            Thread.Sleep(3000);
            GetPlayers(Requests.ClientRequest("GET", "/chat/v5/participants/lol-champ-select", false)[1]);
            _ = Task.Run(GetAdvancedPlayersStats);

            Global.FetchedPlayers = true;
        }
    }

    private static void GetTokenOpGg()
    {
        Logger.Info(LogModule.LobbyRevealer, "Fetching OP.GG token...");
        string response = Requests.WebRequest("https://www.op.gg/multisearch");
        _opggtoken = Utils.LrParse(response, "\"buildId\":\"", "\",\"assetPrefix") ?? "null";
    }

    private static void GetPlayers(string req)
    {
        Logger.Info(LogModule.LobbyRevealer, "Getting Players for revealing lobby...");
        dynamic deserialized = JsonConvert.DeserializeObject(req);
        JArray teamPlayers = deserialized.participants;

        List<string> cacheNames = (from dynamic player in teamPlayers select $"{player.game_name}#{player.game_tag}").Cast<string>().ToList();

        string stats = Requests.WebRequest(
            $"https://www.op.gg/_next/data/{_opggtoken}/en_US/multisearch/{Global.Region}.json?summoners={string.Join(",", cacheNames.Select(x => x))}&region={Global.Region}");
        if (stats == null)
        {
            return;
        }

        dynamic json = JsonConvert.DeserializeObject(stats);
        JArray summoners = json.pageProps.summoners;

        foreach (dynamic sum in summoners)
        {
            Player newPlayer = new(sum.name.ToString(), $"https://www.op.gg/summoners/{Global.Region}/{sum.name}")
            {
                Id = sum.id.ToString(),
                Level = Convert.ToInt32(sum.level)
            };

            dynamic soloTierInfo = sum.solo_tier_info;
            if (soloTierInfo != null)
            {
                newPlayer.SoloDuoQ.Division = soloTierInfo.division >= 1 ? soloTierInfo.division : 0;
                newPlayer.SoloDuoQ.Tier = soloTierInfo.tier ?? "Unranked";
                newPlayer.SoloDuoQ.Lp = soloTierInfo.lp >= 0 ? soloTierInfo.lp : 0;
            }

            Global.PlayerList.Add(newPlayer);
        }
    }

    private static async Task<string> GetPlayerStats(Player player)
    {
        string stats = await Requests.WebRequestAsync(
            $"https://www.op.gg/_next/data/{_opggtoken}/en_US/summoners/{Global.Region}/{player.Username}.json?region={Global.Region}&summoner={player.Username}");
        return stats;
    }

    private static void GetAdvancedPlayersStats()
    {
        Logger.Info(LogModule.LobbyRevealer, $"Getting advanced stats of {Global.PlayerList.Count} players in background...");

        foreach (Player player in Global.PlayerList)
        {
            string stats = GetPlayerStats(player).Result;

            dynamic json = JsonConvert.DeserializeObject(stats);
            dynamic summoner = json.pageProps.data.league_stats;
            int sumId = json.pageProps.data.id;

            Player currentPlayer = Global.FindPlayer(sumId.ToString());

            currentPlayer.SoloDuoQ.Wins = summoner[0].win >= 1 ? summoner[0].win : 0;
            currentPlayer.SoloDuoQ.Losses = summoner[0].lose >= 1 ? summoner[0].lose : 0;

            currentPlayer.FlexQ.Wins = summoner[1].win >= 1 ? summoner[1].win : 0;
            currentPlayer.FlexQ.Losses = summoner[1].lose >= 1 ? summoner[1].lose : 0;
            currentPlayer.FlexQ.Division = summoner[1].division >= 1 ? summoner[1].division : 0;
            currentPlayer.FlexQ.Tier = summoner[1].tier ?? "Unranked";
            currentPlayer.FlexQ.Lp = summoner[1].lp >= 0 ? summoner[1].lp : 0;
        }

        Logger.Info(LogModule.LobbyRevealer, "Advanced stats of all players fetched !");
    }
}