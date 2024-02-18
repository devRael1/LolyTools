﻿using Loly.src.Logs;
using Loly.src.Variables;
using Loly.src.Variables.Class;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Management;
using System.Text;
using static Loly.src.Tools.Utils;
using Console = Colorful.Console;

namespace Loly.src.LeagueClient;

public class Ux
{
    private static int _lcuPid;

    public static void LeagueClientTask()
    {
        if (LeagueClientIsOpen())
        {
            GetLeagueAuth();
            LoadSummonerId();
        }

        while (true)
        {
            Process client = Process.GetProcessesByName("LeagueClientUx").FirstOrDefault();
            if (client != null)
            {
                if (Global.AuthClient.Count == 0 && Global.AuthRiot.Count == 0)
                {
                    GetLeagueAuth();
                }

                if (Global.Summoner.SummonerId == null)
                {
                    LoadSummonerId();
                }

                Global.Region ??= GetRegion(Requests.WaitSuccessClientRequest("GET", "/riotclient/get_region_locale", true)[1]).ToLower();
                Global.IsLeagueOpen = true;
                if (!_lcuPid.Equals(client.Id))
                {
                    _lcuPid = client.Id;
                }
            }
            else
            {
                Global.IsLeagueOpen = false;
            }

            Thread.Sleep(15000);
        }
    }

    public static bool LeagueClientIsOpen()
    {
        Process client = Process.GetProcessesByName("LeagueClientUx").FirstOrDefault();
        return client != null;
    }

    private static void GetLeagueAuth()
    {
        Global.IsLeagueOpen = false;
        Global.AuthRiot.Clear();
        Global.AuthClient.Clear();

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

    private static string FindString(string text, string from, string to)
    {
        int pFrom = text.IndexOf(from, StringComparison.Ordinal) + from.Length;
        int pTo = text.LastIndexOf(to, StringComparison.Ordinal);

        return text[pFrom..pTo];
    }

    private static void LoadSummonerId()
    {
        if (Global.Summoner?.SummonerId != null)
        {
            return;
        }

        Logger.Info(LogModule.Loly, "Getting your summoner ID...", true);
        string[] currentSummoner = Requests.WaitSuccessClientRequest("GET", "lol-summoner/v1/current-summoner", true);
        dynamic currentSummonerSplit = JsonConvert.DeserializeObject(currentSummoner[1]);

        Global.Summoner.SummonerId = currentSummonerSplit["summonerId"];
        Global.Summoner.DisplayName = currentSummonerSplit["displayName"];
        Global.Summoner.GameName = currentSummonerSplit["gameName"];
        Global.Summoner.SummonerLevel = currentSummonerSplit["summonerLevel"];
        Global.Summoner.AccountId = currentSummonerSplit["accountId"];
        Global.Summoner.Puuid = currentSummonerSplit["puuid"];

        Logger.Info(LogModule.Loly, $"Summoner ID loaded : {Global.Summoner.SummonerId}", true);
    }
}