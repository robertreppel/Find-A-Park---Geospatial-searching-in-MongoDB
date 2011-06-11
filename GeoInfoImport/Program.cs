using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GeoData;
using log4net;
using NServiceBus;

namespace GeoInfoImport
{
    class Program
    {
        static void Main(string[] args)
        {
            IBus bus = ConfigureMessageBus();

            bool doImport = AreThereValidParametersForDataImportIn(args);

            var geoRepository = new MongoGeoDataStore();
            if(doImport)
            {   
                Console.Write("Press any key to begin geo information import.");
                Console.ReadKey();
                Console.WriteLine("Started import at: {0}", DateTime.Now.ToShortTimeString());

                ILog logger = LogManager.GetLogger("Name");
                var importer = new GeoDataImporter(logger, geoRepository);
                importer.Bus = bus;
                string file = args[1];
                importer.ImportGeonamesFrom(file);

                Console.WriteLine();
                Console.WriteLine("Finished import at: {0}", DateTime.Now.ToShortTimeString());
                Console.WriteLine("No. of geonames in DB: {0}", geoRepository.GeonamesCount());
            }


            Console.ReadKey();
        }

        private static IBus ConfigureMessageBus()
        {
            //SetLoggingLibrary.Log4Net(log4net.Config.XmlConfigurator.Configure);
            var bus =  Configure.With() 
                .DefaultBuilder()
                .XmlSerializer()
                .MsmqTransport()
                .IsTransactional(true)
                .MsmqSubscriptionStorage()
                .UnicastBus()
                .LoadMessageHandlers()
                .CreateBus()
                .Start();
            return bus;
        }

        private static bool AreThereValidParametersForDataImportIn(string[] args)
        {

            if(args.Count() == 0)
            {
                return false;
            }
            bool doImport = false;
            if (!String.IsNullOrEmpty(args[0]))
            {
                if (args[0].Equals("import"))
                {
                    if (!String.IsNullOrEmpty(args[1]))
                    {
                        if (File.Exists(args[1]))
                        {
                            doImport = true;
                        } else
                        {
                            Console.WriteLine("File does not exist: {0}", args[1]);
                            Console.WriteLine();
                            Console.WriteLine("For the FindAPark demo website, download http://download.geonames.org/export/dump/US.zip.");
                            Console.WriteLine("Unzip it and place the 'US.txt' file into c:\\geodata, then run GeoInfoImport again.");
                            Console.WriteLine();
                            Console.WriteLine("(One more thing: The importer assumes that you have a default install of http://www.mongodb.org/ on your localhost.)");
                            Console.WriteLine();

                        }
                    }

                }
            }
            return doImport;
        }
    }
}
