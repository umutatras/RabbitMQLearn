using ClosedXML.Excel;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.FileExcelCreateService.Models;
using RabbitMQ.FileExcelCreateService.Services;
using Shared;
using System.Data;
using System.Text;
using System.Text.Json;

namespace RabbitMQ.FileExcelCreateService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly RabbitMQClientService _rabbitmqClientService;
        private readonly IServiceProvider _serviceProvider;
        private IModel _channel;

        public Worker(ILogger<Worker> logger, RabbitMQClientService rabbitmqClientService, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _rabbitmqClientService = rabbitmqClientService;
            _serviceProvider = serviceProvider;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _channel = _rabbitmqClientService.Connect();
            _channel.BasicQos(0, 1, false);


            return base.StartAsync(cancellationToken);
        }
        protected override  Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer=new AsyncEventingBasicConsumer(_channel);
            _channel.BasicConsume(RabbitMQClientService.QueueName,false,consumer);
            consumer.Received += Consumer_Received;
            return Task.CompletedTask;
            
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            await Task.Delay(3000);
            var createExcelMessage = JsonSerializer.Deserialize<CreateExcelMessage>(Encoding.UTF8.GetString(@event.Body.ToArray()));
            using var ms=new MemoryStream();

            var wb = new XLWorkbook();
            var ds = new DataSet();
            ds.Tables.Add(GetTable("products"));
            wb.Worksheets.Add(ds);
            wb.SaveAs(ms);

            MultipartFormDataContent multipartFormDataContent = new();
            multipartFormDataContent.Add(new ByteArrayContent(ms.ToArray()),"file",Guid.NewGuid().ToString()+".xlsx");
            var baseUrl = "https://localhost:7059/api/file";

            using var  httpClient=new HttpClient();
            var response=await httpClient.PostAsync($"{baseUrl}?fileId={createExcelMessage.FileId}", multipartFormDataContent);
            if(response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"File( Id: {createExcelMessage.FileId} baþarýlý þekilde oluþtu");
                _channel.BasicAck(@event.DeliveryTag, false);
            }


        }

        private DataTable GetTable(string tableName)
        {
            List<Models.Product> products;
            using var scope=_serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<UdemyBlogDbContext>();
            products = context.Products.ToList();

            DataTable table = new DataTable() {TableName=tableName };

            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("ContentSummary", typeof(string));
            table.Columns.Add("ContentMain", typeof(string));

            products.ForEach(x =>
            {
                table.Rows.Add(x.Id, x.Title, x.ContentSummary, x.ContentMain);
            });
            return table;
        }
    }
}
