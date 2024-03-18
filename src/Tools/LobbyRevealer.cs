using System.Web;
using Gommon;
using Loly.src.Logs;
using Loly.src.Variables;
using Loly.src.Variables.Class;
using Loly.src.Variables.Enums;
using Newtonsoft.Json;

namespace Loly.src.Tools;

public class LobbyRevealer
{
    private static string opGGToken;
    public static string OpGGToken { get => opGGToken; set => opGGToken = value; }

    public static void GetTokenOpGg()
    {
        Logger.Info(LogModule.LobbyRevealer, "Fetching OP.GG token");
        string response = Requests.WebRequest("www.op.gg", false);
        OpGGToken = Utils.LrParse(response, "\"buildId\":\"", "\",\"assetPrefix") ?? "null";
        Logger.Info(LogModule.LobbyRevealer, $"Fetching OP.GG token successfully (Token: {OpGGToken})");
    }

    public static void GetLobbyRevealing()
    {
        Thread.Sleep(TimeSpan.FromSeconds(3));

        if (OpGGToken == null)
        {
            GetTokenOpGg();
        }

        GetPlayers(Requests.ClientRequest("GET", "/chat/v5/participants/lol-champ-select", false)[1]);
        Global.FetchedPlayers = true;
        GetAdvancedPlayersStats();
        Global.LobbyRevealingStarted = false;
    }

    public static void GetPlayers(string req)
    {
        Global.PlayerList.Clear();

        Logger.Info(LogModule.LobbyRevealer, "Fetching Players for revealing lobby");
        ParticipantsResponse teamPlayers = JsonConvert.DeserializeObject<ParticipantsResponse>(req);

        List<string> cacheNames = (from Participant player in teamPlayers.Participants select $"{player.GameName}#{player.GameTag}").Cast<string>().ToList();
        if (cacheNames.Count == 0)
        {
            Logger.Warn(LogModule.LobbyRevealer, "No players found in the lobby");
            Logger.Warn(LogModule.LobbyRevealer, "Please contact the developer to fix/analyze this issue");
            Logger.Warn(LogModule.LobbyRevealer, "Please save your logs and send them to the developer");
            return;
        }

        string encodeNames = string.Join(",", cacheNames.Select(n => HttpUtility.UrlEncode(n)));
        string url = $"www.op.gg/_next/data/{OpGGToken}/en_US/multisearch/{Global.Region}.json?summoners={encodeNames}&region={Global.Region}";
        string stats = Requests.WebRequest(url, false);

        MultisearchResponse response = JsonConvert.DeserializeObject<MultisearchResponse>(stats);
        foreach (Summoner sum in response.PageProps.Summoners)
        {
            Player newPlayer = new(sum.GameName, sum.Tagline, $"https://www.op.gg/summoners/{Global.Region}/{sum.GameName}-{sum.Tagline}")
            {
                Id = sum.Id,
                Level = (int)sum.Level
            };

            SoloTierInfo soloTierInfo = sum.SoloTierInfo;
            if (soloTierInfo != null)
            {
                newPlayer.SoloDuoQ.Division = (int)(soloTierInfo.Division >= 1 ? soloTierInfo.Division : 0);
                newPlayer.SoloDuoQ.Tier = soloTierInfo.Tier ?? "Unranked";
                newPlayer.SoloDuoQ.Lp = (int)(soloTierInfo.Lp >= 0 ? soloTierInfo.Lp : 0);
            }

            Global.PlayerList.Add(newPlayer);
        }

        Logger.Info(LogModule.LobbyRevealer, $"Players successfully fetched : {Global.PlayerList.Select(p => p.UserTag).Join(",")}");
    }

    public static void GetAdvancedPlayersStats()
    {
        Logger.Info(LogModule.LobbyRevealer, $"Fetching advanced stats of {Global.PlayerList.Count} players in background");

        foreach (Player player in Global.PlayerList)
        {
            Task.Run(() => GetPlayerStats(player)).ContinueWith(t =>
            {
                if (t.IsFaulted) Utils.LogNewError($"Fetch all stats of player '{player.UserTag}'", LogModule.LobbyRevealer, t.Exception);
            });
        }
    }

    public static void GetPlayerStats(Player player)
    {
        string url = $"www.op.gg/_next/data/{OpGGToken}/en_US/summoners/{Global.Region}/{player.UserTagUrlReady}.json?region={Global.Region}&summoner={player.UserTagUrlReady}";
        string stats = Requests.WebRequest(url, false);

        if (stats == null)
        {
            Logger.Error(LogModule.LobbyRevealer, $"Error while fetching advanced stats of '{player.UserTag}'");
            Logger.Error(LogModule.LobbyRevealer, "Please check the request logs for more information");
            return;
        }

        PlayerStatsResponse response = JsonConvert.DeserializeObject<PlayerStatsResponse>(stats);
        List<LeagueStat> summoner = response.PageProps.Data.LeagueStats;
        int sumId = response.PageProps.Data.Id;

        Player currentPlayer = Global.PlayerList.Find(x => x.Id == sumId);

        currentPlayer.SoloDuoQ.Wins = (int)(summoner[0].Win >= 1 ? summoner[0].Win : 0);
        currentPlayer.SoloDuoQ.Losses = (int)(summoner[0].Lose >= 1 ? summoner[0].Lose : 0);

        currentPlayer.FlexQ.Wins = (int)(summoner[1].Win >= 1 ? summoner[1].Win : 0);
        currentPlayer.FlexQ.Losses = (int)(summoner[1].Lose >= 1 ? summoner[1].Lose : 0);
        currentPlayer.FlexQ.Division = (int)(summoner[1].TierInfo.Division >= 1 ? summoner[1].TierInfo.Division : 0);
        currentPlayer.FlexQ.Tier = summoner[1].TierInfo.Tier ?? "Unranked";
        currentPlayer.FlexQ.Lp = (int)(summoner[1].TierInfo.Lp >= 0 ? summoner[1].TierInfo.Lp : 0);

        Logger.Info(LogModule.LobbyRevealer, $"Advanced stats fetched for {currentPlayer.UserTag} : " +
            $"Solo/DuoQ: {currentPlayer.SoloDuoQ.Tier} {currentPlayer.SoloDuoQ.Division} | {currentPlayer.SoloDuoQ.Lp}LP ({currentPlayer.SoloDuoQ.Wins} Win(s)  {currentPlayer.SoloDuoQ.Losses} Losse(s)) / " +
            $"FlexQ: {currentPlayer.FlexQ.Tier} {currentPlayer.FlexQ.Division} | {currentPlayer.FlexQ.Lp}LP ({currentPlayer.FlexQ.Wins} Win(s) {currentPlayer.FlexQ.Losses} Losse(s))");
    }
}