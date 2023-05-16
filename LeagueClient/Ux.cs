using System.Diagnostics;
using System.Management;
using System.Text;
using Loly.Menus.Core;
using Loly.Variables;
using static Loly.Tools.Utils;
using Console = Colorful.Console;

namespace Loly.LeagueClient;

public class Ux
{
    private static int _lcuPid;

    public static void LeagueClientTask()
    {
        GetLeagueAuth();

        while (true)
        {
            Process client = Process.GetProcessesByName("LeagueClientUx").FirstOrDefault();
            if (client != null)
            {
                if (Global.AuthClient.Count == 0 && Global.AuthRiot.Count == 0) GetLeagueAuth();
                Global.Region ??= GetRegion(Requests.WaitSuccessClientRequest("GET", "/riotclient/get_region_locale", true)[1]).ToLower();
                Global.IsLeagueOpen = true;
                if (!_lcuPid.Equals(client.Id)) _lcuPid = client.Id;
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
            Console.Clear();
            Interface.ShowArt();
            Console.WriteLine(" Unable to find League Client running on your computer.\n Press Enter to close application...", Colors.ErrorColor);
            Console.ReadKey();
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

    private static string FindString(string text, string from, string to)
    {
        int pFrom = text.IndexOf(from, StringComparison.Ordinal) + from.Length;
        int pTo = text.LastIndexOf(to, StringComparison.Ordinal);

        return text.Substring(pFrom, pTo - pFrom);
    }
}