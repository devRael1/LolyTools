using Loly.src.Logs;
using Loly.src.Variables;
using Loly.src.Variables.Class;
using Loly.src.Variables.Enums;

using Newtonsoft.Json;

using static Loly.src.Tools.Utils;

namespace Loly.src;

public class Updater
{
    private static string GetLatestVersionFromGithub(string owner, string repo)
    {
        var apiUrl = $"https://api.github.com/repos/{owner}/{repo}/releases/latest";
        using HttpClient client = new();
        client.DefaultRequestHeaders.UserAgent.ParseAdd(
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");

        HttpResponseMessage httpResponse = client.GetAsync(apiUrl).Result;
        httpResponse.EnsureSuccessStatusCode();
        return httpResponse.Content.ReadAsStringAsync().Result;
    }

    public static void CheckUpdate()
    {
        Console.Title = $"{Global.SoftName} - Checking for updates...";
        Logger.Info(LogModule.Updater, "[1/5] Fetching version info from Github repository...", LogType.Console);

        try
        {
            var response = GetLatestVersionFromGithub("devRael1", "LolyTools");
            if (response.Length == 0) throw new Exception("Unable to fetch the latest version from Github");

            UpdaterResponse responseUpdater = JsonConvert.DeserializeObject<UpdaterResponse>(response);

            Logger.Info(LogModule.Updater, "[2/5] Checking versions...", LogType.Console);
            Console.Write(Environment.NewLine);

            if (Version.FullVersionNoStage == responseUpdater.TagName)
            {
                Logger.Info(LogModule.Updater, "Up to date !", LogType.Console);
                Logger.Info(LogModule.Updater, "You are using the latest version of the software", LogType.Console);
                Logger.Info(LogModule.Updater, "Press any key to start application...", LogType.Console);
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Logger.Info(LogModule.Updater, "Update available :", LogType.Console);
                Logger.Info(LogModule.Updater, $"[x] Your version : {Version.FullVersionNoStage}", LogType.Console);
                Logger.Info(LogModule.Updater, $"[x] Latest version : {responseUpdater.TagName}", LogType.Console);
                Logger.Info(LogModule.Updater, "Press any key to download latest version...", LogType.Console);
                Console.ReadKey();

                Asset update = responseUpdater.Assets[0];
                Console.Write(Environment.NewLine);

                Logger.Info(LogModule.Updater, $"[3/5] Downloading the updated software (v{responseUpdater.TagName})...", LogType.Console);
                Logger.Info(LogModule.Updater, "[4/5] Fetching download stats...", LogType.Console);

                Console.Write(Environment.NewLine);

                Logger.Info(LogModule.Updater, "Download Stats:", LogType.Console);
                Logger.Info(LogModule.Updater, $"  [x] Size: {FormatBytes(update.Size, false)}", LogType.Console);
                Logger.Info(LogModule.Updater, $"  [x] Downloading Count: {update.DownloadCount}", LogType.Console);
                Logger.Info(LogModule.Updater, $"  [x] Release Date: {update.CreatedAt}", LogType.Console);

                var path = Path.Combine(Environment.CurrentDirectory, $"Loly Tools - v{responseUpdater.TagName}.exe");

                using (HttpClient client = new())
                {
                    using HttpResponseMessage responseMessage = client.GetAsync(update.BrowserDownloadUrl).Result;
                    using HttpContent content = responseMessage.Content;
                    using Stream stream = content.ReadAsStreamAsync().Result;
                    using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None);
                    stream.CopyTo(fileStream);
                }

                Console.Write(Environment.NewLine);
                Logger.Info(LogModule.Updater, "[5/5] Done !", LogType.Console);
                Logger.Info(LogModule.Updater, $"[!] Note: You can delete this executable and use the latest : Loly Tools - v{responseUpdater.TagName}.exe", LogType.Console);
                Logger.Info(LogModule.Updater, "Press any key to exit...", LogType.Console);
                Console.ReadKey();
                Console.Clear();
                Environment.Exit(0);
            }
        }
        catch (Exception ex)
        {
            Logger.Error(LogModule.Updater, $"An error has occurred in the verification/recovery of the Loly Tools version", ex, LogType.Both);
            Logger.Error(LogModule.Updater, $"The error has been logged in the logs", null, LogType.Both);
            Logger.Error(LogModule.Updater, "Please try again later or contact the developer", null, LogType.Both);
            Logger.Error(LogModule.Updater, "Press any key to exit...", null, LogType.Both);
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}