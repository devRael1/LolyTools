namespace Loly.Tools;

public class LanguageChanger
{
    public static void CreateShortcut(string language, string exeFilePath)
    {
        if (!File.Exists(exeFilePath)) return;

        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        string shortcutPath = Path.Combine(desktopPath, $"League Of Legends - {language.Split("_")[1]}.lnk");

        dynamic shell = Activator.CreateInstance(Type.GetTypeFromProgID("WScript.Shell")!);

        dynamic shortcut = shell.CreateShortcut(shortcutPath);
        shortcut.TargetPath = exeFilePath;
        shortcut.Arguments = $"--locale={language}";
        shortcut.WorkingDirectory = Path.GetDirectoryName(exeFilePath);
        shortcut.Save();
    }

    public static string AutoDetectExePath()
    {
        string exePathx86 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
            "Riot Games", "League of Legends", "LeagueClient.exe");
        string exePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
            "Riot Games", "League of Legends", "LeagueClient.exe");

        if (File.Exists(exePath)) return exePath;
        return File.Exists(exePathx86) ? exePathx86 : null;
    }
}