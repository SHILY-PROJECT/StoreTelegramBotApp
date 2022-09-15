using System.Net;
using System.Text;

namespace StoreTelegramBot.CommandCenter.Toolkit;

public class HttpClientFactory
{
    public static HttpClient CreateDefautl(bool allowAutoRedirect = false) =>
        CreateHttpClient(new CookieContainer(), allowAutoRedirect);

    public static HttpClient CreateHttpClient(CookieContainer cookieContainer, bool allowAutoRedirect = false)
    {
        var httpClientHandler = new HttpClientHandler()
        {
            AllowAutoRedirect = allowAutoRedirect,
            MaxAutomaticRedirections = 5,
            AutomaticDecompression = DecompressionMethods.All,
            CookieContainer = cookieContainer
        };

        var defaultHeaders = new Dictionary<string, string>
        {
            { "Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8" },
            { "Accept-Encoding", "gzip, deflate, br" },
            { "Accept-Language", "en-US,en;q=0.9,ru;q=0.8" },
            { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36 Edg/105.0.1343.33" },
            { "Connection", "keep-alive" }
        };

        var client = new HttpClient(httpClientHandler, false);
        client.DefaultRequestHeaders.AddRange(defaultHeaders);

        return client;
    }
}