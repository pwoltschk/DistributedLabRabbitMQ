using System;
using System.Globalization;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;

namespace DirectMeasurementDeviceConsumer.RabbitMQ
{
    public class RabbitMQConsumer
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _channel;
        private static QueueingBasicConsumer _consumer;
        private static Random _rnd;

        public void ProcessMessages()
        {
            while (true)
            {
                GetMessageFromQueue();
            }
        }

        private void GetMessageFromQueue()
        {
            string response = null;
            var ea = _consumer.Queue.Dequeue();
            var props = ea.BasicProperties;
            var replyProps = _channel.CreateBasicProperties();

            try
            {
                response = MakeMeasurement(ea);
            }
            catch (Exception ex)
            {
                Console.WriteLine(" ERROR : " + ex.Message);            }
            finally
            {
                if (response != null)
                {
                    var responseBytes = Encoding.UTF8.GetBytes(response);
                    _channel.BasicPublish("", props.ReplyTo, replyProps, responseBytes);
                }
                _channel.BasicAck(ea.DeliveryTag, false);
            }

        }

        private string MakeMeasurement(BasicDeliverEventArgs ea)
        {
            var measurement = (DeviceMeasurement)ea.Body.DeSerialize(typeof(DeviceMeasurement));
            var response = _rnd.Next(1000, 100000000).ToString(CultureInfo.InvariantCulture);
            Console.WriteLine("Measurement -  {0} : £{1} : Auth Code <{2}> ", measurement.DeviceNumber, measurement.Value, response);

            return response;
        }

        public void CreateConnection()
        {
            _factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.BasicConsume("rpc_queue", false, _consumer);
            _rnd = new Random();
        }

        public void Close()
        {
            _connection.Close();
        }
    }
}
