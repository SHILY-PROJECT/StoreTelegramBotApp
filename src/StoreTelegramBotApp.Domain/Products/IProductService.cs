namespace StoreTelegramBotApp.Domain.Products;

public interface IProductService
{
    Task<IEnumerable<GoodsOfDayModel>> GetGoodsOfDayAsync(CancellationToken cancellationToken);
}
