using Loly.src.Logs;
using Loly.src.Variables;
using Loly.src.Variables.Class;
using Loly.src.Variables.Enums;
using Newtonsoft.Json;

namespace Loly.src.Tools;

public class AutoChat
{

    public static void HandleChampSelectAutoChat()
    {
        Logger.Info(LogModule.AutoChat, "Getting Chat & Summoner ID");
        string[] myChatProfile = Requests.ClientRequest("GET", "lol-chat/v1/me", true);
        ChatMe chatProfileJson = JsonConvert.DeserializeObject<ChatMe>(myChatProfile[1]);
        string currentChatId = chatProfileJson.Id;

        Logger.Info(LogModule.AutoChat, $"Sending {Settings.ChatMessages.Count} message(s) in chat");

        int count = 0;
        string timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
        foreach (string message in Settings.ChatMessages)
        {
            string msg = Utils.FormatMessage(message);

            int attempts = 0;
            string httpRes = "";
            while (httpRes != "200" && attempts < 5)
            {
                string body = "{\"type\":\"chat\",\"fromId\":\"" + currentChatId + "\",\"fromSummonerId\":" + Global.SummonerLogged.SummonerId +
                              ",\"isHistorical\":false,\"timestamp\":\"" + timestamp + "\",\"body\":\"" + msg + "\"}";
                string[] response = Requests.ClientRequest("POST", "lol-chat/v1/conversations/" + Global.LastChatRoom + "/messages", true, body);
                attempts++;
                httpRes = response[0];
                if (httpRes == "200")
                {
                    count++;
                }

                Thread.Sleep(attempts * 200);
            }
        }

        Logger.Info(LogModule.AutoChat, $"{count} message(s) sended successfully");
    }
}