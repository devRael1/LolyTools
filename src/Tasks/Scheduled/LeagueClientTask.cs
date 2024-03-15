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
                if (Global.AuthClient.Count == 0 && Global.AuthRiot.Count == 0) GetLeagueAuth();
                if (Global.SummonerLogged.SummonerId == null) LoadSummonerId();

                if (Global.Region == "")
                {
                    Logger.Info(LogModule.Loly, "Fetching region of your League Of Legends Client");
                    string response = Requests.WaitSuccessClientRequest("GET", "/riotclient/region-locale", true)[1];
                    PlayerRegion regionSplit = JsonConvert.DeserializeObject<PlayerRegion>(response);
                    Global.Region = regionSplit.Region.ToLower();
                    Logger.Info(LogModule.Loly, $"Region fetched successfully : {Global.Region.ToUpper()}");
                }

                if (!_lcuPid.Equals(client.Id)) _lcuPid = client.Id;
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
                Logger.Info(LogModule.Loly, "Unable to find League Client running on your computer");
                Logger.Info(LogModule.Loly, "Press Enter to close application...");
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
                if (o["Name"].Equals(gamename)) commandline = "[" + o["CommandLine"] + "]";
            }

            return commandline;
        }

        private static void LoadSummonerId()
        {
            Logger.Info(LogModule.Loly, "Fetching your Summoner ID");

            string[] currentSummoner = Requests.WaitSuccessClientRequest("GET", "lol-summoner/v1/current-summoner", true);
            CurrentSummoner currentSum = JsonConvert.DeserializeObject<CurrentSummoner>(currentSummoner[1]);

            Global.SummonerLogged.SummonerId = currentSum.SummonerId;
            Global.SummonerLogged.DisplayName = currentSum.DisplayName;
            Global.SummonerLogged.GameName = currentSum.GameName;
            Global.SummonerLogged.TagLine = currentSum.TagLine;
            Global.SummonerLogged.SummonerLevel = currentSum.SummonerLevel;
            Global.SummonerLogged.AccountId = currentSum.AccountId;
            Global.SummonerLogged.Puuid = currentSum.Puuid;

            Logger.Info(LogModule.Loly, $"Logged Summoner ID loaded : {Global.SummonerLogged.SummonerId}");
        }
    }
}
