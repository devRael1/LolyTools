using Loly.src.Tasks.Scheduled;
using Loly.src.Variables;
using System.Net.Http.Headers;
using System.Text;

namespace Loly.src.Tools;

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
            if (!string.IsNullOrEmpty(body))
            {
                request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            }

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
            if (request[0][..1] == "2")
            {
                return request;
            }

            if (LeagueClientTask.LeagueClientIsOpen())
            {
                Thread.Sleep(TimeSpan.FromSeconds(3));
            }
            else
            {
                return request;
            }
        }

        return request;
    }

    public static string WebRequest(string url)
    {
        using HttpClient client = new();
        HttpResponseMessage response = client.GetAsync(url).Result;
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        HttpContent responseContent = response.Content;
        string responseString = responseContent.ReadAsStringAsync().Result;
        return responseString;
    }
}