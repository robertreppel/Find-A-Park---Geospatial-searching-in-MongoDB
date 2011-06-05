using System.Collections.Generic;
using GeoData;
using NUnit.Framework;

namespace GeoInfoImport
{
    /**
     * Populate the database with a known set of test data before tests are run.
     */
    [SetUpFixture]
    class TestData
    {
        private const string ConnectionString = "mongodb://localhost";
        private const string MongoDbName = "geonamesplacestest";

        [SetUp]
	    public void Populate()
	    {
            const string testDataFile =
                @"GeoDataFiles\US10000.txt";

            var log = new Log();
            var geoRepository = new MongoGeoDataStore(ConnectionString, MongoDbName);
            var geoDataImporter = new GeoDataImporter(log, geoRepository);
            geoDataImporter.ImportGeonamesFrom(testDataFile);
	    }
    }

    [TestFixture]
    class TestGeoDataSearches
    {
        private IGeoDataStore _geoDataStore;

        [SetUp]
        public void InitializeGeoDataStore()
        {
            _geoDataStore = new MongoGeoDataStore("mongodb://localhost", "geonamesplacestest");
        }

        [Test] public void ShouldFindPlacesBeginningWithUppercaseLetters()
        {
            List<Place> places = _geoDataStore.PlacesStartingWith("Wi".ToUpper());

            CollectionAssert.IsNotEmpty(places);
            Assert.IsTrue(places.Count == 3);
        }

        [Test]
        public void ShouldFindPlacesBeginningWithLowercaseLetters()
        {
            List<Place> places = _geoDataStore.PlacesStartingWith("Wi");

            CollectionAssert.IsNotEmpty(places);
            Assert.IsTrue(places.Count == 3);
        }

        [Test]
        public void GivenPlaceAndStateFindFirstMatch()
        {
            const string city = "Greenview Estates";
            const string state = "IL";

            var result = _geoDataStore.FindBy(city.ToUpper(), state);

            Assert.IsNotNull(result);
        }

        [Test]
        public void FindNearestParksShouldReturnSome()
        {
            const double latitude = 47.0;
            const double longitude = 118.0;
            const int distanceKm = 100;

            var result = _geoDataStore.FindParksWithin(latitude, longitude, distanceKm);

            Assert.IsNotNull(result);
        }
    }
}
