using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace zedvance.telegrambot.api.Core.Entities
{
    public class Flow
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, Func<string, string>> Question { get; set; }

    }

    public class FlowQuestions
    {
        public const string AppEntryFlow_SERVICE_OPTIONS = "AppEntryFlow_SERVICE_OPTIONS";
        public const string FeedbackFlow_NAME_REQUEST = "FeedbackFlow_NAME_REQUEST";
        public const string FeedbackFlow_AGE_REQUEST = "FeedbackFlow_AGE_REQUEST";
        public const string FeedbackFlow_FEEDBACK = "FeedbackFlow_FEEDBACK";
        public const string FeedbackFlow_END = "FeedbackFlow_END";
    }

    public static class CurrentSessionHandler
    {
        public static KeyValuePair<Question, Action<Update>> CurrentQuestion;
        public static KeyValuePair<Question, Action<Update>> NextQuestion = new KeyValuePair<Question, Action<Update>>(null, null);

        public static void EndSession(Update update)
        {
            AppEntryFlow.InitRequestHandler(update);
        }
    }

    public static class AppEntryFlow
    {
        static TelegramBotClient botClient = new TelegramBotClient("1290024489:AAEEW4XWhk5zESY3eWuxR-tZzXATboNwEG0");

        public static Dictionary<Question, Action<Update>> Questions = new Dictionary<Question, Action<Update>>
        {
            {new Question(FlowQuestions.AppEntryFlow_SERVICE_OPTIONS), ServiceOptionsRequestHandler},
        };

        public static async void InitRequestHandler(Update update)
        {
            var response = new StringBuilder();
            response.AppendLine("👋Hey there! What would you like to do today.");
            var replyMarkup = new InlineKeyboardMarkup(new List<InlineKeyboardButton>{
                InlineKeyboardButton.WithCallbackData("📣Give feedback", "1"),
                InlineKeyboardButton.WithCallbackData("Contact us", "2"),
                InlineKeyboardButton.WithCallbackData("Apply for a loan", "3")
            });
            // response.AppendLine("[1]. 📣Give Feedback");
            // Console.WriteLine(response.ToString());
            await botClient.SendTextMessageAsync(
                chatId: update.Message?.Chat,
                text: response.ToString(),
                replyMarkup: replyMarkup
            );
            CurrentSessionHandler.NextQuestion = Questions.First();

        }
        public static void ServiceOptionsRequestHandler(Update update)
        {
            if(update.CallbackQuery.Data == "1")
            {
                FeedbackFlow.InitRequestHandler(update);
            }
            else
                FeedbackFlow.InvalidSelectionHandler(update);

        }
    }

    public static class FeedbackFlow
    {
        static TelegramBotClient botClient = new TelegramBotClient("1290024489:AAEEW4XWhk5zESY3eWuxR-tZzXATboNwEG0");

        public static Dictionary<Question, Action<Update>> Questions = new Dictionary<Question, Action<Update>>
        {
            {new Question(FlowQuestions.FeedbackFlow_NAME_REQUEST), NameRequestHandler},
            {new Question(FlowQuestions.FeedbackFlow_AGE_REQUEST), AgeRequestHandler},
            {new Question(FlowQuestions.FeedbackFlow_FEEDBACK), FeedbackRequestHandler},
            {new Question(FlowQuestions.FeedbackFlow_END, isEndSignal: true), AppEntryFlow.InitRequestHandler},
        };

        public static async void InvalidSelectionHandler(Update update)
        {
            await botClient.SendTextMessageAsync(
                chatId: update.Message?.Chat,
                text: "You've made an innvalid selection. Please try again?"
            );
        }
        public static async void InitRequestHandler(Update update)
        {
            CurrentSessionHandler.NextQuestion = Questions.First();
            await botClient.SendTextMessageAsync(
                chatId: update.CallbackQuery.Message?.Chat,
                text: "What is your name?"
            );


        }
        public static async void NameRequestHandler(Update update)
        {
            CurrentSessionHandler.NextQuestion = Questions.First(x => x.Key.NormalizedName == FlowQuestions.FeedbackFlow_AGE_REQUEST);
            await botClient.SendTextMessageAsync(
                chatId: update.Message?.Chat,
                text: "How old are you?"
            );

        }
        public static async void AgeRequestHandler(Update update)
        {
            CurrentSessionHandler.NextQuestion = Questions.First(x => x.Key.NormalizedName == FlowQuestions.FeedbackFlow_FEEDBACK);
            await botClient.SendTextMessageAsync(
                chatId: update.Message?.Chat,
                text: "What's your feedback?"
            );
        }
        public static async void FeedbackRequestHandler(Update update)
        {
            CurrentSessionHandler.NextQuestion = Questions.First(x => x.Key.NormalizedName == FlowQuestions.FeedbackFlow_END);
            await botClient.SendTextMessageAsync(
                chatId: update.Message?.Chat,
                text: "Thanks for your feedback?"
            );
        }
    }

    public class Question
    {
        public Question()
        {
        }
        public Question(string normalizedName, bool isEndSignal = false)
        {
            NormalizedName = normalizedName;
            IsEndSignal = isEndSignal;
        }
        public string NormalizedName { get; set; }
        public bool IsEndSignal { get; set; }
    }


}

