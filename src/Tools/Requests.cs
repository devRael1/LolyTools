using Loly.src.Logs;
using Loly.src.Tasks.Scheduled;
using Loly.src.Variables;
using Loly.src.Variables.Class;
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

            Logger.Request(new Request { Method = method, Url = url, Body = body });

            HttpResponseMessage response = client.SendAsync(request).Result;
            int statusCode = (int)response.StatusCode;
            string statusString = statusCode.ToString();
            string responseFromServer = response.Content.ReadAsStringAsync().Result;
            response.EnsureSuccessStatusCode();

            Logger.Request(new Response { Method = method, StatusCode = statusCode, Data = new[] { statusString, responseFromServer } });

            response.Dispose();
            return new[] { statusString, responseFromServer };
        }
        catch (HttpRequestException ex)
        {
            Logger.Request(new Response { Method = method, StatusCode = Convert.ToInt32(ex.StatusCode), Exception = ex });
            return new[] { "999", "" };
        }
        catch (Exception ex)
        {
            Logger.Request(new Response { Method = method, StatusCode = 0, Exception = ex });
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

    public static string WebRequest(string url, bool logResponse = true)
    {
        try
        {
            using HttpClient client = new();

            Logger.Request(new Request { Method = "GET", Url = url });

            HttpResponseMessage response = client.GetAsync($"https://{url}").Result;
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            HttpContent responseContent = response.Content;
            string responseString = responseContent.ReadAsStringAsync().Result;
            response.EnsureSuccessStatusCode();

            Logger.Request(new Response
            {
                Method = "GET",
                StatusCode = (int)response.StatusCode,
                Data = new[] { response.StatusCode.ToString(), logResponse ? responseString : "NOT LOGGED" }
            });

            return responseString;
        }
        catch (HttpRequestException ex)
        {
            Logger.Request(new Response { Method = "GET", StatusCode = Convert.ToInt32(ex.StatusCode), Exception = ex });
            return null;
        }
        catch (Exception ex)
        {
            Logger.Request(new Response { Method = "GET", StatusCode = 0, Exception = ex });
            return null;
        }
    }
}