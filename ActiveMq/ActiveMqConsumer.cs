using Apache.NMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActiveMq.Enum;
using Apache.NMS.ActiveMQ;

namespace ActiveMq
{
    public class ActiveMqConsumer:IActiveMqConsumer
    {
        //private string URI;
        //private string TOPIC;
        private string USERNAME;
        private string PASSWORD;
        private IConnectionFactory factory;
        private IConnection connection;
        private ISession session;
        private IMessageProducer producer;
        protected MessageType messageType;
        public string uri
        {
            set; get;
        }

        public string topic
        {
            set; get;
        }

        public string username
        {
            set { USERNAME = value; }
        }

        public string password
        {
            set { PASSWORD = value; }
        }



        public void SetMessageType(MessageType messageType)
        {
            messageType = messageType;
        }

        //{
        //    get;set;


        //}

        public void consumer_listener(IMessage message)
        {
            

       
            string strMsg;

           
                if (messageType.Equals(MessageType.Text))
                {
                    ITextMessage msg = (ITextMessage) message;
                    strMsg = msg.Text;
                }
                else
                {
                    IBytesMessage msg = (IBytesMessage) message;
                    strMsg = Encoding.Default.GetString(msg.Content);
                }

             
                Console.Write(strMsg);

           
          
        }

        public ActiveMqConsumer()
        {
            producer = null;
            factory = null;
            connection = null;
            session = null;
            //try
            //{
            //    //Create the Connection factory   
            //    IConnectionFactory factory = new ConnectionFactory("tcp://localhost:61616/");
            //    //Create the connection   
            //    using (IConnection connection = factory.CreateConnection())
            //    {
            //        connection.ClientId = "testing listener";
            //        connection.Start();
            //        //Create the Session   
            //        using (ISession session = connection.CreateSession())
            //        {
            //            //Create the Consumer   
            //            IMessageConsumer consumer = session.CreateDurableConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic("testing"), "testing listener", null, false);
            //            consumer.Listener += new MessageListener(consumer_Listener);
            //            Console.ReadLine();
            //        }
            //        connection.Stop();
            //        connection.Close();
            //    }
            //}
            //catch (System.Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}
        }


        ~ActiveMqConsumer()
        {
              producer = null;
            factory = null;
            connection = null;
            session = null;
        }

     

        public IMessageConsumer CreateConsumer(TopicType topicType, string strTopicName)
        {
            if (topicType.Equals(TopicType.Topic))
            {
                return session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(strTopicName));
            }
            else
            {
                return session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(strTopicName));
            }
        }

        public IMessageConsumer CreateConsumer(TopicType topicType, string strTopicName, string strSelector)
        {
            if (strSelector == "")
            {
                Console.WriteLine("MQ selector不能为空");
                return null;
            }

            if (topicType.Equals(TopicType.Topic))
            {
                return session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(strTopicName), strSelector, false);
            }
            else
            {
                return session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(strTopicName), strSelector, false);
            }
          
        }

        public void Start()
        {
            factory = new ConnectionFactory(uri);

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
    }
}
