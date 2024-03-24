using System.Web;

using Gommon;

using Loly.src.Logs;
using Loly.src.Variables;
using Loly.src.Variables.Class;
using Loly.src.Variables.Enums;
using Loly.src.Variables.UGG.APIs.GetMultisearch;
using Loly.src.Variables.UGG.APIs.GetSummonerProfile;

using Newtonsoft.Json;

namespace Loly.src.Tools;

public class LobbyRevealer
{
    public static void GetLobbyRevealing()
    {
        Thread.Sleep(TimeSpan.FromSeconds(5));

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
        if (teamPlayers == null || teamPlayers.Participants.Count == 0)
        {
            Logger.Warn(LogModule.LobbyRevealer, "No players found in the lobby");
            Logger.Warn(LogModule.LobbyRevealer, "Please contact the developer to fix/analyze this issue");
            Logger.Warn(LogModule.LobbyRevealer, "Please save your logs and send them to the developer");
            return;
        }

        GetMultisearch body = new();
        foreach (Participant participant in teamPlayers.Participants)
        {
            body.Variables.RegionId.Add(Utils.RegionId(Enum.Parse<Region>(Global.Region.ToUpper())));
            body.Variables.RiotUserName.Add(participant.GameName.ToLower());
            body.Variables.RiotTagLine.Add(participant.GameTag.ToLower());
        }
        GetMultisearchResponse response = JsonConvert.DeserializeObject<GetMultisearchResponse>(Requests.WebRequest("POST", Global.UrlAPI, body.ToFormattedString()));
        if (response.Data.GetMultisearch.All(p => p.RiotUserName == null))
        {
            Logger.Warn(LogModule.LobbyRevealer, "No players found with multisearch of U.GG");
            Logger.Warn(LogModule.LobbyRevealer, "Please contact the developer to fix/analyze this issue");
            Logger.Warn(LogModule.LobbyRevealer, "Please save your logs and send them to the developer");
            return;
        }

        response.Data.GetMultisearch.ForEach(player => Global.PlayerList.Add(new Player
        {
            RiotUserName = player.RiotUserName,
            RiotTagLine = player.RiotTagLine,
            RiotUserTag = $"{player.RiotUserName}#{player.RiotTagLine}",
            RiotUserTagEncoded = $"{HttpUtility.UrlEncode($"{player.RiotUserName}")}-{player.RiotTagLine}",
            RoleStats = player.RoleStats,
            SoloDuoQ = player.RankData,
            FlexQ = new RankScore(),
            Winstreak = (int)player.Winstreak,
            Link = $"https://u.gg/lol/profile/{Utils.RegionId(Enum.Parse<Region>(Global.Region.ToUpper()))}/{HttpUtility.UrlEncode($"{player.RiotUserName}")}-{player.RiotTagLine}/overview"
        }));

        Logger.Info(LogModule.LobbyRevealer, $"Players successfully fetched : " +
            $"{Global.PlayerList.Select(p => $"{p.RiotUserName}#{p.RiotTagLine}").Join(" / ")}");
    }

    public static void GetAdvancedPlayersStats()
    {
        Logger.Info(LogModule.LobbyRevealer, $"Fetching advanced stats of {Global.PlayerList.Count} players in background");
        Parallel.ForEach(Global.PlayerList, player => GetPlayerStats(player));
    }

    public static void GetPlayerStats(Player player)
    {
        GetSummonerProfile body = new()
        {
            Variables = new()
            {
                RegionId = Utils.RegionId(Enum.Parse<Region>(Global.Region.ToUpper())),
                RiotUserName = player.RiotUserName,
                RiotTagLine = player.RiotTagLine
            }
        };
        GetSummonerProfileResponse response = JsonConvert.DeserializeObject<GetSummonerProfileResponse>(Requests.WebRequest("POST", Global.UrlAPI, body.ToFormattedString()));
        if (response.Errors != null)
        {
            Logger.Warn(LogModule.LobbyRevealer, $"No advanced stats found for {player.RiotUserName}#{player.RiotTagLine}");
            Logger.Warn(LogModule.LobbyRevealer, "Please check logs for more information");
            Logger.Warn(LogModule.LobbyRevealer, "Please save your logs and send them to the developer");
            return;
        }

        Player currentPlayer = Global.PlayerList.Find(p => p.RiotUserName == response.Data.ProfileInitSimple.PlayerInfo.RiotUserName &&
        p.RiotTagLine == response.Data.ProfileInitSimple.PlayerInfo.RiotTagLine);

        currentPlayer.Level = response.Data.ProfileInitSimple.PlayerInfo.SummonerLevel;
        currentPlayer.FlexQ.Wins = response.Data.FetchProfileRanks.RankScores[1].Wins;
        currentPlayer.FlexQ.Losses = response.Data.FetchProfileRanks.RankScores[1].Losses;
        currentPlayer.FlexQ.Rank = response.Data.FetchProfileRanks.RankScores[1].Rank;
        currentPlayer.FlexQ.Tier = response.Data.FetchProfileRanks.RankScores[1].Tier == "" ? "Unranked" : response.Data.FetchProfileRanks.RankScores[1].Tier;
        currentPlayer.FlexQ.Lp = response.Data.FetchProfileRanks.RankScores[1].Lp;
    }
}