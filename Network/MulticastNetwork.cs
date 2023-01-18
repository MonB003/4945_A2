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

        public IPEndPoint endPoint;
        public IPAddress mcastAddress;
        public Socket mcastSocket;

        public MulticastNetwork(GameThread gt) : base(gt)
        {
        }

        public MulticastNetwork(int port, string ipAddress, GameThread gt) : base(port, ipAddress, gt) { }


        public MulticastNetwork(int port, string ipAddress, GameThread gt, int bufferSize) : base(port, ipAddress, gt, bufferSize) { }


        public override void send(P packet)
        {
            Console.WriteLine("SENDING: " + packet.ToString());

            try
            {
                //Send multicast packets to the listener.
                this.mcastSocket.SendTo(packet.GetBuffer(), endPoint);
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
                mcastSocket.ReceiveFrom(buffer, ref remoteEP);

                P packet = new P(buffer[0], buffer[1], buffer[2], buffer[3], buffer[4], buffer[5]);

                Console.WriteLine("RECEIVED PACKET: " + packet.ToString());

                // gameEngine.notify()
            }

        }

        public override void setup()
        {
          
            this.mcastAddress = IPAddress.Parse(this.GetIPAddress());

            this.mcastSocket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Dgram,
                ProtocolType.Udp
                );

            IPAddress localIP = IPAddress.Any;
            EndPoint localEP = (EndPoint)new IPEndPoint(localIP, this.GetPort()); // RECIEVE ENDPOINT
            // RECIEVE STUFF
            MulticastOption mcastOption = new MulticastOption(mcastAddress, localIP);
            mcastSocket.SetSocketOption(SocketOptionLevel.IP,
                                        SocketOptionName.AddMembership,
                                        mcastOption);
            mcastSocket.Bind(localEP); 


            mcastSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
            this.endPoint = new IPEndPoint(this.mcastAddress, this.GetPort()); // SEND ENDPOINT
        }
    }

}
