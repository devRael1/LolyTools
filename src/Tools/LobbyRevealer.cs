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
        Logger.Info(LogModule.LobbyRevealer, "Fetching OP.GG token", Global.LogsMenuEnable ? LogType.Both : LogType.File);
        string response = Requests.WebRequest("https://www.op.gg/multisearch");
        OpGGToken = Utils.LrParse(response, "\"buildId\":\"", "\",\"assetPrefix") ?? "null";
        Logger.Info(LogModule.LobbyRevealer, $"Fetching OP.GG token successfully (Token: {OpGGToken})", Global.LogsMenuEnable ? LogType.Both : LogType.File);
    }

    public static void GetPlayers(string req)
    {
        Logger.Info(LogModule.LobbyRevealer, "Fetching Players for revealing lobby...", Global.LogsMenuEnable ? LogType.Both : LogType.File);
        ParticipantsResponse teamPlayers = JsonConvert.DeserializeObject<ParticipantsResponse>(req);

        List<string> cacheNames = (from Participant player in teamPlayers.Participants select $"{player.GameName}#{player.GameTag}").Cast<string>().ToList();
        if (cacheNames.Count == 0)
        {
            Logger.Warn(LogModule.LobbyRevealer, "No players found in the lobby");
            Logger.Warn(LogModule.LobbyRevealer, "Please contact the developer to fix/analyze this issue");
            Logger.Warn(LogModule.LobbyRevealer, "Please save your logs and send them to the developer");
            return;
        }

        string url = $"https://www.op.gg/_next/data/{OpGGToken}/en_US/multisearch/{Global.Region}.json?summoners={string.Join(",", cacheNames.Select(x => x))}&region={Global.Region}";
        string stats = Requests.WebRequest(url);

        MultisearchResponse response = JsonConvert.DeserializeObject<MultisearchResponse>(stats);
        foreach (Summoner sum in response.PageProps.Summoners)
        {
            Player newPlayer = new(sum.Name, sum.Tagline, $"https://www.op.gg/summoners/{Global.Region}/{sum.Name}#{sum.Tagline}")
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
    }

    public static async Task GetAdvancedPlayersStatsAsync()
    {
        Logger.Info(LogModule.LobbyRevealer, $"Getting advanced stats of {Global.PlayerList.Count} players in background...", Global.LogsMenuEnable ? LogType.Both : LogType.File);

        var tasks = Global.PlayerList.Select(player => Task.Run(() =>
        {
            string stats = GetPlayerStats(player);

            // TODO : Check the response of stats variable, there is an error :
            // One or more errors occurred. (Value cannot be null. (Parameter 'value'))
            // 
            // System.AggregateException: One or more errors occurred. (Value cannot be null. (Parameter 'value'))
            //  ---> System.ArgumentNullException: Value cannot be null. (Parameter 'value')
            //    at Newtonsoft.Json.JsonConvert.DeserializeObject(String value, Type type, JsonSerializerSettings settings)
            //    at Newtonsoft.Json.JsonConvert.DeserializeObject[T](String value, JsonSerializerSettings settings)
            //    at Loly.src.Tools.LobbyRevealer.<>c__DisplayClass6_0.<GetAdvancedPlayersStatsAsync>b__1() in D:\Loly tools\src\Tools\LobbyRevealer.cs:line 68
            //    at System.Threading.Tasks.Task.InnerInvoke()
            //    at System.Threading.Tasks.Task.<>c.<.cctor>b__272_0(Object obj)
            //    at System.Threading.ExecutionContext.RunFromThreadPoolDispatchLoop(Thread threadPoolThread, ExecutionContext executionContext, ContextCallback callback, Object state)
            // --- End of stack trace from previous location ---
            //    at System.Threading.ExecutionContext.RunFromThreadPoolDispatchLoop(Thread threadPoolThread, ExecutionContext executionContext, ContextCallback callback, Object state)
            //    at System.Threading.Tasks.Task.ExecuteWithThreadLocal(Task& currentTaskSlot, Thread threadPoolThread)
            // --- End of stack trace from previous location ---
            //    at Loly.src.Tools.LobbyRevealer.GetAdvancedPlayersStatsAsync() in D:\Loly tools\src\Tools\LobbyRevealer.cs:line 84
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
        }));

        await Task.WhenAll(tasks);

        Logger.Info(LogModule.LobbyRevealer, "Advanced stats of all players fetched !", Global.LogsMenuEnable ? LogType.Both : LogType.File);
    }

    public static string GetPlayerStats(Player player)
    {
        string url = $"https://www.op.gg/_next/data/{OpGGToken}/en_US/summoners/{Global.Region}/{player.UserTag}.json?region={Global.Region}&summoner={player.UserTag}";
        string stats = Requests.WebRequest(url);
        return stats;
    }
}