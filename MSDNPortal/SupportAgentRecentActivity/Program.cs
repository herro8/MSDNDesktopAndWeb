
using System;
using System.Collections.Generic;

namespace SupportAgentRecentActivity
{
    class Program
    {
        public static System.Timers.Timer timer = new System.Timers.Timer();
        static void Main(string[] args)
        {
            //init timer
          
            timer.Interval = 1000 * 60 * 0.5;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

            Console.WriteLine("Service is working!");


          

            Console.Read();
            
        }

        private static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();

            //do not work at the weekend
            if (DateTime.Now.DayOfWeek== DayOfWeek.Saturday || DateTime.Now.DayOfWeek== DayOfWeek.Sunday)
            {
                return;
            }
            //no need to trace data out of working hour
            if (DateTime.Now.Hour>=9 || DateTime.Now.Hour<= 19)
            {
                StartDownload();
            }

            //all work done, reinit timer 
            timer.Interval = 1000 * 60 * 45;
            timer.Start();

        }


        private static void StartDownload()
        {
            var msdnguy = new Ahead.Subscription.MSDNStuff();
            msdnguy.log += LogMessage;
            Console.WriteLine("MSDN Service Start!");
            msdnguy.DoBusinessWork();
            Console.WriteLine("MSDN Service Done!");
            var uifxguy = new Ahead.Subscription.UIFXServerStuff();
            uifxguy.log += LogMessage;
            Console.WriteLine("Asp.net Service Start!");
            uifxguy.SetInitCount();
            uifxguy.DoBusinessWork();
            Console.WriteLine("Asp.net Service Done!");
        }

        private static void LogMessage(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
