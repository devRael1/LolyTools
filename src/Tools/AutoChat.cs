using Loly.src.Logs;
using Loly.src.Variables;
using Loly.src.Variables.Class;
using Loly.src.Variables.Enums;

using Newtonsoft.Json;

using static Loly.src.Variables.Global;

namespace Loly.src.Tools;

public class AutoChat
{

    public static void HandleChampSelectAutoChat()
    {
        Logger.Info(LogModule.AutoChat, "Fetching Chat information & Summoner ID");
        var myChatProfile = Requests.ClientRequest("GET", "lol-chat/v1/me", true);
        ChatMe chatProfileJson = JsonConvert.DeserializeObject<ChatMe>(myChatProfile[1]);
        var currentChatId = chatProfileJson.Id;

        Logger.Info(LogModule.AutoChat, $"Sending {CurrentSettings.AutoChat.ChatMessages.Count} message(s) in chat");

        var count = 0;
        var timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
        foreach (var message in CurrentSettings.AutoChat.ChatMessages)
        {
            var msg = Utils.FormatMessage(message);

            var attempts = 0;
            var httpRes = "";
            while (httpRes != "200" && attempts < 5)
            {
                var body = "{\"type\":\"chat\",\"fromId\":\"" + currentChatId + "\",\"fromSummonerId\":" + Global.SummonerLogged.SummonerId +
                              ",\"isHistorical\":false,\"timestamp\":\"" + timestamp + "\",\"body\":\"" + msg + "\"}";
                var response = Requests.ClientRequest("POST", "lol-chat/v1/conversations/" + Global.LastChatRoom + "/messages", true, body);
                attempts++;
                httpRes = response[0];
                if (httpRes == "200") count++;

                Thread.Sleep(attempts * 100);
            }
        }

        Logger.Info(LogModule.AutoChat, $"{count} message(s) sended successfully");
    }
}