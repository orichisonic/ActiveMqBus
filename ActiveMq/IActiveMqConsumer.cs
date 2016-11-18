using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActiveMq.Enum;
using Apache.NMS;

namespace ActiveMq
{
    public interface IActiveMqConsumer
    {
        IMessageConsumer CreateConsumer(TopicType topicType, string strTopicName);

        IMessageConsumer CreateConsumer(TopicType topicType, string strTopicName, string strSelector);

        void Start();

        void Close();

        void consumer_listener(IMessage message, MessageType messageType);
    }
}
