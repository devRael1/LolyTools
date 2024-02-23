using Loly.src.Tools;
using Loly.src.Variables;
using static Loly.src.Tools.LobbyRevealer;

namespace Loly.src.Tasks
{
    public class LobbyRevealingTask
    {
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
                if (OpGGToken == null)
                {
                    GetTokenOpGg();
                }

                Thread.Sleep(3000);
                GetPlayers(Requests.ClientRequest("GET", "/chat/v5/participants/lol-champ-select", false)[1]);
                _ = Task.Run(GetAdvancedPlayersStats);

                Global.FetchedPlayers = true;
            }
        }
    }
}
