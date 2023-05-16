using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using Loly.Variables;
using Newtonsoft.Json;
using static Loly.LeagueClient.Ux;
using static Loly.Tools.AutoAccept;
using static Loly.Tools.PicknBan;

namespace Loly.LeagueClient;

public class Requests
{
    public static string[] ClientRequest(string method, string url, bool useclient, string body = null)
    {
        HttpClientHandler handler = new()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
        try
        {
            using HttpClient client = new(handler);

            string token;
            int port;

            if (useclient)
            {
                port = Convert.ToInt32(Global.AuthClient["port"]);
                token = Global.AuthClient["token"];
            }
            else
            {
                port = Convert.ToInt32(Global.AuthRiot["port"]);
                token = Global.AuthRiot["token"];
            }

            client.BaseAddress = new Uri("https://127.0.0.1:" + port + "/");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token);
            HttpRequestMessage request = new(new HttpMethod(method), url);
            if (!string.IsNullOrEmpty(body)) request.Content = new StringContent(body, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.SendAsync(request).Result;
            int statusCode = (int)response.StatusCode;
            string statusString = statusCode.ToString();
            string responseFromServer = response.Content.ReadAsStringAsync().Result;
            response.Dispose();

            return new[] { statusString, responseFromServer };
        }
        catch
        {
            return new[] { "999", "" };
        }
    }

    public static string[] WaitSuccessClientRequest(string method, string url, bool useclient, string body = null)
    {
        string[] request = { "000", "" };
        while (request[0][..1] != "2")
        {
            request = ClientRequest(method, url, useclient, body);
            if (request[0][..1] == "2") return request;

            if (LeagueClientIsOpen())
                Thread.Sleep(3000);
            else
                return request;
        }

        return request;
    }

    public static string WebRequest(string url)
    {
        using HttpClient client = new();
        HttpResponseMessage response = client.GetAsync(url).Result;
        if (response.IsSuccessStatusCode)
        {
            HttpContent responseContent = response.Content;
            string responseString = responseContent.ReadAsStringAsync().Result;
            return responseString;
        }

        throw new Exception($"The request failed with status code {response.StatusCode}");
    }

    public static void AnalyzeSession()
    {
        while (true)
        {
            START :
            if (!Global.IsLeagueOpen)
            {
                Thread.Sleep(5000);
                goto START;
            }

            string[] gameSession = ClientRequest("GET", "lol-gameflow/v1/session", true);
            if (gameSession[0] == "200")
            {
                dynamic session = JsonConvert.DeserializeObject(gameSession[1]);
                string phase = session.phase;

                string phaseName = Regex.Replace(phase, "(\\B[A-Z])", " $1");
                if (Global.Session != phaseName) Global.Session = phaseName;

                switch (phase)
                {
                    case "Lobby":
                        Thread.Sleep(5000);
                        break;
                    case "Matchmaking":
                        Global.AcceptedCurrentMatch = false;
                        Thread.Sleep(5000);
                        break;
                    case "ReadyCheck":
                        if (Settings.AutoAccept)
                            AutoAcceptQueue();
                        break;
                    case "ChampSelect":
                        // TODO: Créer l'envoie auto de messages (peut etre en système de tache)
                        Global.AcceptedCurrentMatch = false;
                        HandleChampSelect();
                        break;
                    case "InProgress":
                        Thread.Sleep(10000);
                        break;
                    case "WaitingForStats":
                        Thread.Sleep(10000);
                        break;
                    case "PreEndOfGame":
                        Thread.Sleep(10000);
                        break;
                    case "EndOfGame":
                        Thread.Sleep(15000);
                        break;
                    default:
                        Thread.Sleep(5000);
                        break;
                }

                if (phase != "ChampSelect") Global.LastChatRoom = "";
            }

            Thread.Sleep(500);
        }
    }
}