using System;
using Apache.NMS;
using Apache.NMS.ActiveMQ;

namespace ActiveMq
{
    public class ActiveMqProducter
    {

        public ActiveMqProducter()
        {
            try
            {
                //Create the Connection Factory   
                IConnectionFactory factory = new ConnectionFactory("tcp://localhost:61616/");
            using (IConnection connection = factory.CreateConnection())
            {
                //Create the Session   
                using (ISession session = connection.CreateSession())
                {
                    //Create the Producer for the topic/queue   
                    IMessageProducer prod = session.CreateProducer(
                        new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic("testing"));
                    //Send Messages   
                    int i = 0;
                    while (!Console.KeyAvailable)
                    {
                        ITextMessage msg = prod.CreateTextMessage();
                        msg.Text = i.ToString();
                        Console.WriteLine("Sending: " + i.ToString());
                        prod.Send(msg, Apache.NMS.MsgDeliveryMode.NonPersistent, Apache.NMS.MsgPriority.Normal,
                            TimeSpan.MinValue);
                        System.Threading.Thread.Sleep(5000);
                        i++;
                    }
                }
            }
            Console.ReadLine();
        }

    catch ( System.Exception e)
        {
            Console.WriteLine("{0}", e.Message);
            Console.ReadLine();
        }
    }
}
}
