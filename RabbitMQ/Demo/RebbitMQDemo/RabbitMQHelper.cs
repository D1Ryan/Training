using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;


class RabbitMQHelper
{
    /// <summary>
    /// 交换机名称
    /// </summary>
    const string ExchangeName = "howdy.exchange";
    /// <summary>
    /// 队列名称
    /// </summary>
    const string QueueName = "howdy.queue";
    /// <summary>
    /// 连接配置
    /// </summary>
    private static readonly ConnectionFactory rabbitMqFactory = new ConnectionFactory()
    {
        HostName = "127.0.0.1",
        UserName = "guest",
        Password = "123",
        Port = 5672,
        VirtualHost = "/"
    };
    public static IConnection GetConnection()
    {
        //定义一个连接工厂
        ConnectionFactory factory = rabbitMqFactory;
        return factory.CreateConnection();
    }
    public static IConnection GetConnection(ConnectionFactory connFactory)
    {
        ConnectionFactory factory = connFactory;
        return factory.CreateConnection();
    }
    #region 直接模式 ExchangeType.Direct 
    /// <summary>
    ///  单点精确路由模式
    /// </summary>
    public static void DirectExchangeSendMsg(List<string> msgList)
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
                foreach (string msg in msgList)
                {
                    var msgBody = Encoding.UTF8.GetBytes(msg);
                    channel.BasicPublish(exchange: ExchangeName, routingKey: QueueName, basicProperties: props, body: msgBody);
                    Console.WriteLine(string.Format("***发送时间:{0}，发送完成，输入exit退出消息发送",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                }
            }
        }
    }
    /// <summary>
    /// 消息接收，获取一次
    /// </summary>
    public static string DirectAcceptExchange()
    {
        using (IConnection conn = rabbitMqFactory.CreateConnection())
        {
            using (IModel channel = conn.CreateModel())
            {
                channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
                channel.QueueDeclare(QueueName, durable: true, autoDelete: false, exclusive: false, arguments: null);
                channel.QueueBind(QueueName, ExchangeName, routingKey: QueueName);

                BasicGetResult msgResponse = channel.BasicGet(QueueName, true);
                if (msgResponse != null)
                {
                    var msgBody = Encoding.UTF8.GetString(msgResponse.Body.ToArray());
                    Console.WriteLine(string.Format("***接收时间:{0}，消息内容：{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msgBody));
                    return msgBody;
                }
                else {
                    return "";
                }
            }
        }
    }
    /// <summary>
    /// 基于事件的，当消息到达时触发事件，获取数据
    /// </summary>
    public static void DirectAcceptExchangeEvent()
    {
        using (IConnection conn = rabbitMqFactory.CreateConnection())
        {
            using (IModel channel = conn.CreateModel())
            {
                //channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
                channel.QueueDeclare(QueueName, durable: true, autoDelete: false, exclusive: false, arguments: null);
                //channel.QueueBind(QueueName, ExchangeName, routingKey: QueueName);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var msgBody = Encoding.UTF8.GetString(ea.Body.ToArray());
                    Console.WriteLine(string.Format("***接收时间:{0}，消息内容：{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msgBody));
                };
                channel.BasicConsume(QueueName, true, consumer: consumer);
                Console.WriteLine("按任意值，退出程序");
                Console.ReadKey();
            }
        }
    }
    /// <summary>
    /// 基于事件的，当消息到达时触发事件，获取数据
    /// </summary>
    public static void DirectAcceptExchangeTask()
    {
        using (IConnection conn = rabbitMqFactory.CreateConnection())
        {
            using (IModel channel = conn.CreateModel())
            {
                //channel.ExchangeDeclare(ExchangeName, "direct", durable: true, autoDelete: false, arguments: null);
                channel.QueueDeclare(QueueName, durable: true, autoDelete: false, exclusive: false, arguments: null);
                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);//告诉broker同一时间只处理一个消息
               //channel.QueueBind(QueueName, ExchangeName, routingKey: QueueName);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var msgBody = Encoding.UTF8.GetString(ea.Body.ToArray());
                    Console.WriteLine(string.Format("***接收时间:{0}，消息内容：{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msgBody));
                    int dots = msgBody.Split('.').Length - 1;
                    System.Threading.Thread.Sleep(dots * 1000);
                    //处理完成，告诉Broker可以服务端可以删除消息，分配新的消息过来
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };
                //noAck设置false,告诉broker，发送消息之后，消息暂时不要删除，等消费者处理完成再说
                channel.BasicConsume(QueueName, false, consumer: consumer);

                Console.WriteLine("按任意值，退出程序");
                Console.ReadKey();
            }
        }
    } 
    #endregion
}

