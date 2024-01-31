

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.WatermarkWebApp.Services;
using System.Drawing;
using System.Text;
using System.Text.Json;

namespace RabbitMQ.WatermarkWebApp.BackgroundServices
{
    public class ImageWatermarkProcessBackgroundService : BackgroundService
    {
        private readonly RabbitMQClientService _service;
        private readonly ILogger<ImageWatermarkProcessBackgroundService> _logger;
        private IModel _channel;
        public ImageWatermarkProcessBackgroundService(ILogger<ImageWatermarkProcessBackgroundService> logger, RabbitMQClientService service)
        {
            _logger = logger;
            _service = service;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            _channel.BasicConsume(RabbitMQClientService.QueueName,false, consumer);
            consumer.Received += Consumer_Received;
            return Task.CompletedTask;
        }

        private Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            try
            {
                var productImageCreatedEvent = JsonSerializer.Deserialize<ProductImageCreatedEvent>(Encoding.UTF8.GetString(@event.Body.ToArray()));
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", productImageCreatedEvent.ImageName);

                using var img = Image.FromFile(path);
                using var graphic = Graphics.FromImage(img);
                var font = new Font(FontFamily.GenericMonospace, 32, FontStyle.Bold, GraphicsUnit.Pixel);
                var textSize = graphic.MeasureString("www.umutatras@gmail.com", font);
                var color = Color.FromArgb(128, 255, 255, 255);
                var brush = new SolidBrush(color);
                var position = new Point(img.Width - ((int)textSize.Width + 30), img.Height - ((int)textSize.Height + 30));
                graphic.DrawString("www.umutatras@gmail.com", font, brush, position);
                img.Save("wwwroot/images/watermarks/" + productImageCreatedEvent.ImageName);
                img.Dispose();
                graphic.Dispose();
                _channel.BasicAck(@event.DeliveryTag, false);
               
            }
            catch (Exception)
            {

                throw;
            }
            return Task.CompletedTask;

        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {

            return base.StopAsync(cancellationToken);
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _channel = _service.Connect();
            _channel.BasicQos(0, 1, false);

            return base.StartAsync(cancellationToken);
        }
    }
}
