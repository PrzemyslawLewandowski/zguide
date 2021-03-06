﻿using System;
using System.Text;
using ZeroMQ;

namespace ZMQGuide
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Collecting updates from weather server…");

            // default zipcode is 10001
            var zipcode = "10001 "; // the reason for having a space after 10001 is in case of the message would start with 100012 which we are not interested in

            if (args.Length > 0)
                zipcode = args[1] + " ";

            using (var context = ZmqContext.Create())
            using (var subscriber = context.CreateSocket(SocketType.SUB))
            {
                subscriber.Subscribe(Encoding.Unicode.GetBytes(zipcode));
                subscriber.Connect("tcp://localhost:5556");

                const int updatesToCollect = 100;
                int totalTemperature = 0;

                for (int updateNumber = 0; updateNumber < updatesToCollect; updateNumber++)
                {
                    string update = subscriber.Receive(Encoding.Unicode);
                    totalTemperature += Convert.ToInt32(update.Split()[1]);
                }

                Console.WriteLine("Average temperature for zipcode {0} was {1}F", zipcode, totalTemperature/updatesToCollect);
            }
        }
    }
}