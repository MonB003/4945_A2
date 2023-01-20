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

        public Socket mcastSocket;
        public IPEndPoint endPoint;
        public IPEndPoint groupEndPoint;

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
                this.mcastSocket.SendTo(packet.GetBuffer(), this.groupEndPoint);
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

                Console.WriteLine("BUFFER SIZE " + results.Length);
                mcastSocket.ReceiveFrom(results, ref remoteEP);
                Buffer.BlockCopy(results, 0, this.buffer, 0, BUFFER_SIZE * sizeof(float));
                P packet = new P(buffer[0], buffer[1], buffer[2], buffer[3], buffer[4]);
                Console.WriteLine("RECEIVED PACKET: " + packet.ToString());
                // gameEngine.notify()
            }

        }

        private void setUpReciever()
        {
            this.endPoint = new IPEndPoint(IPAddress.Any, this.GetPort());
            this.mcastSocket.Bind(this.endPoint);
        }

        private void setUpSender()
        {
            MulticastOption multicastOption = new MulticastOption(IPAddress.Parse(this.GetIPAddress()));
            this.mcastSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, multicastOption);
            this.groupEndPoint = new IPEndPoint(IPAddress.Parse(this.GetIPAddress()), this.GetPort());

        }
        public override void setup()
        {
            this.mcastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            setUpReciever();
            setUpSender();
        }
    }

}
