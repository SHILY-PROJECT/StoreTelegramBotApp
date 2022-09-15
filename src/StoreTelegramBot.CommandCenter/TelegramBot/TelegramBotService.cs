using StoreTelegramBot.CommandCenter.TelegramBot.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace StoreTelegramBot.CommandCenter.TelegramBot;

public class TelegramBotService
{
    private readonly Dictionary<string, Func<ITelegramBotClient, Update, CancellationToken, Task>> _defCommands;

    private readonly IServiceProvider _serviceProvider;
    private readonly TelegramBotConfiguration _botCfg;
    private readonly TelegramBotClient _bot;
    private readonly MenuCommand _menuCommand;

    public TelegramBotService(IServiceProvider serviceProvider, TelegramBotConfiguration telegramBotConfiguration, MenuCommand menuCommand)
    {
        _serviceProvider = serviceProvider;
        _botCfg = telegramBotConfiguration;
        _bot = new TelegramBotClient(_botCfg.Token);
        _menuCommand = menuCommand;

        _defCommands = new()
        {
            { "/menu", _menuCommand.SendMenu }
        };
    }

    public async Task Start(CancellationToken token) => await Task.Run(() =>
        _bot.StartReceiving(this.HandleUpdateAsync, this.HandlePollingErrorAsync, cancellationToken: token));

    private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken) 
    {
        if (update?.Message?.Text is not string cmd || string.IsNullOrWhiteSpace(cmd)) throw new ArgumentException();

        await (update.Message switch
        {
            _ when _defCommands.TryGetValue(cmd, out var command) => command.Invoke(bot, update, cancellationToken),
            _ when _menuCommand.Commands.TryGetValue(cmd, out var command) => command.Invoke(bot, update, cancellationToken),
            _ => throw new NotImplementedException()
        });
    }

    private Task HandlePollingErrorAsync(ITelegramBotClient bot, Exception ex, CancellationToken cancellationToken) => Task.Run(() =>
    {
        Console.WriteLine(ex.Message);
    });
}