using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;

namespace Acompanhamento.Infrastructure.Broker
{
    public class BrokerConsumer
    {
        readonly IBrokerConnection _brokerConnection;     

        public BrokerConsumer(IBrokerConnection brokerConnection)
        {
            _brokerConnection = brokerConnection;     
        }

        public void BrokerStartConsumer<T>(string queueName, string exchange, string routingKey, Action<T> callback)
        {
            var channel = _brokerConnection.CreateChannel();

            channel.ExchangeDeclare(exchange: exchange,
                                    type: ExchangeType.Topic,
                                    durable: true,
                                    autoDelete: false);

            channel.QueueDeclare(queueName, true, false, false, null);

            channel.QueueBind(queue: queueName,
                                exchange: exchange,
                                routingKey: routingKey);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;
                
                try
                {
                    T deserializedObject = JsonConvert.DeserializeObject<T>(message);

                    if (deserializedObject != null)
                    {
                        callback(deserializedObject);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao desserializar a mensagem: {ex.Message}");
                }

                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: queueName,
                                    autoAck: false,
                                    consumer: consumer);
        }
    }
}