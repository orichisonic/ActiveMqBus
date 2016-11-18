using System;
using System.Collections.Generic;
using System.Text;
using ActiveMq.Enum;
using Apache.NMS;
using Apache.NMS.ActiveMQ;

namespace ActiveMq
{
    public class ActiveMqProducter:IActiveMqProducter
    {
        //private string URI;
        //private string TOPIC;
        private string USERNAME;
        private string PASSWORD;
        private IConnectionFactory factory;
        private IConnection connection;
        private ISession session;
        private IMessageProducer producer;
        public string url
        {
            set; get; }

        public string topic
        {
            set; get; }

        public string username
        {
            set { USERNAME = value; }
        }

        public string password
        {
            set { PASSWORD = value; }

        }

        public ActiveMqProducter()
        {
            producer = null;
            factory = null;
            connection = null;
            session = null;
            //        try
            //        {

            //             factory = new ConnectionFactory("tcp://localhost:61616/");
            //        using (IConnection connection = factory.CreateConnection())
            //        {

            //            using (ISession session = connection.CreateSession())
            //            {
            //                IMessageProducer prod = session.CreateProducer(
            //                    new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic("testing"));
            //                int i = 0;
            //                while (!Console.KeyAvailable)
            //                {
            //                    ITextMessage msg = prod.CreateTextMessage();
            //                    msg.Text = i.ToString();
            //                    Console.WriteLine("Sending: " + i.ToString());
            //                    prod.Send(msg, Apache.NMS.MsgDeliveryMode.NonPersistent, Apache.NMS.MsgPriority.Normal,
            //                        TimeSpan.MinValue);
            //                    System.Threading.Thread.Sleep(5000);
            //                    i++;
            //                }
            //            }
            //        }
            //        Console.ReadLine();
            //    }

            //catch ( System.Exception e)
            //    {
            //        Console.WriteLine("{0}", e.Message);
            //        Console.ReadLine();
            //    }
        }

        ~ActiveMqProducter()
        {
            if (producer != null)
            {
                producer.Dispose();
            }

            Close();
        }

        public void Start()
        {
            factory = new ConnectionFactory(url);

            if (USERNAME != "")
            {
                connection = factory.CreateConnection(USERNAME, PASSWORD);
            }
            else
            {
                connection = factory.CreateConnection();
            }
            connection.Start();
            session = connection.CreateSession();
          
        }

        public void Close()
        {
            if (session != null)
            {
                session.Close();
            }
            if (connection != null)
            {
                connection.Stop();
                connection.Close();
            }
        }

        public void CreateProducer(TopicType activeMqType, string strTopicName)
        {
            switch (activeMqType)
            {
                case TopicType.Topic:
                    producer = session.CreateProducer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(strTopicName));
                    break;
                case TopicType.Queue:
                    producer = session.CreateProducer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(strTopicName));
                    break;
            }
  
    }

        public void SendMqMessage<TPropery>(MessageType messageType, string strText, List<TPropery> lstProperty) where TPropery:Property
        {

            try
            {
                IMessage msg;
                if (messageType.Equals(MessageType.Text))
                {
                    msg = producer.CreateTextMessage();
                    ((ITextMessage)msg).Text = strText;
                }
                else
                {
                    msg = producer.CreateBytesMessage();
                    ((IBytesMessage)msg).Content = Encoding.Default.GetBytes(strText);
                }

                foreach (Property prop in lstProperty)
                {
                    msg.Properties.SetString(prop.name, prop.value);
                }
                producer.Send(msg, Apache.NMS.MsgDeliveryMode.NonPersistent, Apache.NMS.MsgPriority.Normal, TimeSpan.MinValue);
            }
            catch (System.Exception ex)
            {
           
            }

        }

        public void SendMqMessage(string strText)
        {

            ITextMessage msg = producer.CreateTextMessage();
            msg.Text = strText;
            producer.Send(msg, Apache.NMS.MsgDeliveryMode.NonPersistent, Apache.NMS.MsgPriority.Normal, TimeSpan.MinValue);
        }
    }
}
