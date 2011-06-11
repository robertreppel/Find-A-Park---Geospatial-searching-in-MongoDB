﻿using System;
using FileHelpers;
using GeoData;
using log4net;
using Messages;
using NServiceBus;

namespace GeoInfoImport
{
    internal class GeoDataImporter    
    {
        private readonly ILog _log;
        private readonly IGeoDataStore _geoDataStore;
        public IBus Bus { get; set; }

        public GeoDataImporter(ILog log, IGeoDataStore geoDataStore)
        {
            _log = log;
            _geoDataStore = geoDataStore;
        }

        public void ImportGeonamesFrom(string fileName)
        {
            var geoNames = new FileHelperAsyncEngine(typeof(Geoname));
            geoNames.BeginReadFile(fileName);

            _geoDataStore.DeleteAll();

            long cnt = 0;
            foreach (Geoname geoName in geoNames)
            {
                //Store the name in a format suitable for case-insensitive searching:
                geoName.Name = geoName.Name.ToUpper();

                if (Bus != null)
                {
                    //Only publish if a message bus has been instantiated:
                    Console.Write(".");
                    Bus.Publish(new GeoInfoImported(geoName));
                }

                if (geoName.IsCity)
                {
                    SaveCity(geoName, cnt);
                }

                // Parks: http://127.0.0.1:28017/local/geonames/?filter_FeatureClass=L&filter_FeatureCode=PRK
                if(geoName.IsPark)
                {
                    SavePark(geoName);
                }

                cnt++;
            }

            _geoDataStore.DoIndexing();

            geoNames.Close();
        }

        private void SaveCity(Geoname geoName, long cnt)
        {
            _geoDataStore.Save(geoName);
            _log.Info(String.Format("Id: {0}, Name: {1}, State: {2}", geoName.GeonameId, geoName.Name, geoName.StateCode));
        }

        private void SavePark(Geoname geoName)
        {
            var park = new Park {GeonameId = geoName.GeonameId, Name = geoName.Asciiname};
            //Store lat and long in a format fit for geospatial indexing:
            var location = new double[2];
            location[0] = geoName.Latitude;
            location[1] = geoName.Longitude;
            park.Location = location;
            park.StateCode = geoName.StateCode;

            _geoDataStore.Save(park);
        }
    }
}