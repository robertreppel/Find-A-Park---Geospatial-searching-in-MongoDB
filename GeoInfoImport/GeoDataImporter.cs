using System;
using System.Collections.Generic;
using FileHelpers;
using GeoData;
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
            this._log = log;
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
                    Bus.Publish(new GeoInfoImported(geoName));
                }

                if (geoName.FeatureClass == "P")
                {
                    SaveCity(geoName, cnt);
                }

                // Parks: http://127.0.0.1:28017/local/geonames/?filter_FeatureClass=L&filter_FeatureCode=PRK
                if(geoName.FeatureClass.Equals("L") && geoName.FeatureCode.Equals("PRK"))
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
            _log.WriteInfoAbout(geoName, cnt);
        }

        private void SavePark(Geoname geoName)
        {
            var park = new Park();
            park.GeonameId = geoName.GeonameId;
            park.Name = geoName.Asciiname;
            //Store lat and long in a format fit for geospatial indexing:
            var location = new double[2];
            location[0] = geoName.Latitude;
            location[1] = geoName.Longitude;
            park.Location = location;
            park.StateCode = geoName.Admin1Code;

            _geoDataStore.Save(park);
        }
    }

    internal class GeoInfoImported : IMessage
    {
        public Geoname GeoName { get; private set; }

        public GeoInfoImported(Geoname geoName)
        {
            GeoName = geoName;
        }
    }
}