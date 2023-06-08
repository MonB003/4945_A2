using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N = _4945_A2.Network.Network;

namespace _4945_A2.Threads
{
    public class UserThread : T
    {
        N network;

        public UserThread(N network) {
            this.network = network;
        }

        public void send() { 
            // Take in a obj
            // Then give it to the network to send.
        }


    }
}
