using RabbitMQ.Client;
using System;
using System.Text;

namespace RebbitMQDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to RabbitMQ Product!");
            //DirectExchangeSendMsg();
            TopicExchangeSendMsg();
            Console.WriteLine("按任意值，退出程序");
            Console.ReadKey();
        }

        /// <summary>
        /// 连接配置
        /// </summary>
        private static readonly ConnectionFactory rabbitMqFactory = new ConnectionFactory()
        {
            HostName = "192.168.1.111",//HostName = "127.0.0.1",
            UserName = "anyue",
            Password = "123",
            Port = 5672,
            VirtualHost = "/"
        };
        /// <summary>
        /// 路由名称
        /// </summary>
        const string ExchangeName = "howdy.exchange";

        //队列名称
        const string QueueName = "howdy.queue";

        /// <summary>
        /// 路由名称
        /// </summary>
        const string TopExchangeName = "topic.howdy.exchange";

        //队列名称
        const string TopQueueName = "topic.howdy.queue";


        /// <summary>
        ///  单点精确路由模式
        /// </summary>
        public static void DirectExchangeSendMsg()
        {
            using (IConnection conn = rabbitMqFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
                    channel.QueueDeclare(QueueName, durable: true, autoDelete: false, exclusive: false, arguments: null);
                    channel.QueueBind(QueueName, ExchangeName, routingKey: QueueName);

                    var props = channel.CreateBasicProperties();
                    props.Persistent = true;
                    string vadata = Console.ReadLine();
                    while (vadata != "exit")
                    {
                        var msgBody = Encoding.UTF8.GetBytes(vadata);
                        channel.BasicPublish(exchange: ExchangeName, routingKey: QueueName, basicProperties: props, body: msgBody);
                        Console.WriteLine(string.Format("***发送时间:{0}，发送完成，输入exit退出消息发送",
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                        vadata = Console.ReadLine();
                    }
                }
            }
        }
        /// <summary>
        /// topic 模糊匹配模式，符号“#”匹配一个或多个词，符号“*”匹配不多不少一个词。因此“log.#”能够匹配到“log.info.oa”，但是“log.*” 只会匹配到“log.error”
        /// </summary>
        public static void TopicExchangeSendMsg()
        {
            string routingKey = "topic.howdy.#";
            using (IConnection conn = rabbitMqFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    channel.ExchangeDeclare(TopExchangeName, ExchangeType.Topic, durable: false, autoDelete: false, arguments: null);
                    channel.QueueDeclare(TopQueueName, durable: false, autoDelete: false, exclusive: false, arguments: null);
                    channel.QueueBind(TopQueueName, TopExchangeName, routingKey: routingKey);
                    //var props = channel.CreateBasicProperties();
                    //props.Persistent = true;
                    string vadata = Console.ReadLine();
                    while (vadata != "exit")
                    {
                        var msgBody = Encoding.UTF8.GetBytes(vadata);
                        channel.BasicPublish(exchange: TopExchangeName, routingKey: routingKey, basicProperties: null, body: msgBody);
                        Console.WriteLine(string.Format("***发送时间:{0}，发送完成，输入exit退出消息发送", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                        vadata = Console.ReadLine();
                    }
                }
            }
        }
    }
}
