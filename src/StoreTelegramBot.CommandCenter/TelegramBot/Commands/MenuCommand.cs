using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using StoreTelegramBotApp.Domain.Products;

namespace StoreTelegramBot.CommandCenter.TelegramBot.Commands;

public class MenuCommand
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IProductService _productService;

    public MenuCommand(IServiceProvider serviceProvider, IProductService productService)
    {
        _serviceProvider = serviceProvider;
        _productService = productService;

        Commands = new()
        {
            { "💯 Товар дня 💯", CommandTopOfDayAsync  },
            { "📈 Самые продаваемые товары 📈", CommandMostSoldProductsAsync },
            { "🎉 Самые большие скидки 🎉", CommandBiggestSavingsAsync },
            { "🎁 Промокод 🎁", CommandPromoCodeAsync }
        };
    }

    public Dictionary<string, Func<ITelegramBotClient, Update, CancellationToken, Task>> Commands { get; init; }

    public async Task SendMenu(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        var rkm = new ReplyKeyboardMarkup(this.Commands.Keys.Select(k => new KeyboardButton[] { new(k) }));
        await bot.SendTextMessageAsync(update.Message!.Chat.Id, "Menu", replyMarkup: rkm, cancellationToken: cancellationToken);
    }

    protected async Task CommandTopOfDayAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        var produncts = await _productService.GetGoodsOfDayAsync(cancellationToken);

        var msgArr = produncts.Select(p =>
            $"ID: {p.ProductId}{Environment.NewLine}" +
            $"NAME: {p.Name}{Environment.NewLine}" +
            $"NAME TRANSLIT: {p.NameTranslit}{Environment.NewLine}" +
            $"IMAGE: {p.Image}{Environment.NewLine}" +
            $"END DATE OF PROMOTION: {p.EndDateOfPromotion:MM/dd/yyyy HH:mm:ss}{Environment.NewLine}");

        await bot.SendTextMessageAsync(update.Message!.Chat.Id, string.Join(new string('=', 50) + Environment.NewLine, msgArr), cancellationToken: cancellationToken);
    }

    protected async Task CommandMostSoldProductsAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        
    }

    protected async Task CommandBiggestSavingsAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {

    }

    protected async Task CommandPromoCodeAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {

    }
}