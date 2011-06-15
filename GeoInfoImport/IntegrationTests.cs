using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FakeItEasy;
using GeoData;
using NUnit.Framework;

namespace GeoInfoImport
{
    [TestFixture]
    class IntegrationTests
    {

        [Test]
        public void ShouldImport()
        {
            const string fileName =
                @"GeoDataFiles\US10000.txt";

            var log = new Log();
            var geoRepository = new MongoGeoDataStore("mongodb://localhost", "geonamestest");
            var geoDataImporter = new GeoDataImporter(log, geoRepository);
            geoDataImporter.ImportFromFile(fileName);

            Geoname geoName = geoRepository.ByGeonameId(7723040);
            Assert.That(geoName != null);
        }       
    }
}
