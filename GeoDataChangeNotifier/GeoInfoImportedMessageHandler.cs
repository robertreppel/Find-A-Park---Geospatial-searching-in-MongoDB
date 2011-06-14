﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            if (geoName.IsPark)
            {
                Logger.Info("Send email about new park information: " + message.GeoName.Name);
            }
        }

        private static readonly ILog Logger = LogManager.GetLogger(typeof(GeoInfoImportedMessageHandler));
    }
}