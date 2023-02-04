using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Chat
{
    public partial class frmClient : Form
    {
        public frmClient()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void myTCPClient1_OnClose()
        {
            toolStripStatusLabel2.Text = "Disconnected ...";
            button1.Enabled = true;
        }

        private void myTCPClient1_OnRecive(byte[] data)
        {
            string msg = Encoding.UTF8.GetString(data);
            Display("Server : ", msg);
        }

        private void myTCPServer1_OnAccept()
        {
            toolStripStatusLabel2.Text = "Connected";
            button1.Enabled = true;
        }


        private void Display(string name,string msg)
        {
            textBox1.AppendText("[" + DateTime.Now.ToString() + "]");
            textBox1.AppendText("\r\n");
            textBox1.AppendText(name);
            textBox1.AppendText(msg);
            textBox1.AppendText("\r\n");
            textBox1.AppendText("\r\n");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                textBox2.Focus();
                return;
            }

            if (myTCPClient1.Send(Encoding.UTF8.GetBytes(textBox2.Text)))
            {
                Display("Client",textBox2.Text);
                textBox2.Text = "";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
            myTCPClient1.Connect(textBox3.Text, (int)numericUpDown1.Value);

            toolStripStatusLabel2.Text = "Connecting ...";
        }

        private void myTCPClient1_ConnectedE()
        {
            toolStripStatusLabel2.Text = "Connected.";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            myTCPClient1.Close();
        }
    }
}
