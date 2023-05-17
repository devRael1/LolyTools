﻿using Loly.LeagueClient;
using Loly.Variables;
using Newtonsoft.Json;
using static Loly.Logs;

namespace Loly.Tools;

public class AutoChat
{
    private static string _currentChatId = "";

    public static void HandleChampSelectAutoChat()
    {
        GetChatId();

        Log(LogType.AutoChat, $"Sending {Settings.ChatMessages.Count} messages in chat...");
        string timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
        foreach (string message in Settings.ChatMessages)
        {
            string msg = FormatMessage(message);

            int attempts = 0;
            string httpRes = "";
            while (httpRes != "200" && attempts < 5)
            {
                string body = "{\"type\":\"chat\",\"fromId\":\"" + _currentChatId + "\",\"fromSummonerId\":" + Global.CurrentSummonerId +
                              ",\"isHistorical\":false,\"timestamp\":\"" + timestamp + "\",\"body\":\"" + msg + "\"}";
                string[] response = Requests.ClientRequest("POST", "lol-chat/v1/conversations/" + Global.LastChatRoom + "/messages", true, body);
                attempts++;
                httpRes = response[0];
                Thread.Sleep(attempts * 100);
            }
        }

        Log(LogType.AutoChat, "Messages sended successfully !");
    }

    public static string FormatMessage(string message)
    {
        return message.Replace("\"", "'").Replace("\\", "");
    }

    private static void GetChatId()
    {
        Log(LogType.AutoChat, "Getting chat & summoner id...");
        string[] myChatProfile = Requests.ClientRequest("GET", "lol-chat/v1/me", true);
        dynamic chatProfileJson = JsonConvert.DeserializeObject(myChatProfile[1]);
        _currentChatId = chatProfileJson.id;
        Global.CurrentSummonerId = chatProfileJson.summonerId;
    }
}