using Loly.src.Tools;
using Loly.src.Variables;
using static Loly.src.Tools.LobbyRevealer;

namespace Loly.src.Tasks.Scheduled
{
    public class LobbyRevealingTask
    {
        public static void GetLobbyRevealing()
        {
            if (!Settings.LobbyRevealer || Global.Session != "ChampSelect" || Global.FetchedPlayers)
            {
                return;
            }

            Global.PlayerList.Clear();
            if (OpGGToken == null)
            {
                GetTokenOpGg();
            }

            GetPlayers(Requests.ClientRequest("GET", "/chat/v5/participants/lol-champ-select", false)[1]);
            Task.Run(GetAdvancedPlayersStats);

            Global.FetchedPlayers = true;
        }
    }
}
