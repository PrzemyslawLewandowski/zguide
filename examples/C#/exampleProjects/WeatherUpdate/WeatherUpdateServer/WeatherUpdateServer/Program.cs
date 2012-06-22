//
//  Weather update server
//  Binds PUB socket to tcp://*:5556
//  Publishes random weather updates
//

//  Author:     Michael Compton, Tomas Roos, Przemyslaw Lewandowski
//  Email:      michael.compton@littleedge.co.uk, ptomasroos@gmail.com, prz.lewandowski@gmail.com

using System;
using System.Text;
using ZeroMQ;

namespace ZMQGuide
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            using (var context = ZmqContext.Create())
            using (var publisher = context.CreateSocket(SocketType.PUB))
            {
                publisher.Bind("tcp://*:5556");

                var randomizer = new Random(DateTime.Now.Millisecond);

                while (true)
                {
                    //  Get values that will fool the boss
                    int zipcode = randomizer.Next(0, 100000);
                    int temperature = randomizer.Next(-80, 135);
                    int relativeHumidity = randomizer.Next(10, 60);

                    string update = zipcode.ToString() + " " + temperature.ToString() + " " + relativeHumidity.ToString();

                    //  Send message to 0..N subscribers via a pub socket
                    publisher.Send(update, Encoding.Unicode);
                }
            }
        }
    }
}
