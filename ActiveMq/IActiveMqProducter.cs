using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActiveMq.Enum;

namespace ActiveMq
{
    public interface IActiveMqProducter
    {
        void Start();

        void Close();

        void CreateProducer(TopicType activeMqType, string strTopicName);

        void SendMqMessage<TPropery>(MessageType messageType, string strText, List<TPropery> lstProperty) where TPropery:Property;

        void SendMqMessage(string strText);
    }
}
