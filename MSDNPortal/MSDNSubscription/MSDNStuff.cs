using Ahead.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Ahead.Support;

namespace Ahead.Subscription
{
    public class MSDNStuff
    {
        public static void Main()
        {

            new MSDNStuff().DoBusinessWork();

            Console.WriteLine("ALL work done");
            Console.Read();
        }

        public MSDNStuff()
        {
            log = null;
        }

        private string FormatRSSAddress = "https://social.msdn.microsoft.com/Profile/u/activities/feed?displayName=@name";
        public event LogMessageEventHandler log;

        public void DoBusinessWork()
        {
          
            Ahead.Data.DB1 db = new DB1();
            List<SupportAgentProfile> profiles = db.SupportAgents.Where(w => w.DeliverMSDN != "0").Select(s => new SupportAgentProfile() { Alias = s.Alias, ProfileAddress = s.DeliverMSDN }).ToList();

            #region read profile content from MSDN page
            List<Ahead.Data.SupportAgent> supportagents = db.SupportAgents.ToList();
            string timeformat = DateTime.Now.ToString("yyyy-MM-dd");
            List<ForumActivity> recentforumactivity = db.ForumActivities.Where(w => w.LogTime == timeformat).ToList();

            foreach (var profile in profiles)
            {
                try
                {
                    profile.ProfileAddress = FormatRSSAddress.Replace("@name", profile.ProfileAddress);
                    Ahead.Data.SupportAgent supportagent = supportagents.Where(w => w.Alias == profile.Alias).FirstOrDefault();
                    if (supportagent == null)
                    {
                        Console.WriteLine("find a support agent (" + profile.Alias + ") not in database");
                        continue;
                    }
                    HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(profile.ProfileAddress);
                    wr.Timeout = 1000 * 60;
                    WebResponse resp = wr.GetResponse();
                    Stream stream = resp.GetResponseStream();

                    XmlTextReader reader = new XmlTextReader(stream);
                    reader.XmlResolver = null;
                    XmlDocument doc = new XmlDocument();
                    doc.Load(reader);
                    var nodes = doc.SelectNodes("//item");

                    foreach (XmlNode node in nodes)
                    {
                        ForumActivity fa = new ForumActivity();
                        fa.PublishTime = DateTime.Parse(node.SelectSingleNode("pubDate").InnerText);

                        if (!CheckPubDate(fa.PublishTime))
                        {
                            //no need to download other dates beside today
                            continue;
                        }
                        //determine add or update
                        fa.Comment = node.SelectSingleNode("description").InnerText;
                        if (recentforumactivity.Count(w => w.Alias == supportagent.Alias && w.Comment == fa.Comment) == 0)
                        {
                            fa.Alias = supportagent.Alias;
                            fa.Comment = node.SelectSingleNode("description").InnerText;
                            fa.LogTime = fa.PublishTime.ToString("yyyy-MM-dd");
                            fa.LastUpdateTime = DateTime.Now;
                            fa.OperationType = CheckOperationType(node.SelectSingleNode("title").InnerText);
                            db.ForumActivities.Add(fa);
                        }
                        else
                        {
                            var data = db.ForumActivities.Where(w => w.Alias == supportagent.Alias && w.Comment == fa.Comment).FirstOrDefault();
                            if (data == null)
                            {
                                continue;
                            }
                            data.LastUpdateTime = DateTime.Now;
                        }
                    }
                }
                catch (Exception exx)
                {
                    if (log != null)
                    {
                        log(exx.Message);
                    }
                    continue;
                }
                db.SaveChanges();
            }
            #endregion
        }


        //we only need log current date working 
        private bool CheckPubDate(DateTime startTime)
        {
            DateTime today = DateTime.Today;
            DateTime tempToday = new DateTime(startTime.Year, startTime.Month, startTime.Day);
            if (today == tempToday)
                return true;
            else
                return false;
        }

        private string CheckOperationType(string innerText)
        {
            string operationtype = string.Empty;
            if (innerText.IndexOf("Marked a proposed answer") != -1)
            {
                operationtype = "Marked a proposed answer";
            }
            else if (innerText.IndexOf("Marked an answer") != -1)
            {
                operationtype = "Marked an answer";
            }
            else if (innerText.IndexOf("Answered the question") != -1)
            {
                operationtype = "Answered the question";
            }
            else if (innerText.IndexOf("Replied to a forums thread") != -1)
            {
                operationtype = "Replied to a forums thread";
            }
            else if (innerText.IndexOf("Contributed a proposed answer") != -1)
            {
                operationtype = "Contributed a proposed answer";
            }
            else if (innerText.IndexOf("Voted a helpful post") != -1)
            {
                operationtype = "Voted a helpful post";
            }
            else if (innerText.IndexOf("Included a code snippet") != -1)
            {
                operationtype = "Included a code snippet";
            }
            else if (innerText.IndexOf("Quickly answered the question") != -1)
            {
                operationtype = "Quickly answered the question";
            }
            else
            {
                operationtype = innerText;
            }
            return operationtype;

        }

        private DateTime ConvertTOLocalDate(string dateformat)
        {
            DateTime local_datetime = DateTime.ParseExact(dateformat, "yyyy-MM-dd HH:mm:ss", new CultureInfo("zh-CN"), DateTimeStyles.AssumeUniversal);
            return local_datetime;
        }
    }
}
