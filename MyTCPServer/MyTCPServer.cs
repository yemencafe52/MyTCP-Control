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

namespace MyTCPServer
{
    public partial class MyTCPServer: UserControl
    {
        private Socket s;
        private int port;

        public delegate void ReciveD(byte[] data);
        public event ReciveD OnRecive;

        public delegate void AcceptD();
        public event AcceptD OnAccept;

        public delegate void CloseD();
        public event CloseD OnClose;

        private byte[] buffer = new byte[1024 * 8];
        public MyTCPServer()
        {
            InitializeComponent();
        }

        public MyTCPServer(int port)
        {
            InitializeComponent();
            this.port = port;
        }

        public bool Listen(int port)
        {
            bool res = false;

            try
            {
                if (s != null)
                {
                    Close();
                }

                this.port = port;
                s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                s.Bind(new IPEndPoint(IPAddress.Parse("0.0.0.0"), this.port));
                s.Listen(1);
                s.BeginAccept(new AsyncCallback(Accept), null);
                res = true;

            }
            catch
            {
                Close();
            }

            return res;
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

        private void Accept(IAsyncResult ar)
        {
            try
            {
                Socket c = this.s.EndAccept(ar);
                this.s.Close();
                this.s.Dispose();

                this.s = c;
                this.s.BeginReceive(this.buffer, 0, this.buffer.Length, SocketFlags.None, new AsyncCallback(Recive), null);

                if (OnAccept != null)
                {
                    OnAccept();
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

                if(data.Length == snt)
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
                    if(this.IsConnected)
                    {
                        res = true;
                    }
                }

                return res;
            }
        }

        private void MyTCPServer_Resize(object sender, EventArgs e)
        {
            this.Width = 75;
            this.Height = 22;
        }
    }
}
