using System.Text;
using StoreTelegramBot.CommandCenter.Toolkit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StoreTelegramBotApp.Domain.Products;

public class ProductService : IProductService
{
    private const string MainUrl = "https://www.mvideo.ru/";

    private readonly HttpClient _http;

    public ProductService()
	{
        _http = HttpClientFactory.CreateDefautl(true);
        _ = this.CollectCookiesAsync();
    }

	public async Task<IEnumerable<GoodsOfDayModel>> GetGoodsOfDayAsync(CancellationToken token)
	{
        string url, content, resp;

        // Запрашиваем айдишники товаров дня
        url = "https://www.mvideo.ru/bff/settings/shelf-product-sets?tags=goodofday&tags=goodofday2&type=daily";
        resp = await this.RequestJsonAsync(url, token);
        var jObj = JObject.Parse(resp);

        var productIdsText = this.GetValueFromJsonProps(jObj, "products");
        var statusEndDateOfPromotion = DateTime.TryParse(this.GetValueFromJsonProps(jObj, "dateTo"), out var endDateOfPromotion);

        var products = JsonConvert.DeserializeObject<string[]>(productIdsText).Select(id => new GoodsOfDayModel
        {
            ProductId = id,
            EndDateOfPromotion = statusEndDateOfPromotion ? endDateOfPromotion : default
        });
        var productIdsForJson = string.Join(",", products.Select(g => $"\"{g.ProductId}\""));

        // Запрашиваем полную инфу по товарам дня
        url = "https://www.mvideo.ru/bff/product-details/list";
        content = $"{{\"productIds\":[{productIdsForJson}],\"mediaTypes\":[\"images\"],\"status\":true,\"category\":true,\"categories\":true,\"brand\":true,\"propertyTypes\":[\"KEY\"]}}";
        resp = await this.SendJsonAsync(url, content, token);
        jObj = JObject.Parse(resp);
        var productsInfoText = this.GetValueFromJsonProps(jObj, "products");
        return JsonConvert.DeserializeObject<GoodsOfDayModel[]>(productsInfoText).Select(p => p with { EndDateOfPromotion = endDateOfPromotion });
    }

    private async Task CollectCookiesAsync()
    {
        var url = "https://www.mvideo.ru/";
        using var msg = new HttpRequestMessage(HttpMethod.Get, url);
        using var resp = await _http.SendAsync(msg);
    }

    private async Task<string> RequestJsonAsync(string url, CancellationToken token)
    {
        using var msg = new HttpRequestMessage(HttpMethod.Get, url);
        using var response = await _http.SendAsync(msg, token);
        return await response.Content.ReadAsStringAsync(token);
    }

    private async Task<string> SendJsonAsync(string url, string stringContent, CancellationToken token)
    {
        using var msg = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(stringContent, Encoding.UTF8, "application/json")
        };
        msg.Headers.AddRangeOrReplace(new()
        {
            { "Accept", "application/json" },
            { "Connection", "keep-alive"},
            { "Host", "www.mvideo.ru" },
            { "Referer", "https://www.mvideo.ru/" },
        });
        using var response = await _http.SendAsync(msg, token);
        return await response.Content.ReadAsStringAsync(token);
    }

    private string? GetValueFromJsonProps(JObject? jObject, string searchedForPropName) => jObject
        ?.Descendants()
        ?.OfType<JProperty>()
        ?.Where(p => p.Name.Equals(searchedForPropName, StringComparison.OrdinalIgnoreCase))
        ?.Select(p => p.Value)
        ?.FirstOrDefault()
        ?.ToString();
}