using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Runtime.CompilerServices;

namespace CDNHw
{
    public class TCPHelper
    {
        //config for ip and port
        public byte[] HostIP { get; set; } = new byte[] { 127, 0, 0, 1 };
        public int HostPort { get; set; } = 8091;

        private Socket socket = null;

        public TCPHelper(byte[] hostIP, int hostPort)
        {
            HostIP = hostIP;
            HostPort = hostPort;
        }
        public TCPHelper() { }

        //when the componet wants to be a server， it should run the InitListner method 
        public void InitListener()
        {
            //regular process for TCP port listening
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(new IPAddress(HostIP), HostPort));
            socket.Listen(1);
        }

        //when a port is listening， it should keep a loop for accept requests
        public void AcceptLoop(Func<string, string> processRequest)
        {


            var client = socket.Accept();

            byte[] buff = new byte[21474836];

            int v = client.Receive(buff);

            byte[] res = new byte[v];
            Array.Copy(buff, res, v);

            string rcvStr = UTF8Encoding.UTF8.GetString(res);



            client.Send(UTF8Encoding.UTF8.GetBytes(processRequest(rcvStr)));

            client.Close();

        }

        //regular process for send request to a TCP server
        public string SendRequest(string request, string ip, int port)
        {
            var s2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            s2.Connect(ip, port);

            s2.Send(UTF8Encoding.UTF8.GetBytes(request));

            var buff = new byte[21474836];

            var length = s2.Receive(buff);

            var ms = UTF8Encoding.UTF8.GetString(buff, 0, length);

            return ms;
        }


    }
}
