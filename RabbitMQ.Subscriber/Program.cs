using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared;
using System.Text;
using System.Text.Json;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://csucgwuk:e7d5GUeGxZrQ9OkCeLwT9MzEuFt0evO-@chimpanzee.rmq.cloudamqp.com/csucgwuk");
using var connection=factory.CreateConnection();
var channel = connection.CreateModel();
//channel.QueueDeclare("hello-queue", true, false, false);
//var randomQueue = channel.QueueDeclare().QueueName;
//var randomQueue = "kalici-kuyruk";
//channel.QueueDeclare(randomQueue, true, false, false);
//channel.QueueBind(randomQueue, "logs-faonut", "", null);//kuyruğun consumer bağlantı kopartınca silinmesini sağlar.


channel.BasicQos(0, 1, false);
var consumer=new EventingBasicConsumer(channel);
var queueName = channel.QueueDeclare().QueueName;
//var routeKey = "*.Error.*";

Dictionary<string,object> headers = new Dictionary<string, object>();
headers.Add("format", "pdf");
headers.Add("shape", "a4");
headers.Add("x-match", "all");
channel.QueueBind(queueName,"header-exchange", string.Empty, headers);
channel.BasicConsume(queueName, false, consumer);

consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());
    Console.WriteLine(message);
    Product product=JsonSerializer.Deserialize<Product>(message);
    Console.WriteLine(product.Name);
    //File.AppendAllText("log-critical.txt", message+"\n");
    channel.BasicAck(e.DeliveryTag, false);
};


Console.ReadLine();
    /*
     * Channel queueDeclare
     * durable=true da fiziksel olarak tutulur,false yapılırsa memory resetlendiği zaman kaybolur işlemler 
     * exclusive=true sadece proje içinde erişim sağlanır,false istenilen projeden bağlantı sağlanır.
     * AutoDelete=bağlı olan son subscribe da bağlantısını koparırsa kuyruk silinir.genelde false tercih edilir.
     */