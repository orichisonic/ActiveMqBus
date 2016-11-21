using ActiveMq;
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

namespace ActiveMqProducerDemo
{
    public partial class Form1 : Form
    {
        public List<Property> m_lstProperty = new List<Property>();
        public static ActiveMqProducter activeMqProducter;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lv.Columns.Clear();
            ListViewCtrl.SetListViewStyle(ref lv);

            lv.Columns.Add("属性名称", "Property Name", 150, HorizontalAlignment.Left, 0);
            lv.Columns.Add("属性值", "Property Value", 150, HorizontalAlignment.Left, 0);

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (!GlobalFunction.CheckControlInput(txtURI, "URI", 0, false)) return;
                activeMqProducter = new ActiveMqProducter();
                activeMqProducter.url = txtURI.Text;
                activeMqProducter.username = txtUserName.Text;
                activeMqProducter.password = txtPassword.Text;
                activeMqProducter.Start();

                //m_mq = new MQ();
                //m_mq.uri = txtURI.Text;
                //m_mq.username = txtUserName.Text;
                //m_mq.password = txtPassword.Text;
                //m_mq.Start();
            }
            catch (System.Exception ex)
            {
                GlobalFunction.MsgBoxException(ex.Message, "btnConnect_Click");
            }
        }

        private void btnCreateProd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!GlobalFunction.CheckControlInput(txtTopicName, "Topic Name", 0, false)) return;

                activeMqProducter.CreateProducer(rdTopic.Checked?TopicType.Topic : TopicType.Queue, txtTopicName.Text);
            }
            catch (System.Exception ex)
            {
                GlobalFunction.MsgBoxException(ex.Message, "btnCreateProd_Click");
            }
        }

        private void btnAddProperty_Click(object sender, EventArgs e)
        {
            if (!GlobalFunction.CheckControlInput(txtPropertyName, "Property name", 0, false)) return;
            if (!GlobalFunction.CheckControlInput(txtPropertyValue, "Property value", 0, false)) return;

            ListViewItem item = null;
            ListViewCtrl.AddNewListViewItem(ref item, ref lv);
            ListViewCtrl.SetListViewValue(txtPropertyName.Text, "属性名称", lv, ref item);
            ListViewCtrl.SetListViewValue(txtPropertyValue.Text, "属性值", lv, ref item);

            CreatePropertiesArray();
        }

        private void CreatePropertiesArray()
        {
            m_lstProperty.Clear();
            foreach (ListViewItem item in lv.Items)
            {
                Property property = new Property();
                property.name = ListViewCtrl.GetListViewValue("属性名称", lv, item.Index);
                property.value = ListViewCtrl.GetListViewValue("属性值", lv, item.Index);

                m_lstProperty.Add(property);
            }
        }

        private void btnRemoveProperty_Click(object sender, EventArgs e)
        {
            if (lv.SelectedItems.Count <= 0)
            {
                return;
            }

            lv.Items.Remove(lv.SelectedItems[0]);

            CreatePropertiesArray();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!GlobalFunction.CheckControlInput(rtxSendData, "Send Data", 0, false)) return;

            SendMessage();
        }

        private void SendMessage()
        {
            activeMqProducter.SendMqMessage(rdMsgTypeText.Checked?MessageType.Text : MessageType.Byte, rtxSendData.Text, m_lstProperty);
        }

        private void btnTimerSend_Click(object sender, EventArgs e)
        {
            if (!GlobalFunction.CheckControlInput(txtInterval, "Timer Interval", 0, true)) return;

            int interval = Convert.ToInt32(txtInterval.Text);
            if (interval <= 0)
            {
                GlobalFunction.MsgBoxExclamation("请输入大于0的整数");
                txtInterval.Focus();
                return;
            }

            btnSend.Enabled = false;
            btnTimerSend.Enabled = false;

            timer1.Interval = interval;
            timer1.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            btnTimerSend.Enabled = true;
            btnSend.Enabled = true;
        }
    }
}
