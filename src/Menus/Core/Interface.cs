using Loly.src.Variables;
using Loly.src.Variables.Class;
using static Loly.src.Tools.Utils;
using Console = Colorful.Console;

namespace Loly.src.Menus.Core;

public class Interface
{
    public const string ArtName = @"
██╗      ██████╗ ██╗  ██╗   ██╗    ████████╗ ██████╗  ██████╗ ██╗     ███████╗    
██║     ██╔═══██╗██║  ╚██╗ ██╔╝    ╚══██╔══╝██╔═══██╗██╔═══██╗██║     ██╔════╝    
██║     ██║   ██║██║   ╚████╔╝        ██║   ██║   ██║██║   ██║██║     ███████╗    
██║     ██║   ██║██║    ╚██╔╝         ██║   ██║   ██║██║   ██║██║     ╚════██║    
███████╗╚██████╔╝███████╗██║          ██║   ╚██████╔╝╚██████╔╝███████╗███████║    
╚══════╝ ╚═════╝ ╚══════╝╚═╝          ╚═╝    ╚═════╝  ╚═════╝ ╚══════╝╚══════╝    ";

    public const string ArtNameLogs = @"
██╗      ██████╗ ██╗  ██╗   ██╗    ██╗      ██████╗  ██████╗ ███████╗
██║     ██╔═══██╗██║  ╚██╗ ██╔╝    ██║     ██╔═══██╗██╔════╝ ██╔════╝
██║     ██║   ██║██║   ╚████╔╝     ██║     ██║   ██║██║  ███╗███████╗
██║     ██║   ██║██║    ╚██╔╝      ██║     ██║   ██║██║   ██║╚════██║
███████╗╚██████╔╝███████╗██║       ███████╗╚██████╔╝╚██████╔╝███████║
╚══════╝ ╚═════╝ ╚══════╝╚═╝       ╚══════╝ ╚═════╝  ╚═════╝ ╚══════╝";

    public static void ShowArt()
    {
        string[] array = ArtName.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        uint num2 = 0;
        while (num2 < array.Length)
        {
            string text2 = array[num2];
            Console.SetCursorPosition((Console.WindowWidth - text2.Length) / 2, Console.CursorTop);
            Console.WriteLine(text2, Colors.PrimaryColor);
            num2 += 1;
        }

        const string text3 = "League Of Legends";
        Console.SetCursorPosition((Console.WindowWidth - text3.Length) / 2, Console.CursorTop);
        Console.Write("L", Colors.PrimaryColor);
        Console.Write("eague ", Colors.InfoColor);
        Console.Write("O", Colors.PrimaryColor);
        Console.Write("f ", Colors.InfoColor);
        Console.Write("L", Colors.PrimaryColor);
        Console.Write("egends", Colors.InfoColor);
        Console.Write(Environment.NewLine);
        string text4 = $"Loly Tools by {Global.SoftAuthor}";
        Console.SetCursorPosition((Console.WindowWidth - text4.Length) / 2, Console.CursorTop);
        Console.Write("L", Colors.PrimaryColor);
        Console.Write("oly - ", Colors.InfoColor);
        Console.Write("T", Colors.PrimaryColor);
        Console.Write("ools by ", Colors.InfoColor);
        Console.Write($"{Global.SoftAuthor}", Colors.PrimaryColor);
        Console.Write(Environment.NewLine);
        ShowLoggedUser();
        Console.Write(Environment.NewLine);
        Console.Write(Environment.NewLine);

        TopLength = array.Length + 4;
    }

    public static void ShowLogsArt()
    {
        string[] array = ArtNameLogs.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        uint num2 = 0;
        while (num2 < array.Length)
        {
            string text2 = array[num2];
            Console.SetCursorPosition((Console.WindowWidth - text2.Length) / 2, Console.CursorTop);
            Console.WriteLine(text2, Colors.PrimaryColor);
            num2 += 1;
        }

        const string text1 = "Loly Tools Logs";
        Console.SetCursorPosition((Console.WindowWidth - text1.Length) / 2, Console.CursorTop);
        Console.Write("L", Colors.PrimaryColor);
        Console.Write("oly ", Colors.InfoColor);
        Console.Write("T", Colors.PrimaryColor);
        Console.Write("ools ", Colors.InfoColor);
        Console.Write("L", Colors.PrimaryColor);
        Console.Write("ogs", Colors.InfoColor);
        Console.Write(Environment.NewLine);
        ShowLoggedUser();
        Console.Write(Environment.NewLine);
        Console.Write(Environment.NewLine);

        TopLength = array.Length + 4;
    }

    private static void ShowLoggedUser()
    {
        if (Global.SummonerLogged.SummonerId != null)
        {
            string text = $"Logged in as {Global.SummonerLogged.GetFullGameName()} [Summoner ID : {Global.SummonerLogged.SummonerId}]";
            Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2, Console.CursorTop);
            Console.Write("Logged in as ", Colors.InfoColor);
            Console.Write(Global.SummonerLogged.GetFullGameName(), Colors.PrimaryColor);
            Console.Write(" [Summoner ID : ", Colors.InfoColor);
            Console.Write(Global.SummonerLogged.SummonerId, Colors.PrimaryColor);
            Console.Write("]", Colors.InfoColor);
        }
        else
        {
            string text = "Not logged in for the moment";
            Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2, Console.CursorTop);
            Console.Write("Not logged in for the moment", Colors.InfoColor);
        }
    }
}