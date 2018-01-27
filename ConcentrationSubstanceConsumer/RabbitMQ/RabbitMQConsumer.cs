using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;

namespace ConcentrationSubstanceConsumer.RabbitMQ
{
    public class RabbitMQConsumer
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;        

        private const string ExchangeName = "Topic_Exchange";
        private const string ConcentrationSubstanceQueueName = "ConcentrationSubstanceTopic_Queue";

        public void CreateConnection()
        {
            _factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };            
        }

        public void Close()
        {
            _connection.Close();
        }

        public void ProcessMessages()
        {
            using (_connection = _factory.CreateConnection())
            {
                using (var channel = _connection.CreateModel())
                {
                    Console.WriteLine("Listening for Topic <measurement.concentrationsubstance>");
                    Console.WriteLine("------------------------------------------");
                    Console.WriteLine();
                    
                    channel.ExchangeDeclare(ExchangeName, "topic");
                    channel.QueueDeclare(ConcentrationSubstanceQueueName, true, false, false, null);
                    channel.QueueBind(ConcentrationSubstanceQueueName, ExchangeName, "measurement.concentrationsubstance");

                    channel.BasicQos(0, 10, false);
                    Subscription subscription = new Subscription(channel, ConcentrationSubstanceQueueName, false);
                    
                    while (true)
                    {
                        BasicDeliverEventArgs deliveryArguments = subscription.Next();

                        var message = (ConcentrationSubstance)deliveryArguments.Body.DeSerialize(typeof(ConcentrationSubstance));
                        var routingKey = deliveryArguments.RoutingKey;

                        Console.WriteLine("-- Concentration Substance - Routing Key <{0}> : {1}, £{2}, {3}, {4}", routingKey, message.TankName, message.ValueToMeasure, message.MeasurementDelay, message.SubstanceNumber);
                        subscription.Ack(deliveryArguments);
                    }
                }
            }
        }
    }
}
