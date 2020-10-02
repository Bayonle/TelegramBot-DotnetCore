using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using zedvance.telegrambot.api.Core.Entities;

namespace zedvance.telegrambot.api.Core.Application
{
    public class HandleIncomingMessageCommand : IRequest
    {
        public Update Update { get; set; }
    }

    public class BotResponse
    {
        public string Text { get; set; }
        public IReplyMarkup ReplyMarkup { get; set; } = null;
        public BotResponse(string text, IReplyMarkup replyMarkup = null)
        {
            Text = text;
            ReplyMarkup = replyMarkup;
        }
    }

    public class HandleIncomingMessageCommandHandler : IRequestHandler<HandleIncomingMessageCommand>
    {
        public Task<Unit> Handle(HandleIncomingMessageCommand request, CancellationToken cancellationToken)
        {
            if(new List<string>{"hey", "/start"}.Contains(request.Update.Message?.Text, StringComparer.CurrentCultureIgnoreCase))
            {
                AppEntryFlow.InitRequestHandler(request.Update);
                return Task.FromResult(Unit.Value);
            }

            if (CurrentSessionHandler.NextQuestion.Equals(default(KeyValuePair<Question, Action<string>>)))
            {
                return Task.FromResult(Unit.Value);
            }

            if(CurrentSessionHandler.NextQuestion.Key == null)
            {
                return Task.FromResult(Unit.Value);
            }

            if (CurrentSessionHandler.NextQuestion.Key.IsEndSignal || CurrentSessionHandler.NextQuestion.Value == null)
            {
                CurrentSessionHandler.EndSession(request.Update);
                return Task.FromResult(Unit.Value);

            }

            HandleResponse(request.Update);
            return Task.FromResult(Unit.Value);
        }


        public void HandleResponse(Update update)
        {
            CurrentSessionHandler.NextQuestion.Value.Invoke(update);
        }
    }
}