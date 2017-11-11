using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;

namespace MeasurementDeviceConsumer.RabbitMQ
{
    public class RabbitMQConsumer
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        
        private const string ExchangeName = "Topic_Exchange";
        private const string DeviceMeasurementQueueName = "DeviceMeasurementTopic_Queue";

        public void CreateConnection()
        {
            _factory = new ConnectionFactory {
                HostName = "localhost",
                UserName = "guest", Password = "guest" };            
        }

        public void Close()
        {
            throw new NotFiniteNumberException("Not implemented");
        }

        public void ProcessMessages()
        {
            using (_connection = _factory.CreateConnection())
            {
                using (var channel = _connection.CreateModel())
                {
                    Console.WriteLine("Listening for Topic <measurement.devicemeasurement>");
                    Console.WriteLine("-----------------------------------------");
                    Console.WriteLine();

                    channel.ExchangeDeclare(ExchangeName, "topic");
                    channel.QueueDeclare(DeviceMeasurementQueueName, 
                        true, false, false, null);

                    channel.QueueBind(DeviceMeasurementQueueName, ExchangeName, 
                        "measurement.devicemeasurement");

                    channel.BasicQos(0, 10, false);
                    Subscription subscription = new Subscription(channel, 
                        DeviceMeasurementQueueName, false);
                    
                    while (true)
                    {
                        BasicDeliverEventArgs deliveryArguments = subscription.Next();

                        var message = 
                            (DeviceMeasurement)deliveryArguments.Body.DeSerialize(typeof(DeviceMeasurement));

                        var routingKey = deliveryArguments.RoutingKey;

                        Console.WriteLine("--- Measurement - Routing Key <{0}> : {1} : {2}", routingKey, message.DeviceNumber, message.Value);
                        subscription.Ack(deliveryArguments);
                    }
                }
            }
        }
    }
}
