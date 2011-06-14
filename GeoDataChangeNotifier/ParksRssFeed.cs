using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Argotic.Syndication;
using Messages;

namespace GeoDataChangeNotifier
{
    internal class ParksRssFeed
    {
        const string SiteUri = "http://localhost:10001/Content/ParksRss.xml";

        public void DeleteAllItems()
        {
            //Create an empty feed, overwriting currently existing items:
            var feed = new RssFeed();
            SetChannelInfo(feed);
            Save(feed);
        }

        private static void Save(RssFeed feed)
        {
            using (var stream = new FileStream(ParksRssFeedPath(), FileMode.Create, FileAccess.Write))
            {
                feed.Save(stream);
            }
        }

        private static void SetChannelInfo(RssFeed feed)
        {
            feed.Channel.Link = new Uri(SiteUri);
            feed.Channel.Title = "New US Parks";
            feed.Channel.Description = "Keeping you posted for the Great Outdoors";
        }

        private static string ParksRssFeedPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory + "Content/ParksRss.xml";
        }
    }
}