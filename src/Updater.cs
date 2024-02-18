using Loly.src.Logs;
using Loly.src.Variables;
using Loly.src.Variables.Class;
using Newtonsoft.Json;
using System.Net;
using Console = Colorful.Console;

namespace Loly.src;

public class Updater
{
    private static string GetLatestVersionFromGithub(string owner, string repo)
    {
        string apiUrl = $"https://api.github.com/repos/{owner}/{repo}/releases/latest";
        string response = string.Empty;

        using HttpClient client = new();
        client.DefaultRequestHeaders.UserAgent.ParseAdd(
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");

        HttpResponseMessage httpResponse = client.GetAsync(apiUrl).Result;
        if (httpResponse.StatusCode != HttpStatusCode.OK)
        {
            return response;
        }

        response = httpResponse.Content.ReadAsStringAsync().Result;
        return response;
    }

    public static void CheckUpdate()
    {
        Console.Title = $"{Global.SoftName} - Checking for updates...";
        Console.WriteLine(" [1/5] Fetching version info from remote... ", Colors.InfoColor);

        try
        {
            string response = GetLatestVersionFromGithub("devRael1", "LolyTools");
            if (response.Length == 0)
            {
                throw new Exception("Unable to fetch the latest version from Github.");
            }

            dynamic json = JsonConvert.DeserializeObject(response);
            string version = json.tag_name;

            Console.WriteLine(" [2/5] Checking versions... ", Colors.InfoColor);
            Console.Write(Environment.NewLine);

            if (Version.FullVersionNoStage == version)
            {
                Console.WriteLine(" Up to date", Colors.SuccessColor);
                Console.WriteLine(" [x] You are using the latest version of the software.", Colors.SuccessColor);
                Console.WriteLine(" Press any key to continue...", Colors.SuccessColor);
                _ = Console.ReadKey();
                Console.Clear();
                Thread.Sleep(2000);
            }
            else
            {
                Console.WriteLine(" Update available:", Colors.WarningColor);
                Console.WriteLine($" [x] Your version: {Version.FullVersionNoStage}", Colors.WarningColor);
                Console.WriteLine($" [x] Latest version: {version}", Colors.WarningColor);
                Console.WriteLine(" Press any key to download latest version...", Colors.WarningColor);
                _ = Console.ReadKey();

                dynamic update = json["assets"][0];
                int megaBytes = update["size"] / (1 * 1000);
                int downloadCount = update["download_count"];
                string releaseDate = update["created_at"];

                Console.Write(Environment.NewLine);

                Console.WriteLine($" [3/5] Downloading the updated software (v{version})... ", Colors.InfoColor);
                Console.WriteLine(" [4/5] Fetching download stats... ", Colors.InfoColor);

                Console.Write(Environment.NewLine);

                Console.WriteLine(" Download Stats:", Colors.InfoColor);
                Console.WriteLine($"  [x] Size: {megaBytes} KB", Colors.InfoColor);
                Console.WriteLine($"  [x] Downloading Count: {downloadCount}", Colors.InfoColor);
                Console.WriteLine($"  [x] Release Date: {releaseDate}", Colors.InfoColor);

                string url = update["browser_download_url"];
                string path = Path.Combine(Environment.CurrentDirectory, $"Loly Tools - v{version}.exe");

                using (HttpClient client = new())
                {
                    using HttpResponseMessage responseMessage = client.GetAsync(url).Result;
                    using HttpContent content = responseMessage.Content;
                    using Stream stream = content.ReadAsStreamAsync().Result;
                    using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None);
                    stream.CopyTo(fileStream);
                }

                Console.Write(Environment.NewLine);
                Console.WriteLine(" [5/5] Done !", Colors.InfoColor);
                Console.WriteLine($" [!] Note: You can delete this executable and use the latest (Loly Tools - v{version}.exe).", Colors.InfoColor);
                Console.WriteLine(" Press any key to exit...", Colors.InfoColor);
                _ = Console.ReadKey();
                Console.Clear();
                Environment.Exit(0);
            }
        }
        catch (Exception ex)
        {
            Logger.Error(LogModule.Updater, $"An error has occurred in the verification/recovery of the Loly Tools version.", ex);

            Console.WriteLine(" Failed", Colors.ErrorColor);
            Console.WriteLine($" [x] {ex.Message}", Colors.ErrorColor);
            Console.WriteLine(" Press any key to exit...", Colors.ErrorColor);
            _ = Console.ReadKey();
            Environment.Exit(0);
        }
    }
}