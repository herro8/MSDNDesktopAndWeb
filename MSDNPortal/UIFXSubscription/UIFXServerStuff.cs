using Ahead.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ahead.Support;

namespace Ahead.Subscription
{
    public class UIFXServerStuff
    {

        public static void Main()
        {
            UIFXServerStuff uifx = new Subscription.UIFXServerStuff();
            uifx.DoBusinessWork();
        }

        private string FormatRSSAddressForUIFX = "https://forums.asp.net/search/client/author/@name/1/30?s=postDate&amp;d=desc";
        public event LogMessageEventHandler log;

        public UIFXServerStuff()
        {
            log = null;
        }


        private int InsertCount
        { get; set; }


        private int UpdateCount { get; set; }

        public void SetInitCount()
        {

            InsertCount = 0;
            UpdateCount = 0;
        }

        public void DoBusinessWork()
        {

            List<SupportAgentProfile> profiles = new List<SupportAgentProfile>();
            Ahead.Data.DB1 db = new DB1();

            #region read asp.net guy from txt
            profiles = db.SupportAgents.Where(w => w.DeliverASPNET != "0").Select(s => new SupportAgentProfile() { Alias = s.Alias, ProfileAddress = s.DeliverASPNET }).ToList();
            #endregion


            #region read recent activity form asp.net website
            foreach (var profile in profiles)
            {

                try
                {
                    profile.ProfileAddress = FormatRSSAddressForUIFX.Replace("@name", profile.ProfileAddress);
                    HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(profile.ProfileAddress);
                    wr.Timeout = 1000 * 60;
                    WebResponse resp = wr.GetResponse();
                    Stream stream = resp.GetResponseStream();
                    JSONResult json_result = new JSONResult();
                    using (StreamReader r = new StreamReader(stream))
                    {
                        string json = r.ReadToEnd();
                        json_result = JsonConvert.DeserializeObject<JSONResult>(json);
                    }


                    foreach (var json in json_result.Results)
                    {
                        ForumActivity fa = new ForumActivity();
                        fa.Alias = profile.Alias;
                        fa.Comment = "Replied to a UIFX-Server thread " + string.Format("<a href=\"https://forums.asp.net{0}\" target=\"_blank\">{1}</a>",json.Url, json.Title);
                        //check add new or update available
                        if (db.ForumActivities.Count(w => w.Alias == fa.Alias && w.Comment == fa.Comment) == 0)
                        {
                            fa.LastUpdateTime = DateTime.Now;
                            fa.LogTime = DateTime.Now.ToString("yyyy-MM-dd");
                            fa.OperationType = "Replied to a forums thread";
                            fa.PublishTime = json.CreatedDate;
                            //check date, if today then save to database
                            if (!CheckPubDate(fa.PublishTime))
                            {
                                continue;
                            }
                            db.ForumActivities.Add(fa);
                            InsertCount++;
                        }
                        else
                        {
                            var data = db.ForumActivities.Where(w => w.Alias == fa.Alias && w.Comment == fa.Comment).FirstOrDefault();
                            data.LastUpdateTime = DateTime.Now;
                            UpdateCount++;
                        }
                    }
                    db.SaveChanges();
                }
                catch (Exception exx)
                {
                    if (log!=null)
                    {

                        log(exx.Message);
                    }
                    continue;
                }
            }

            Console.WriteLine(string.Format("Query for UIFX reply count, insert {0}, update {1} ", InsertCount, UpdateCount));
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
    }
}
