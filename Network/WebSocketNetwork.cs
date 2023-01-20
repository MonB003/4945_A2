using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

using GameThread = _4945_A2.Threads.GameThread;
using P = _4945_A2.Packet.Packet;

namespace _4945_A2.Network
{
    public class WebSocketNetwork : Network
    {
        private HubConnection connection;
        public WebSocketNetwork(GameThread gt) : base(gt)
        {
        }

        public WebSocketNetwork(int port, string ipAddress, GameThread gt) : base(port, ipAddress, gt)
        {
        }

        public WebSocketNetwork(int port, string ipAddress, GameThread gt, int bufferSize) : base(port, ipAddress, gt, bufferSize)
        {
        }

        public override void send(P packet)
        {
            Console.WriteLine("SEND: " + packet.ToString());
            byte[] b = packet.GetBuffer();
            connection.SendAsync("SendPacket", b[0], b[1], b[2], b[3], b[4], b[5]);
        }

        public override void setup()
        {
            connection = new HubConnectionBuilder().WithUrl("http://10.0.0.132:5000/gamehub").Build();
            Console.WriteLine("Connection " + connection.ToString());
            connection.StartAsync().Wait();
            Console.WriteLine(connection.State + " " + connection.ConnectionId);
        }

        protected override void receive()
        {
            connection.On("RecievePacket", (byte user, byte action, byte x, byte y, byte z) =>
            {
                P p = new P(user, action, x, y, z);
                Console.WriteLine("RECIEVED: " + p.ToString());
            });
        }
    }
}

