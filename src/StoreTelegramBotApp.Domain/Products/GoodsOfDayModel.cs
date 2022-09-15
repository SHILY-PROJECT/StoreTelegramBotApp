using Newtonsoft.Json;

namespace StoreTelegramBotApp.Domain.Products;

public record GoodsOfDayModel
{
    [JsonProperty("productId")]
    public string? ProductId { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("nameTranslit")]
    public string? NameTranslit { get; set; }

    [JsonProperty("image")]
    public string? Image { get; set; }

    [JsonProperty("images")]
    public string[]? Images { get; set; }

    public DateTime? EndDateOfPromotion { get; set; } = default;
}