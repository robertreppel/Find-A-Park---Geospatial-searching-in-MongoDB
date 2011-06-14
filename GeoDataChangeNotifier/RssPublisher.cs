using System;
using System.IO;
using Argotic.Syndication;
using GeoData;
using Messages;

namespace GeoDataChangeNotifier
{
    internal class RssPublisher
    {
        const string SiteUri = "http://localhost:10001/Content/ParksRss.xml";

        public void DeleteAllItems()
        {
            //Create an empty feed, overwriting currently existing items:
            var feed = new RssFeed();
            SetChannelInfo(feed);
            Save(feed);
        }

        public void AddItem(Geoname geoName)
        {

            var fromExistingParksRssItems = new Uri(SiteUri);
            RssFeed feed = RssFeed.Create(fromExistingParksRssItems);

            RssItem item = CreateRssFeedItem(geoName.Name, geoName.StateCode);

            feed.Channel.AddItem(item);

            Save(feed);
        }

        private static void Save(RssFeed feed)
        {
            using (var stream = new FileStream(ParksRssFeedPath(), FileMode.Create, FileAccess.Write))
            {
                feed.Save(stream);
            }
        }

        private void SetChannelInfo(RssFeed feed)
        {
            feed.Channel.Link           = new Uri(SiteUri);
            feed.Channel.Title          = "New US Parks";
            feed.Channel.Description    = "Keeping you posted for the Great Outdoors";
        }

        private static string ParksRssFeedPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory + "Content/ParksRss.xml";
        }

        private static RssItem CreateRssFeedItem(string name, string state)
        {
            var item = new RssItem();
            item.Title = "New Park: " + name;


            item.Link = new Uri("http://localhost:10000"); //Uri of the FindAPark site.
            item.Description = String.Format("{0} is located in {1}.", name, state);
            return item;
        }
    }
}