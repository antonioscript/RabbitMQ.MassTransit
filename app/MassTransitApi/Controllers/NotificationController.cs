using MassTransit;
using MassTransitApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MassTransitApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly IBus _bus;

        public NotificationController(IBus bus)
        {
            _bus = bus;
        }

        [HttpPost]
        public async Task<IActionResult> SendNotification([FromBody] NotificationMessage message)
        {
            if (message is null || string.IsNullOrWhiteSpace(message.Text))
                return BadRequest("Mensagem inválida!");

            await _bus.Publish(message);

            return Ok($"Mensagem enviada: {message.Text}");
        }
    }
}
