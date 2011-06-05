using System;
using MongoDB.Bson.Serialization.Attributes;

namespace GeoData
{
    public class Park 
    {
        [BsonId]
        public long GeonameId;
        public string Name { get; set; }
        public double[] Location { get; set; }
        public string StateCode { get; set; }
    }
}