using System.IO.Compression;
using System.Management;
using System.Text;

using Loly.src.Variables;
using Loly.src.Variables.Class;
using Loly.src.Variables.Enums;

using Newtonsoft.Json;

namespace Loly.src.Logs;

public class AutoSendLogs
{
    public static void ErrorReporter(Exception exception)
    {
        var zipPath = Path.Combine(Logger.LogFolder, $"{DateTime.Now.Month:00}-{DateTime.Now.Day:00}-{DateTime.Now.Year:00}.zip");
        if (File.Exists(zipPath)) File.Delete(zipPath);
        ZipFile.CreateFromDirectory(
            Path.Combine(Logger.LogFolder, $"{DateTime.Now.Month:00}-{DateTime.Now.Day:00}-{DateTime.Now.Year:00}"),
            Path.Combine(Logger.LogFolder, $"{DateTime.Now.Month:00}-{DateTime.Now.Day:00}-{DateTime.Now.Year:00}.zip")
        );
        // Check if ZIP file is more than 8MB
        if (new FileInfo(zipPath).Length > 8000000)
        {
            Logger.Info(LogModule.Loly, "ZIP file is more than 8MB, not sending to Discord Wehbook for analyze");
            return;
        }

        var wv = (from x in new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>()
                  select x.GetPropertyValue("Caption")).FirstOrDefault();
        var windowsVersion = wv != null ? wv.ToString() : "Unknown";

        var json = JsonConvert.SerializeObject(new
        {
            username = $"Errors -> New Exception Detected !",
            embeds = new[]
            {
                new
                {
                    color = 16711680,
                    title = "New Exception Detected !",
                    description = $"New exception detected in **{Version.FullVersion}** !\n" +
                    $"Please check the logs for more information.\n" +
                    $"\n**Exception Message :** ```{exception.Message}```" +
                    $"\n**Exception Stack Trace :** ```{exception.StackTrace}```" +
                    $"\n**Exception Source :** ```{exception.Source}```" +
                    $"\n**Exception Object Data :** ```{exception.GetObjectData}```" +
                    $"\n**Windows Information :** ```{Environment.OSVersion.VersionString}\n{windowsVersion}```"
                }
            }
        });

        using var httpClient = new HttpClient();
        var form = new MultipartFormDataContent
        {
            { new StringContent(json, Encoding.UTF8, "application/json"), "payload_json" },
            { new ByteArrayContent(File.ReadAllBytes(zipPath)), "file", $"{DateTime.Now.Month:00}-{DateTime.Now.Day:00}-{DateTime.Now.Year:00}.zip" }
        };

        Logger.Request(new Request { Method = "POST", Url = Global.DiscordWebhook, Body = JsonConvert.SerializeObject(form) });

        try
        {
            HttpResponseMessage response = httpClient.PostAsync(Global.DiscordWebhook, form).GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();
            Logger.Request(new Response
            {
                Method = "POST",
                Url = Global.DiscordWebhook,
                StatusCode = (int)response.StatusCode,
                Data = new[] { response.StatusCode.ToString(), response.Content.ReadAsStringAsync().GetAwaiter().GetResult() }
            });
        }
        catch (HttpRequestException ex)
        {
            Logger.Request(new Response { Method = "POST", Url = Global.DiscordWebhook, StatusCode = Convert.ToInt32(ex.StatusCode), Exception = ex });
        }

        File.Delete(zipPath);
    }
}
