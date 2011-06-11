using GeoData;
using NServiceBus;

namespace Messages
{
    public class GeoInfoImported : IMessage
    {
        public Geoname GeoName { get; private set; }

        public GeoInfoImported(Geoname geoName)
        {
            GeoName = geoName;
        }
    }
}