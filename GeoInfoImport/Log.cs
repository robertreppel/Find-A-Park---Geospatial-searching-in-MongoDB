using System;
using GeoData;

namespace GeoInfoImport
{
    public interface ILog
    {
        void WriteInfoAbout(Geoname geoName, long itemCount);
    }

    public class Log : ILog
    {


        public void WriteInfoAbout(Geoname geoname, long itemCount)
        {
            if(itemCount % 10000 == 0)
            {
                Console.WriteLine("Item count: {6} Id: {0}, Name: {1}, {2}, {3}, {4}, {5}", geoname.GeonameId, geoname.Name, geoname.Admin1Code, geoname.Admin2Code, geoname.Admin3Code, geoname.Admin4Code, itemCount);
            }
        }
    }
}