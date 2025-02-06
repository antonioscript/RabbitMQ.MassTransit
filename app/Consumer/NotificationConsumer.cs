using MassTransit;
using MassTransitApi.Models;
using System.Diagnostics;

namespace MassTransitApi.Consumer
{
    public class NotificationConsumer : IConsumer<NotificationMessage>
    {
        public async Task Consume(ConsumeContext<NotificationMessage> context)
        {
            Debug.WriteLine($"Mensagem recebida: {context.Message.Text}");
            
            await Task.Delay(1000);

            Debug.WriteLine("Processamento concluído!");
        }
    }
}
