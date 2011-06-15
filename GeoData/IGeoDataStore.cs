using System.Collections.Generic;

namespace GeoData
{
    public interface IGeoDataStore
    {
        void Save(Geoname geoname);
        List<Place> PlacesStartingWith(string letters);
        Geoname FindBy(string city, string stateCode);
        List<Park> FindParksWithin(double latitude, double longitude, int kilometers);
        void Save(Park geoname);
        void DoIndexing();
        void DeleteAll();
        long GeonamesCount();
    }
}