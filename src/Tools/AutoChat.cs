using Loly.src.LeagueClient;
using Loly.src.Logs;
using Loly.src.Variables;
using Loly.src.Variables.Enums;
using Newtonsoft.Json;

namespace Loly.src.Tools;

public class AutoChat
{
    private static string _currentChatId = "";

    public static void HandleChampSelectAutoChat()
    {
        GetChatId();

        Logger.Info(LogModule.AutoChat, $"Sending {Settings.ChatMessages.Count} message(s) in chat...");

        int count = 0;
        string timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
        foreach (string message in Settings.ChatMessages)
        {
            string msg = FormatMessage(message);

            int attempts = 0;
            string httpRes = "";
            while (httpRes != "200" && attempts < 5)
            {
                string body = "{\"type\":\"chat\",\"fromId\":\"" + _currentChatId + "\",\"fromSummonerId\":" + Global.Summoner.SummonerId +
                              ",\"isHistorical\":false,\"timestamp\":\"" + timestamp + "\",\"body\":\"" + msg + "\"}";
                string[] response = Requests.ClientRequest("POST", "lol-chat/v1/conversations/" + Global.LastChatRoom + "/messages", true, body);
                attempts++;
                httpRes = response[0];
                if (httpRes == "200")
                {
                    count++;
                }

                Thread.Sleep(attempts * 100);
            }
        }

        Logger.Info(LogModule.AutoChat, $"{count} message(s) sended successfully !");
    }

    public static string FormatMessage(string message)
    {
        return message.Replace("\"", "'").Replace("\\", "");
    }

    private static void GetChatId()
    {
        Logger.Info(LogModule.AutoChat, "Getting chat & summoner id...");
        string[] myChatProfile = Requests.ClientRequest("GET", "lol-chat/v1/me", true);
        dynamic chatProfileJson = JsonConvert.DeserializeObject(myChatProfile[1]);
        _currentChatId = chatProfileJson.id;
        Global.Summoner.SummonerId = chatProfileJson.summonerId;
    }
}