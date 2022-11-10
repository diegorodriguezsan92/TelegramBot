using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

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

        async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var message = update.Message;

            if (message.Text != null)
            {
                Console.WriteLine($"{message.Chat.FirstName}  |   {message.Text}");
                if (message.Text.ToLower().Contains("Hi"))
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Bienvenido al bot de Duga, la tortuga!");
                    return;
                }
            }

            if (message.Photo != null)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "La imagen se ha cargado correctamente.");
                return;
            }

            if (message.Document != null)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "El documento se ha cargado correctamente.");

                // Downloading files
                var fileId = update.Message.Document.FileId;
                var fileInfo = await botClient.GetFileAsync(fileId);
                var filePath = fileInfo.FilePath;

                string destinationFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\{message.Document.FileName}";
                await using FileStream fileStream = System.IO.File.OpenWrite(destinationFilePath);
                await botClient.DownloadFileAsync(filePath, fileStream);
                fileStream.Close();

                Process.Start(@"C:\Users\Diego\Desktop\prueba.exe", $@"""{destinationFilePath}""");
                await Task.Delay(2000);

                // Uploading files
                await using Stream stream = System.IO.File.OpenRead(destinationFilePath);
                await botClient.SendDocumentAsync(message.Chat.Id, new InputOnlineFile(stream, message.Document.FileName.Replace(".jpg", "(edited).jpg")));

                return;
            }

            // Answers with buttons
            if (message.Text.ToLower().Contains("Age"))
            {

                ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                {
                    new KeyboardButton[] { "<18", "19-30" },
                    new KeyboardButton[] { "31-50", "51-70" },
                    new KeyboardButton[] { ">70", "NC" }
                })
                {
                    ResizeKeyboard = true
                };

                Message sentMessage1 = await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "How old are you?",
                    replyMarkup: replyKeyboardMarkup,
                    cancellationToken: cancellationToken);

                await Task.Delay(5000); // TODO: wait until user's answer.
                await botClient.SendTextMessageAsync(message.Chat.Id, "Amazing!");
                return;
            }

        }
        private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }
    }
}