using System;
using System.IO;
using Argotic.Syndication;
using Messages;

namespace RssFeedUpdater
{
    internal class ParksRssPublisher
    {
        const string SiteUri = "http://localhost:10001/Content/ParksRss.xml";

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

        private static string ParksRssFeedPath()
        {
            return @"C:\Users\rreppel\Documents\Visual Studio 2010\Projects\Find-A-Park---Geospatial-searching-in-MongoDB\GeoDataChangeNotifier\Content\ParksRss.xml";
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