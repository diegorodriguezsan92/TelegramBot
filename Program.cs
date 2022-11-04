using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new TelegramBotClient("5502798964:AAE-gplEitgIDipgGFHIaLny8CW7lLvJuyU");
            client.StartReceiving(Update, Error);
            Console.ReadLine();
        }

        async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            var message = update.Message;
            if (message.Text != null)
            {
                if (message.Text.ToLower().Contains("a"))
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Estupendo!");
                    return;
                }
            }
            if (message.Photo != null)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "El archivo se ha cargado correctamente.");
                return;
            }
        }

        private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }

    }
}