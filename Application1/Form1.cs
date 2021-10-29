using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Application1
{
    public partial class Form1 : Form
    {
        MqttClient mqttClient;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                mqttClient = new MqttClient("127.0.0.1");
                mqttClient.MqttMsgPublishReceived += MqttClient_MqttMsgPublishReceived;
                mqttClient.Subscribe(new string[] { "Application1/Message" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
                mqttClient.Connect("Application2");
            });
        }

        private void MqttClient_MqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Message);
            listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add(message)));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                if (mqttClient != null && mqttClient.IsConnected)
                {
                    mqttClient.Publish("Application2/Message", Encoding.UTF8.GetBytes(textBox1.Text));
                }
            });
        }
    }
}
