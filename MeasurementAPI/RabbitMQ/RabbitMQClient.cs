using System;
using System.Collections.Generic;
using Measurements.Models;
using RabbitMQ.Client;

namespace Measurements.RabbitMQ
{
    public class RabbitMQClient
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _model;

        private const string ExchangeName = "Topic_Exchange";
        private const string DeviceMeasurementQueueName = "DeviceMeasurementTopic_Queue";
        private const string AllQueueName = "AllTopic_Queue";

        public RabbitMQClient()
        {
            CreateConnection();
        }

        private static void CreateConnection()
        {
            _factory = new ConnectionFactory
            {
                HostName = "localhost", UserName = "guest", Password = "guest"
            };

            _connection = _factory.CreateConnection();
            _model = _connection.CreateModel();
            _model.ExchangeDeclare(ExchangeName, "topic");

            _model.QueueDeclare(DeviceMeasurementQueueName, true, false, false, null);
            _model.QueueDeclare(AllQueueName, true, false, false, null);

            _model.QueueBind(DeviceMeasurementQueueName, ExchangeName, "measurement.device");

            _model.QueueBind(AllQueueName, ExchangeName, "measurement.*");
        }

        public void Close()
        {
        }

        public void SendMeasurement(DeviceMeasurement measurement)
        {
            SendMessage(measurement.Serialize(), "measurement.device");
            Console.WriteLine(" Measurement Sent {0}, {1}", measurement.DeviceNumber, 
                measurement.Value);
        }


        public void SendMessage(byte[] message, string routingKey)
        {
        }
    }  
}
