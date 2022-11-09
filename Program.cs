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
            var chatId = message.Chat.Id;


            Console.WriteLine($"Start listening for @{message.Chat.FirstName}");

            Console.WriteLine($"{message.Chat.FirstName}   |   {message.Text}");
            await botClient.SendTextMessageAsync(message.Chat.Id, $"Welcome to Duga, la tortuga bot, {message.Chat.FirstName}.");


            if (message.Photo != null)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Your image has been uploaded successfully.");
                return;
            }

            if (message.Document != null)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "The document has been uploaded successfully.");

                var fileId = update.Message.Document.FileId;
                var fileInfo = await botClient.GetFileAsync(fileId);
                var filePath = fileInfo.FilePath;

                // Console.WriteLine(fileId, fileInfo, filePath);

                string destinationFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\{message.Document.FileName}";
                await using FileStream fileStream = System.IO.File.OpenWrite(destinationFilePath);
                await botClient.DownloadFileAsync(filePath, fileStream);
                fileStream.Close();

                Process.Start(@"C:\Users\Diego\Desktop\prueba.exe", $@"""{destinationFilePath}""");
                await Task.Delay(1500);

                await using Stream stream = System.IO.File.OpenRead(destinationFilePath);
                await botClient.SendDocumentAsync(message.Chat.Id, new InputOnlineFile(stream, message.Document.FileName.Replace(".jpg", ".jpeg")));

                return;
            }
            if (message.Text.ToLower().Contains("Contact"))    // TO DO: complete this snippet
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Please, send me a random contact from your phone and I will send you another one from my agenda.");
                if (message.Contact != null)
                {
                    await botClient.SendContactAsync(message.Chat.Id, phoneNumber: "+1234567890", firstName: "Darth", lastName: "Vader");
                }
            }

            if (message.Text != null) // TO DO: verify this snippet exits properly
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
                    chatId: chatId,
                    text: "How old are you?",
                    replyMarkup: replyKeyboardMarkup,
                    cancellationToken: cancellationToken);

                await Task.Delay(5000); // TO DO: wait until user's answer.
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