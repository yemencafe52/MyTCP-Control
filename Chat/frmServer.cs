using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chat
{
    public partial class frmServer : Form
    {
        private frmClient client = new frmClient();
        public frmServer()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            client.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            myTCPServer1.Listen((int)numericUpDown1.Value);
            toolStripStatusLabel2.Text = "Listening ...";
        }
        private void myTCPServer1_OnClose()
        {
            toolStripStatusLabel2.Text = "Listening ...";
            button1.Enabled = true;
        }

        private void myTCPServer1_OnAccept()
        {
            toolStripStatusLabel2.Text = "Connected";
            button1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(textBox2.Text))
            {
                textBox2.Focus();
                return;
            }

            if(myTCPServer1.Send(Encoding.UTF8.GetBytes(textBox2.Text)))
            {
                Display("Server :",textBox2.Text);
                textBox2.Text = "";
            }
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

        private void myTCPServer1_OnRecive(byte[] data)
        {
            string msg = Encoding.UTF8.GetString(data);
            Display("Client : ", msg);
        }
    }
}
