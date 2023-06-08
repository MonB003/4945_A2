using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using _4945_A2.Threads;
using GameThread = _4945_A2.Threads.GameThread;
using P = _4945_A2.Packet.Packet;

namespace _4945_A2.Network
{
    public class MulticastNetwork : Network
    {

        public Socket SendSockket;
        public Socket ListenSocket;

        public IPEndPoint sendEndPoint;
        public EndPoint listenEndPoint;

        public MulticastNetwork(GameThread gt) : base(gt)
        {
        }

        public MulticastNetwork(int port, string ipAddress, GameThread gt) : base(port, ipAddress, gt) { }


        public MulticastNetwork(int port, string ipAddress, GameThread gt, int bufferSize) : base(port, ipAddress, gt, bufferSize) { }


        public override void send(P packet)
        {
            Console.WriteLine("SENDING: " + packet.ToString());

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(this.GetIPAddress()), this.GetPort());

            try
            {
                //Send multicast packets to the listener.
                this.SendSockket.SendTo(packet.GetBuffer(), endPoint);
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.ToString());
            }
        }

        protected override void receive()
        {
            EndPoint remoteEP = (EndPoint)new IPEndPoint(IPAddress.Any, 0); // Recieve ENDPOINT

            while (true)
            {
                byte[] results = new byte[BUFFER_SIZE * sizeof(float)];
                this.ListenSocket.ReceiveFrom(results, ref remoteEP);
                Buffer.BlockCopy(results, 0, this.buffer, 0, BUFFER_SIZE * sizeof(float));
                P packet = new P(buffer[0], buffer[1], buffer[2], buffer[3], buffer[4]);
                Console.WriteLine("RECEIVED PACKET: " + packet.ToString());
                // gameEngine.notify()
            }

        }

        // Recieving
        private void ListenToMulticast()
        {
            this.ListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            // Create End Point
            IPAddress IPAny = IPAddress.Any;
            listenEndPoint = (EndPoint)new IPEndPoint(IPAny, this.GetPort());

            // Bind end point to socket
            this.ListenSocket.Bind(this.listenEndPoint);

            // Add Options
            MulticastOption mcastOption = new MulticastOption(IPAddress.Parse(this.GetIPAddress()),IPAny);
            this.ListenSocket.SetSocketOption(SocketOptionLevel.IP,SocketOptionName.AddMembership,mcastOption);
        }

        // Sending
        private void JoinMultiCast()
        {
            // Create new socket
            this.SendSockket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            // Create End Point
            IPAddress IPAny = IPAddress.Any;
            sendEndPoint = new IPEndPoint(IPAny, 0);

            // Bind end point to the socket
            this.SendSockket.Bind(sendEndPoint);

            // Add Options
            MulticastOption multicastOption = new MulticastOption(IPAddress.Parse(this.GetIPAddress()), IPAny);
            this.SendSockket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, multicastOption);
        }
        public override void setup()
        {
            ListenToMulticast();
            JoinMultiCast();
        }
    }

}
