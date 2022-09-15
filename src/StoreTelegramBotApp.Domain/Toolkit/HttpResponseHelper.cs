using System.Text;

namespace StoreTelegramBot.CommandCenter.Toolkit;

public class HttpResponseHelper
{
    public static string ExtractContentAsString(HttpResponseMessage httpResponseMessage, string encoding = "iso-8859-1")
    {
        using var content = httpResponseMessage.Content.ReadAsStream();
        using var streamReader = new StreamReader(content, Encoding.GetEncoding(encoding));
        return streamReader.ReadToEnd();
    }

    public static async Task<string> ExtractContentAsStringAsync(HttpResponseMessage httpResponseMessage, string encoding = "iso-8859-1")
    {
        using var content = await httpResponseMessage.Content.ReadAsStreamAsync();
        using var streamReader = new StreamReader(content, Encoding.GetEncoding(encoding));
        return await streamReader.ReadToEndAsync();
    }
}