using System.Net.Http.Headers;
using System.Text;

using Loly.src.Logs;
using Loly.src.Tasks.Scheduled;
using Loly.src.Variables;
using Loly.src.Variables.Class;

namespace Loly.src.Tools;

public static class Requests
{
    public static string[] ClientRequest(string method, string url, bool useclient, string body = null)
    {
        HttpClientHandler handler = new()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
        try
        {
            using var client = new HttpClient(handler);

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

            HttpResponseMessage response = client.SendAsync(request).GetAwaiter().GetResult();
            var statusCode = (int)response.StatusCode;
            var statusString = statusCode.ToString();
            var responseFromServer = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();

            Logger.Request(new Response { Method = method, Url = url, StatusCode = statusCode, Data = new[] { statusString, responseFromServer } });

            response.Dispose();
            return new[] { statusString, responseFromServer };
        }
        catch (HttpRequestException ex)
        {
            Logger.Request(new Response { Method = method, Url = url, StatusCode = Convert.ToInt32(ex.StatusCode), Exception = ex });
            return new[] { "999", "" };
        }
        catch (Exception ex)
        {
            Logger.Request(new Response { Method = method, Url = url, StatusCode = 0, Exception = ex });
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

    public static string WebRequest(string method, string url, string body = null, bool logResponse = true)
    {
        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent
                .ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
            client.BaseAddress = new Uri(url);
            HttpRequestMessage request = new(new HttpMethod(method), url);
            if (!string.IsNullOrEmpty(body))
            {
                request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            }

            Logger.Request(new Request { Method = method, Url = url, Body = body });

            HttpResponseMessage response = client.SendAsync(request).GetAwaiter().GetResult();
            HttpContent responseContent = response.Content;
            var responseString = responseContent.ReadAsStringAsync().GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();

            Logger.Request(new Response
            {
                Method = method,
                Url = url,
                StatusCode = (int)response.StatusCode,
                Data = new[] { response.StatusCode.ToString(), logResponse ? responseString : "RESPONSE NOT LOGGED" }
            });

            return responseString;
        }
        catch (HttpRequestException ex)
        {
            Logger.Request(new Response { Method = method, Url = url, StatusCode = Convert.ToInt32(ex.StatusCode), Exception = ex });
            return null;
        }
        catch (Exception ex)
        {
            Logger.Request(new Response { Method = method, Url = url, StatusCode = 0, Exception = ex });
            return null;
        }
    }
}