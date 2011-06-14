using NServiceBus;

namespace Messages
{
    //Don't do this. Udi says use interfaces for event messages. (No time to fix it before demo.)
    public class GeoInfoImported : IMessage
    {
        public Geoname GeoName { get; private set; }

        public GeoInfoImported(Geoname geoName)
        {
            GeoName = geoName;
        }
    }
}