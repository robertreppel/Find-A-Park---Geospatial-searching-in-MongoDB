using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FakeItEasy;
using GeoData;
using log4net;
using Messages;
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

            var log = A.Fake <ILog>();
            var geoRepository = new MongoGeoDataStore("mongodb://localhost", "geonamestest");
            var geoDataImporter = new GeoDataImporter(log, geoRepository);
            geoDataImporter.ImportGeonamesFrom(fileName);

            ImportedGeoname geoName = geoRepository.ByGeonameId(7723040);
            Assert.That(geoName != null);
        }       
    }
}
