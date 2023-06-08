using _4945_A2.Network;
using System;
using System.Threading;


using Network = _4945_A2.Network.Network;
using MultiCast = _4945_A2.Network.MulticastNetwork;

using Game = _4945_A2.Threads.GameThread;
using User = _4945_A2.Threads.UserThread;

using P = _4945_A2.Packet.Packet;

namespace Program { 
    public class Program
    {
        public static void testNetwork(Network n)
        {
            User u = new User(n);

            n.execute();

            Console.WriteLine("Network setup. Press any button to begin");
            Console.ReadLine();

            for (int i = 0; i < 10; i++)
            {   
                P p = new P();
                Thread.Sleep(1000);
                n.send(p);
            }
            Console.WriteLine("\n\nEND \n\n");
        }

        public static void Main(string[] args)
        {
            Game g = new Game();
            Network mCN = new MulticastNetwork(g);

            if (args.Length == 0)
            {
                mCN.execute();
                Console.WriteLine("Network Listening");
                Console.WriteLine("Hit any key to end program");
                Console.ReadLine();
                return;
            }
            testNetwork(mCN);
        }
    }
}