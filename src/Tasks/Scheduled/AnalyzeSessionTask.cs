using Loly.src.Tools;
using Loly.src.Variables;

namespace Loly.src.Tasks.Scheduled
{
    public class AnalyzeSessionTask
    {
        public static void AnalyzeSession()
        {
            while (true)
            {
                if (!Global.IsLeagueOpen)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                }
                else
                {
                    string[] gameSession = Requests.ClientRequest("GET", "lol-gameflow/v1/gameflow-phase", true);
                    if (gameSession[0] == "200")
                    {
                        string phase = gameSession[1].Replace("\\", "").Replace("\"", "");
                        if (Global.Session != phase)
                        {
                            Global.Session = phase;
                        }

                        switch (phase)
                        {
                            case "Lobby":
                                Global.FetchedPlayers = false;
                                Global.AcceptedCurrentMatch = false;
                                Thread.Sleep(TimeSpan.FromSeconds(5));
                                break;
                            case "Matchmaking":
                                Global.FetchedPlayers = false;
                                Global.AcceptedCurrentMatch = false;
                                Thread.Sleep(TimeSpan.FromSeconds(5));
                                break;
                            case "ReadyCheck":
                                Global.FetchedPlayers = false;
                                if (Settings.AutoAccept)
                                {
                                    AutoAccept.AutoAcceptQueue();
                                }
                                break;
                            case "ChampSelect":
                                {
                                    Global.AcceptedCurrentMatch = false;
                                    if (Settings.AutoChat || Settings.PicknBan)
                                    {
                                        PicknBan.HandleChampSelect();
                                    }
                                }
                                break;
                            case "InProgress":
                                Thread.Sleep(TimeSpan.FromSeconds(10));
                                break;
                            case "WaitingForStats":
                                Thread.Sleep(TimeSpan.FromSeconds(10));
                                break;
                            case "PreEndOfGame":
                                Thread.Sleep(TimeSpan.FromSeconds(10));
                                break;
                            case "EndOfGame":
                                Thread.Sleep(TimeSpan.FromSeconds(15));
                                break;
                            case "None":
                                Thread.Sleep(TimeSpan.FromSeconds(5));
                                break;
                            default:
                                Thread.Sleep(TimeSpan.FromSeconds(10));
                                break;
                        }

                        if (phase != "ChampSelect")
                        {
                            Global.LastChatRoom = "";
                        }
                    }
                }
            }
        }
    }
}
