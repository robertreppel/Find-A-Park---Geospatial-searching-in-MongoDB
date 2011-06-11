using System.Collections.Generic;
using System.Linq;
using FluentMongo.Linq;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace GeoData
{
    public class MongoGeoDataStore : IGeoDataStore
    {
        private readonly string _connectionString;
        private readonly string _mongoDbName;

        private static MongoDatabase _db;
        private const string PlacesCollection = "places";
        private const string ParksCollection = "parks";


        public MongoGeoDataStore()
        {
            //Defaults:
            _connectionString = "mongodb://localhost";
            _mongoDbName = "geonames";
            SetupMongoDb();            
        }

        public MongoGeoDataStore(string connectionString, string mongoDbName)
        {
            _connectionString = connectionString;
            _mongoDbName = mongoDbName;
            SetupMongoDb();
        }

        public void Save(Geoname geoname)
        {
            MongoCollection<Geoname> mongoCollection = _db.GetCollection<Geoname>(PlacesCollection);
            mongoCollection.Save(geoname);
        }

        public void Save(Park geoname)
        {
            MongoCollection<Geoname> mongoCollection = _db.GetCollection<Geoname>(ParksCollection);
            mongoCollection.Save(geoname);
        }

        public void DoIndexing()
        {
            var mongoCityCollection = _db.GetCollection<Geoname>(PlacesCollection);
            mongoCityCollection.EnsureIndex(IndexKeys.Ascending("Name"));
            mongoCityCollection.EnsureIndex(IndexKeys.Ascending("Name", "StateCode"));

            var mongoParksCollection = _db.GetCollection<Park>(ParksCollection);
            mongoParksCollection.EnsureIndex(IndexKeys.GeoSpatial("Location"));
            mongoParksCollection.EnsureIndex(IndexKeys.Ascending("Name"));

        }

        public void DeleteAll()
        {
            var mongoParksCollection = _db.GetCollection<Geoname>(ParksCollection);
            mongoParksCollection.RemoveAll();

            var mongoCityCollection = _db.GetCollection<Geoname>(PlacesCollection);
            mongoCityCollection.RemoveAll();
        }


        public List<Place> PlacesStartingWith(string letters)
        {
            var mongoCollection = _db.GetCollection<Geoname>(PlacesCollection).AsQueryable();
            var cityGeonameCollection = mongoCollection
                .Where(x => x.Name.StartsWith(letters.ToUpper()) 
                    && x.FeatureClass == "P")
                .ToList();

            var results = new List<Place>();
            foreach (var geoname in cityGeonameCollection)
            {
                results.Add(new Place() { 
                    Name=geoname.Name, 
                    Lat=geoname.Latitude, 
                    Long=geoname.Longitude,
                    StateCode = geoname.StateCode
                });
            }
            return results;
        }

        public Geoname FindBy(string city, string stateCode)
        {
            var mongoCollection = _db.GetCollection<Geoname>(PlacesCollection).AsQueryable();
            var result =  mongoCollection
                .Where(x => x.Name == city && x.StateCode == stateCode)
                .ToList().FirstOrDefault();

            return result;
        }

        public List<Park> FindParksWithin(double latitude, double longitude, int kilometers)
        {
            var parksCollection = _db.GetCollection<Park>(ParksCollection);

            //db.places.find( { loc : { $near : [50,50] , $maxDistance : 5 } } ).limit(20),
            var query = Query.Near("Location", latitude, longitude, kilometers);
            var cursor = parksCollection.Find(query); //.SetLimit(20);

            var result = new List<Park>();
            foreach (var hit in cursor)
            {
                result.Add(hit);
            }

            return result;
        }

        public Geoname ByGeonameId(long id)
        {
            var mongoCollection = _db.GetCollection<Geoname>(PlacesCollection).AsQueryable();
            return mongoCollection
                .Where(x => x.GeonameId == id)
                .ToList().FirstOrDefault();
        }

        private void SetupMongoDb()
        {
            if(_db == null)
            {
                MongoServer server = MongoServer.Create(_connectionString);
                _db = server.GetDatabase(_mongoDbName);                    
                BsonClassMap.RegisterClassMap<Geoname>();                
            }
        }

        public long GeonamesCount()
        {
            var mongoCollection = _db.GetCollection<Geoname>(PlacesCollection).AsQueryable();
            return mongoCollection.Count<Geoname>();
        }
    }
}