using System;
using log4net;
using Messages;
using NServiceBus;

namespace RssFeedUpdater
{
    public class GeoInfoImportedMessageHandler : IHandleMessages<GeoInfoImported>
    {
        public void Handle(GeoInfoImported message)
        {
            //throw new Exception("BANG");

            var geoName = message.GeoName;
            if (geoName.IsPark)
            {
                Logger.Info("Park information imported: " + message.GeoName.Name);
                var rssPublisher = new ParksRssPublisher();
                rssPublisher.AddItem(message.GeoName);
            }
        }

        private static readonly ILog Logger = LogManager.GetLogger(typeof(GeoInfoImportedMessageHandler));
    }
}