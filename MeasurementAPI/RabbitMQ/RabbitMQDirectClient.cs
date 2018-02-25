﻿using System;
using System.Text;
using Measurements.Models;
using RabbitMQ.Client;

namespace Measurements.RabbitMQ
{
    public class RabbitMQDirectClient
    {
        private IConnection _connection;
        private IModel _channel;
        private string _replyQueueName;
        private QueueingBasicConsumer _consumer;

        public void CreateConnection()
        {
            var factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _replyQueueName = _channel.QueueDeclare("rpc_reply", true, false, false, null);           

            _consumer = new QueueingBasicConsumer(_channel);
            _channel.BasicConsume(_replyQueueName, true, _consumer);
        }

        public void Close()
        {
            _connection.Close();
        }

        public string MakeMeasurement(DeviceMeasurement measurement)
        {
            var corrId = Guid.NewGuid().ToString();
            var props = _channel.CreateBasicProperties();
            props.ReplyTo = _replyQueueName;
            props.CorrelationId = corrId;

            _channel.BasicPublish("", "rpc_queue", props, measurement.Serialize());

            while (true)
            {
                var ea = _consumer.Queue.Dequeue();

                if (ea.BasicProperties.CorrelationId != corrId) continue;

                var authCode = Encoding.UTF8.GetString(ea.Body);                    
                return authCode;
            }
        }
    }
}
