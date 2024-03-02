using Loly.src.Logs;
using Loly.src.Tools;
using Loly.src.Variables;
using Loly.src.Variables.Class;
using Loly.src.Variables.Enums;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Management;
using System.Text;
using static Loly.src.Tools.Utils;
using Console = Colorful.Console;

namespace Loly.src.Tasks.Scheduled
{
    public class LeagueClientTask
    {
        private static int _lcuPid;

        public static void LolClientTask()
        {
            if (LeagueClientIsOpen())
            {
                Global.IsLeagueOpen = true;

                Process client = Process.GetProcessesByName("LeagueClientUx").FirstOrDefault();
                if (Global.AuthClient.Count == 0 && Global.AuthRiot.Count == 0)
                {
                    GetLeagueAuth();
                }

                LoadSummonerId(false);

                if (Global.Region == "")
                {
                    string response = Requests.WaitSuccessClientRequest("GET", "/riotclient/region-locale", true)[1];
                    PlayerRegion regionSplit = JsonConvert.DeserializeObject<PlayerRegion>(response);
                    Global.Region = regionSplit.Region.ToLower();
                }

                if (!_lcuPid.Equals(client.Id))
                {
                    _lcuPid = client.Id;
                }
            }
            else
            {
                Global.IsLeagueOpen = false;
                Global.AuthRiot.Clear();
                Global.AuthClient.Clear();
                Global.SummonerLogged = new CurrentSummoner();
                _lcuPid = 0;
            }
        }

        public static bool LeagueClientIsOpen()
        {
            Process client = Process.GetProcessesByName("LeagueClientUx").FirstOrDefault();
            return client != null;
        }

        private static void GetLeagueAuth(bool clearAuth = false)
        {
            if (clearAuth)
            {
                Global.AuthRiot.Clear();
                Global.AuthClient.Clear();
            }

            try
            {
                string commandline = Cmd("LeagueClientUx.exe");

                Global.AuthRiot.Add("port", FindString(commandline, "--riotclient-app-port=", "\" \"--no-rads"));
                Global.AuthRiot.Add("token",
                    Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes("riot:" + FindString(commandline, "--riotclient-auth-token=", "\" \"--riotclient"))));

                Global.AuthClient.Add("port", FindString(commandline, "--app-port=", "\" \"--install"));
                Global.AuthClient.Add("token",
                    Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                        .GetBytes("riot:" + FindString(commandline, "--remoting-auth-token=", "\" \"--respawn-command=LeagueClient.exe"))));
            }
            catch
            {
                ResetConsole();
                Console.WriteLine(" Unable to find League Client running on your computer.\n Press Enter to close application...", Colors.ErrorColor);
                _ = Console.ReadKey();
                Environment.Exit(0);
            }
        }

        private static string Cmd(string gamename)
        {
            string commandline = "";
            ManagementClass mngmtClass = new("Win32_Process");
            foreach (ManagementBaseObject managementBaseObject in mngmtClass.GetInstances())
            {
                ManagementObject o = (ManagementObject)managementBaseObject;
                if (o["Name"].Equals(gamename))
                {
                    commandline = "[" + o["CommandLine"] + "]";
                }
            }

            return commandline;
        }

        private static void LoadSummonerId(bool force = false)
        {
            if (Global.SummonerLogged?.SummonerId != null && !force)
            {
                return;
            }

            Logger.Info(LogModule.Loly, "Getting your summoner ID...", Global.LogsMenuEnable ? LogType.Both : LogType.File);
            string[] currentSummoner = Requests.WaitSuccessClientRequest("GET", "lol-summoner/v1/current-summoner", true);
            dynamic currentSummonerSplit = JsonConvert.DeserializeObject(currentSummoner[1]);

            Global.SummonerLogged.SummonerId = currentSummonerSplit["summonerId"];
            Global.SummonerLogged.DisplayName = currentSummonerSplit["displayName"];
            Global.SummonerLogged.GameName = currentSummonerSplit["gameName"];
            Global.SummonerLogged.TagLine = currentSummonerSplit["tagLine"];
            Global.SummonerLogged.SummonerLevel = currentSummonerSplit["summonerLevel"];
            Global.SummonerLogged.AccountId = currentSummonerSplit["accountId"];
            Global.SummonerLogged.Puuid = currentSummonerSplit["puuid"];

            Logger.Info(LogModule.Loly, $"Summoner ID loaded : {Global.SummonerLogged.SummonerId}", Global.LogsMenuEnable ? LogType.Both : LogType.File);
        }
    }
}
