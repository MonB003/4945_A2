using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameThread = _4945_A2.Threads.GameThread;
using P = _4945_A2.Packet.Packet;

namespace _4945_A2.Network
{
    public class WebSocketNetwork : Network
    {
        public WebSocketNetwork(GameThread gt) : base(gt)
        {

        }

        public WebSocketNetwork(int port, string ipAddress, GameThread gt) : base(port, ipAddress, gt) { }

        public WebSocketNetwork(int port, string ipAddress, GameThread gt, int bufferSize) : base(port, ipAddress, gt, bufferSize) { }



// implement these 

        public override void send(P packet)
        {
            throw new NotImplementedException();
        }

        protected override void receive()
        {
            throw new NotImplementedException();
        }

        public override void setup()
        {
            throw new NotImplementedException();
        }
    }
}
