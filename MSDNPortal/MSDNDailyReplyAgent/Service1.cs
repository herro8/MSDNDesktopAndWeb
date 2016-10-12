using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MSDNDailyReplyAgent
{
    public partial class Service1 : ServiceBase
    {

        private System.Timers.Timer timer;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.BaseDirectory+ "\\MSDNDailyReplyAgent.exe.config"));

            //read hour interval
            var _hourinterval = System.Configuration.ConfigurationManager.AppSettings["HourInterval"].ToString();

            int hourinterval = -1;

            if (!Int32.TryParse(_hourinterval, out hourinterval))
            {
                hourinterval = 120;
            }



            //init timer
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000 * 60 * hourinterval;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //do not work at the weekend
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                return;
            }
            //no need to trace data out of working hour
            if (DateTime.Now.Hour >= 9 && DateTime.Now.Hour <= 19)
            {
                StartDownload();
            }
        }

        private static void StartDownload()
        {
            LogInterface.LogInfo("START A NEW WORKING PROCESS");
            var msdnguy = new Ahead.Subscription.MSDNStuff();
            msdnguy.log += new Ahead.Support.LogMessageEventHandler(LogInterface.LogInfo);
            msdnguy.DoBusinessWork();
            var uifxguy = new Ahead.Subscription.UIFXServerStuff();
            uifxguy.log += new Ahead.Support.LogMessageEventHandler(LogInterface.LogInfo);
            uifxguy.SetInitCount();
            uifxguy.DoBusinessWork();
            LogInterface.LogInfo("WORKING DONE WITHOUT ERROR");
        }
        

        protected override void OnStop()
        {
            timer.Stop();
            timer = null;

        }
    }


    public class LogInterface
    {
        private static log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void LogInfo(string msg)
        {
            Logger.Info(msg);
        }

    }
}
