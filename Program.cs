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

            await botClient.SendTextMessageAsync(message.Chat.Id, "Bienvenido al canal de Duga, la tortuga");

            if (message.Text != null)
            {
                Console.WriteLine($"{message.Chat.FirstName}  |   {message.Text}"); // It shows the username and the text via console.
                if (message.Text.ToLower().Contains(""))
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Amazing!");
                    await Task.Delay(1000); // miliseconds
                    await botClient.SendTextMessageAsync(message.Chat.Id, "What's your name?");

                    if (message.Text != null)
                    {
                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] // TO DO: corregir
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
                    }
                    //message.Text;
                    //return;



                }
            }
            else if (message.Photo != null)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Your image has been uploaded successfully.");
                return;
            }
            else if (message.Document != null)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "the document has been uploaded successfully.");

                var fileId = update.Message.Document.FileId;
                var fileInfo = await botClient.GetFileAsync(fileId);
                var filePath = fileInfo.FilePath;

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
            else if (message.Text.ToLower().Contains("Contact"))    // TO DO: corregir
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Please, send me a random contact from your phone and I will send you another one from my agenda.");
                if (message.Contact != null)
                {
                    await botClient.SendContactAsync(message.Chat.Id, phoneNumber: "+1234567890", firstName: "Darth", lastName: "Vader");
                }
            }
        }

        private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }
    }
}