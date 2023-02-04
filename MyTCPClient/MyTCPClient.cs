using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace MyTCPClient
{
    public partial class MyTCPClient: UserControl
    {
        private Socket s;

        private int port;
        private string ip;

        public delegate void ReciveD(byte[] data);
        public event ReciveD OnRecive;

        public delegate void ConnectedD();
        public event ConnectedD ConnectedE;

        public delegate void CloseD();
        public event CloseD OnClose;

        private byte[] buffer = new byte[1024 * 8];

        public MyTCPClient()
        {
            InitializeComponent();
        }

        public void Connect(string ip,int port)
        {
            try
            {
                if (s != null)
                {
                    Close();
                }

                this.ip = ip;
                this.port = port;

                s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                s.BeginConnect(ip, port,new AsyncCallback(Connected),null);
            }
            catch
            {
            }

        }

        private void label1_Resize(object sender, EventArgs e)
        {
            this.Width = 75;
            this.Height = 22;
        }
        public void Close()
        {
            try
            {
                if (s != null)
                {
                    this.s.Close();
                    this.s.Dispose();
                    this.s = null;
                }

                if (OnClose != null)
                {
                    OnClose();
                }
            }
            catch
            {

            }
        }

        private void Recive(IAsyncResult ar)
        {
            try
            {
                int len = this.s.EndReceive(ar);

                if (len > 0)
                {
                    byte[] temp = new byte[len];
                    Array.Copy(this.buffer, 0, temp, 0, len);
                    this.s.BeginReceive(this.buffer, 0, this.buffer.Length, SocketFlags.None, new AsyncCallback(Recive), null);

                    if (OnRecive != null)
                    {
                        OnRecive(temp);
                    }
                }
            }
            catch { }

        }

        private void Connected(IAsyncResult ar)
        {
            try
            {
                this.s.EndConnect(ar);
                this.s.BeginReceive(this.buffer, 0, this.buffer.Length, SocketFlags.None, new AsyncCallback(Recive), null);

                if (ConnectedE != null)
                {
                    ConnectedE();
                }
            }
            catch
            {

            }
        }

        public bool Send(byte[] data)
        {
            bool res = false;

            try
            {
                int snt = this.s.Send(data);

                if (data.Length == snt)
                {
                    res = true;
                }

            }
            catch
            {

            }

            return res;
        }

        public bool IsConnected
        {
            get
            {
                bool res = false;

                if (this.s != null)
                {
                    if (this.IsConnected)
                    {
                        res = true;
                    }
                }

                return res;
            }
        }
    }
}
