using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.FileExcelCreateService;
using RabbitMQ.FileExcelCreateService.Models;
using RabbitMQ.FileExcelCreateService.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((host,services) =>
    {
        services.AddHostedService<Worker>();
        var rabbitmq = host.Configuration.GetConnectionString("SqlServer");
        services.AddDbContext<UdemyBlogDbContext>(options =>
        {
            options.UseSqlServer("Server=UMUT;database=UdemyBlogDb;integrated security=true;TrustServerCertificate=True");
        });
        services.AddSingleton<RabbitMQClientService>();
        services.AddSingleton(sp => new ConnectionFactory() { Uri = new Uri("amqps://csucgwuk:e7d5GUeGxZrQ9OkCeLwT9MzEuFt0evO-@chimpanzee.rmq.cloudamqp.com/csucgwuk"), DispatchConsumersAsync = true });


    })
    .Build();

await host.RunAsync();
