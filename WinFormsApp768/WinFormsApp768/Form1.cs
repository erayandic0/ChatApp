using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WinFormsApp768
{
    public partial class Form1 : Form
    {

        Socket sck;
        EndPoint epLocal, epRemote;
        byte[] buffer;
       
        

        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            sck = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            sck.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            textLocalIP.Text=GetLocalIP();
            textRemoteIP.Text=GetLocalIP();





        }

        private void MessageCallBack(IAsyncResult aResult)
        {   try
            {
                byte[] receivedData = new byte[1500];
                receivedData = (byte[])aResult.AsyncState;

                ASCIIEncoding aEncoding = new ASCIIEncoding();
                string receivedMessage = aEncoding.GetString(receivedData);

                listMassage.Items.Add("Friend : " + receivedMessage);

                buffer = new byte[1500];
                sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), buffer);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            ASCIIEncoding aEncoding = new ASCIIEncoding();
            byte[] sendingMessage = new byte[1500];
            sendingMessage = aEncoding.GetBytes(textMessage.Text);

            sck.Send(sendingMessage);

            listMassage.Items.Add("me: " + textMessage.Text);
            textMessage.Text="";

        }
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            epLocal= new IPEndPoint(IPAddress.Parse(textLocalIP.Text), Convert.ToInt32(textLocalPort.Text));
            sck.Bind(epLocal);

            epRemote = new IPEndPoint(IPAddress.Parse(textRemoteIP.Text), Convert.ToInt32(textRemotePort.Text));
            sck.Connect(epRemote);

            buffer= new byte[1500];
            sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), buffer);
        }

        private string GetLocalIP()
        {
            IPHostEntry host;
            host=Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily== AddressFamily.InterNetwork)
                    return ip.ToString();





            }

            return "127.0.0.1";


        }

            
        
          
        


    }
         
}









































