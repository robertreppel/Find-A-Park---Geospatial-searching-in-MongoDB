using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GeoData;
using log4net;
using Messages;
using NServiceBus;

namespace GeoDataChangeNotifier
{
    public class GeoInfoImportedMessageHandler : IHandleMessages<GeoInfoImported>
    {
        public void Handle(GeoInfoImported message)
        {
            var geoName = message.GeoName;
            if(geoName.IsPark)
            {
                Logger.Info("Park information imported: " + message.GeoName.Name);
                var rssPublisher = new RssPublisher();
                rssPublisher.AddItem(message.GeoName);  
            }
        }

        private static readonly ILog Logger = LogManager.GetLogger(typeof(GeoInfoImportedMessageHandler));
    }
}