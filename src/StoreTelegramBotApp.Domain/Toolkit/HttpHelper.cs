using System.Net.Http.Headers;
using System.Text;

namespace StoreTelegramBot.CommandCenter.Toolkit;

public static class HttpHelper
{
    public static void AddRange(this HttpRequestHeaders requestHeaders, Dictionary<string, string> header, bool skipIfValueEmptyOrNull = false)
    {
        foreach (var kv in header)
        {
            if (skipIfValueEmptyOrNull && string.IsNullOrWhiteSpace(kv.Value)) continue;
            requestHeaders.Add(kv.Key, kv.Value);
        }
    }

    public static void AddRangeOrReplace(this HttpRequestHeaders requestHeaders, Dictionary<string, string> headers, bool skipIfValueEmptyOrNull = false)
    {
        foreach (var kv in headers)
        {
            if (skipIfValueEmptyOrNull && string.IsNullOrWhiteSpace(kv.Value)) continue;
            if (requestHeaders.Contains(kv.Key)) requestHeaders.Remove(kv.Key);
            requestHeaders.Add(kv.Key, kv.Value);
        }
    }

    public static void AddOrReplace(this HttpRequestHeaders requestHeaders, string name, string value, bool skipIfValueEmptyOrNull = false)
    {
        if (skipIfValueEmptyOrNull && string.IsNullOrWhiteSpace(value)) return;
        if (requestHeaders.Contains(name)) requestHeaders.Remove(name);
        requestHeaders.Add(name, value);
    }

    public static async Task<string> ExtractAsStringAsync(this HttpContent content)
    {
        using var streamReader = new StreamReader(await content.ReadAsStreamAsync(), Encoding.GetEncoding("windows-1251"));
        return await streamReader.ReadToEndAsync();
    }

    public static string ExtractAsString(this HttpContent content)
    {
        using var streamReader = new StreamReader(content.ReadAsStream(), Encoding.GetEncoding("windows-1251"));
        return streamReader.ReadToEnd();
    }
}