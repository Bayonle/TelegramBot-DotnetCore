using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Args;
using zedvance.telegrambot.api.Core.Application;

namespace zedvance.telegrambot.api
{
    public class Program
    {
        static TelegramBotClient botClient = new TelegramBotClient("1290024489:AAEEW4XWhk5zESY3eWuxR-tZzXATboNwEG0");

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            // var configuration = Configure();
            // ConsoleDIFactory.ConfigureServices(configuration);
            // // ConfigureLogger();

            // Console.WriteLine("starting..");

            // var me = botClient.GetMeAsync().Result;
            // Console.WriteLine(
            //   $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            // );

            // botClient.OnMessage += Bot_OnMessage;
            // botClient.StartReceiving();

            // Console.WriteLine("Press any key to exit");
            // Console.ReadLine();

            // botClient.StopReceiving();
        }

        public static IConfiguration Configure()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }


        // static async void Bot_OnMessage(object sender, MessageEventArgs e)
        // {
        //     if (e.Message.Text != null)
        //     {
        //         Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.");
        //         var _mediator = ConsoleDIFactory.Services.GetRequiredService<IMediator>();

        //         var response = await _mediator.Send(new HandleIncomingMessageCommand { Message = e.Message.Text });

        //         await botClient.SendTextMessageAsync(
        //           chatId: e.Message.Chat,
        //           text: response
        //         );
        //     }
        // }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

    }
}
