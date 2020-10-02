using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using zedvance.telegrambot.api.Core.Application;

namespace zedvance.telegrambot.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ServicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("receive")]
        public async Task<IActionResult> Receive([FromBody]Update update)
        {
            var botClient = new TelegramBotClient("1290024489:AAEEW4XWhk5zESY3eWuxR-tZzXATboNwEG0");
            //message type for plain text messages, callbackquery for callback buttons
            // if(update.Type == UpdateType.CallbackQuery)
            // {
            //     await botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id, showAlert:false);
            // }

            if(update.Type == UpdateType.Message || update.Type == UpdateType.CallbackQuery)
            {
                var response = await _mediator.Send(new HandleIncomingMessageCommand
                {
                    Update = update,
                });
            }
            return Ok();
        }
    }
}