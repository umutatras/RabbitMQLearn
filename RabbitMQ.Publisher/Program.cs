// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using Shared;
using System.Text;
using System.Text.Json;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://csucgwuk:e7d5GUeGxZrQ9OkCeLwT9MzEuFt0evO-@chimpanzee.rmq.cloudamqp.com/csucgwuk");
using var connection = factory.CreateConnection();
var channel = connection.CreateModel();
//channel.QueueDeclare("hello-queue", true, false, false); direkt kuyruk oluşturma 

/*
 * exchange publisher yayınladığı verileri kkuyruğa yönlendiren araçtır
 */
//channel.ExchangeDeclare("logs-faonut", durable: true, type: ExchangeType.Fanout);//faonut exhange oluşturma
//channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Direct);//direct exhange oluşturma
//channel.ExchangeDeclare("logs-topic", durable: true, type: ExchangeType.Topic);
channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);
Dictionary<string,object> headers=new Dictionary<string, object>();
headers.Add("format", "pdf");
headers.Add("shape", "a4");
var properties = channel.CreateBasicProperties();
properties.Headers=headers;
properties.Persistent = true;
var product = new Product() { Description="asdasd",Id=1,Name="asd"};
var productJsonString=JsonSerializer.Serialize(product);
channel.BasicPublish("header-exchange", string.Empty, properties, Encoding.UTF8.GetBytes(productJsonString));
//Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
//{
//    //var queueName = $"direct-queue-{x}";fanout için kuyruk oluşturma
//    //channel.QueueDeclare(queueName, true, false, false);
//    //channel.QueueBind(queueName, "logs-direct", routeKey, null);


//});
//Enumerable.Range(1, 50).ToList().ForEach(x =>
//{
//    //fanout için 
//    //LogNames log = (LogNames)new Random().Next(1, 4);
//    //string message = $"log-type: {log}";
//    //var messageBody = Encoding.UTF8.GetBytes(message);

//    ////var routeKey = $"route-{log}";
//    //channel.BasicPublish("logs-direct", routeKey, null, messageBody);
//   // Console.WriteLine($"Log gönderilmiştir:{message}");

//   //topic için
//    Random rnd = new Random();
//    LogNames log1 = (LogNames)rnd.Next(1, 5);
//    LogNames log2 = (LogNames)rnd.Next(1, 5);
//    LogNames log3 = (LogNames)rnd.Next(1, 5);
//    var routeKey = $"{log1}.{log2}.{log3}";
//    string message = $"log-type: {log1}-{log2}-{log3}";
//    var messageBody = Encoding.UTF8.GetBytes(message);
//    channel.BasicPublish("logs-topic", routeKey, null, messageBody);
//    Console.WriteLine($"Log gönderilmiştir:{message}");


//});



Console.ReadLine();

public enum LogNames
{
    Critical=1,
    Error=2,
    Warning=3,
    Info=4
}