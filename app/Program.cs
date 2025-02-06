using MassTransit;
using MassTransitApi.Consumer;
using MassTransitApi.Models;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Carregar configurações do appsettings.json
var rabbitMqSettings = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMqSettings>();

// Add services to the container.

builder.Services.AddControllers();

//Add Config RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<NotificationConsumer>(); // Registra o consumidor

    x.UsingRabbitMq((context, cfg) =>
    {
        // Usar as configurações do RabbitMQ
        cfg.Host(rabbitMqSettings.Host, rabbitMqSettings.VirtualHost, h =>
        {
            h.Username(rabbitMqSettings.Username);
            h.Password(rabbitMqSettings.Password);
        });

        // Configura o exchange diretamente com o nome do appsettings.json
        cfg.ExchangeType = ExchangeType.Direct; // Tipo de exchange (pode ser Fanout, Direct, Topic)

        // Configurando a fila usando o nome do appsettings.json
        cfg.ReceiveEndpoint(rabbitMqSettings.QueueName, e =>
        {
            e.ConfigureConsumer<NotificationConsumer>(context); 
            e.Bind(rabbitMqSettings.ExchangeName, x =>
            {
                x.RoutingKey = rabbitMqSettings.RoutingKey; // Defina a chave de roteamento, se necessário
            });
        });
    });
});


// Adicionando serviços do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Adiciona o Swagger

var app = builder.Build();


// Habilitar o Swagger somente no ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Habilita o Swagger
    app.UseSwaggerUI(); // Habilita a interface do Swagger
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
