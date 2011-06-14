using System.Collections.Generic;

namespace GeoData
{
    public interface IGeoDataStore
    {
        void Save(ImportedGeoname importedGeoname);
        List<Place> PlacesStartingWith(string letters);
        ImportedGeoname FindBy(string city, string stateCode);
        List<Park> FindParksWithin(double latitude, double longitude, int kilometers);
        void Save(Park geoname);
        void DoIndexing();
        void DeleteAll();
    }
}