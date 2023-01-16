using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
//using System.Threading.Tasks;
using System.Threading;
using GameThread = _4945_A2.Threads.GameThread;
using P = _4945_A2.Packet.Packet;


namespace _4945_A2.Network
{
    public abstract class Network
    {
        private const int PORT = 8000;
        private const string IP_ADDRESS = "230.0.0.1";
        private const int BUFFER_SIZE = 100;

        private int port;
        private string ipAddress;
        private GameThread gt;
        protected byte[] buffer;

        private Thread t;

        public Network(GameThread gt)
        {
            this.port = PORT;
            this.ipAddress = IP_ADDRESS;
            this.buffer = new byte[BUFFER_SIZE];
            this.gt = gt;
            setup();
        }

        public Network(int port, string ipAddress, GameThread gt)
        {
            this.port = port;
            this.ipAddress = ipAddress;
            this.gt = gt;
            this.buffer = new byte[BUFFER_SIZE];
            setup();

        }

        public Network(int port, string ipAddress, GameThread gt, int bufferSize)
        {
            this.port = port;
            this.ipAddress = ipAddress;
            this.gt = gt;
            this.buffer = new byte[bufferSize];
            setup();
        }

        public int GetPort()
        {
            return port;
        }

        public string GetIPAddress()
        {
            return ipAddress;
        }


        public virtual void execute()
        {
            if (t != null)
            {
                throw new Exception("Error: thread already started.");
            }

            t = new Thread(receive);
            setup();
            Console.WriteLine("EXECUTE");
            t.Start();
        }
        public abstract void setup();

        protected abstract void receive();

        public abstract void send(P packet);

    }
}
