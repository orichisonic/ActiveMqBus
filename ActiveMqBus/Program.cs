using ActiveMq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ActiveMq.Enum;
using Apache.NMS;
namespace ActiveMqBus
{
    class Program
    {
        public  static ActiveMqProducter  activeMqProducter;
        public static ActiveMqConsumer activeMqConsumer;
        public static IMessageConsumer m_consumer;

        private static void Main(string[] args)
        {
            ProducerRun();
            ProducerSendMessage();
            ConsumerRun();
            ConsumerReceiveMessage();
        }

        private static void ProducerRun()
        {
            activeMqProducter=new ActiveMqProducter();
            activeMqProducter.url= "tcp://localhost:61616/";
            activeMqProducter.username = "admin";
            activeMqProducter.password = "admin";
            activeMqProducter.Start();

        }

        private static void ProducerSendMessage()
        {
            activeMqProducter.CreateProducer(TopicType.Topic, "ActiveMq test");
         
            List<Property> lstProperty=new List<Property>();
            Property property=new Property();
            property.name = "Filter";
            property.value = "test";
            lstProperty.Add(property);
            activeMqProducter.SendMqMessage<Property>(MessageType.Text,"Message content", lstProperty);
        }

        private static void ConsumerRun()
        {
            activeMqConsumer = new ActiveMqConsumer();
            activeMqConsumer.uri = "tcp://localhost:61616/";
            activeMqConsumer.username = "admin";
            activeMqConsumer.password = "admin";
            activeMqConsumer.Start();

        }

        private static void ConsumerReceiveMessage()
        {
            activeMqConsumer.CreateConsumer(TopicType.Topic, "ActiveMq test", "Filter=test");
            //m_consumer.Listener += new MessageListener(new activeMqConsumer.consumer_listener());
        }
    }
}
