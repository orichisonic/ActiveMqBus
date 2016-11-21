using ActiveMq;
using Apache.NMS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ActiveMq.Enum;

namespace ActiveMqConsumerDemo
{
   
    public partial class Form1 : Form
    {
        public static ActiveMqConsumer activeMqConsumer;
        public static IMessageConsumer m_consumer;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {

            try
            {
                if (!GlobalFunction.CheckControlInput(txtURI, "URI", 0, false)) return;

                activeMqConsumer = new ActiveMqConsumer();

                activeMqConsumer.uri = txtURI.Text;
                activeMqConsumer.username = txtUserName.Text;
                activeMqConsumer.password = txtPassword.Text;

                activeMqConsumer.Start();
            }
            catch (System.Exception ex)
            {
                GlobalFunction.MsgBoxException(ex.Message, "btnConnect_Click");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
        }

        private void btnSubscribe_Click(object sender, EventArgs e)
        {
            try
            {
                if (!GlobalFunction.CheckControlInput(txtTopicName, "Topic Name", 0, false)) return;

                if (m_consumer != null)
                {
                    m_consumer.Close();
                }
                if (txtSelector.Text != "")
                {
                    m_consumer = activeMqConsumer.CreateConsumer(rdTopic.Checked ? TopicType.Topic : TopicType.Queue, txtTopicName.Text, txtSelector.Text);
                }
                else
                {
                    m_consumer = activeMqConsumer.CreateConsumer(rdTopic.Checked ? TopicType.Topic : TopicType.Queue, txtTopicName.Text);
                }
                activeMqConsumer.SetMessageType(rdMsgTypeText.Checked?MessageType.Text : MessageType.Byte);
                
                m_consumer.Listener += new MessageListener(activeMqConsumer.consumer_listener);
            }
            catch (System.Exception ex)
            {
                GlobalFunction.MsgBoxException(ex.Message, "btnSubscribe_Click");
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (m_consumer != null)
            {
                m_consumer.Close();
            }
        }
    }
}
