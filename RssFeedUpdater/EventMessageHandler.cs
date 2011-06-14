using log4net;
using Messages;
using NServiceBus;

namespace Subscriber1
{
    public class EventMessageHandler : IHandleMessages<GeoInfoImported>
    {
        public void Handle(GeoInfoImported message)
        {
            if (message.GeoName != null)
            {
                var geoName = message.GeoName;
                Logger.Info(string.Format("Subscriber 1 received EventMessage with Id {0}.", geoName.Name));
            }

        }

        private static readonly ILog Logger = LogManager.GetLogger(typeof (EventMessageHandler));
    }
}
