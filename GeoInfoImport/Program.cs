using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using GeoData;

namespace GeoInfoImport
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = InitializeIocContainer();

            bool doImport = AreThereValidParametersForDataImportIn(args);

            if(doImport)
            {
                Console.WriteLine("Started import at: {0}", DateTime.Now.ToShortTimeString());

                var importer = container.Resolve<IImportData>();
                importer.ImportFromFile(args[1]);

                Console.WriteLine("Finished import at: {0}", DateTime.Now.ToShortTimeString());
                var geoRepository = container.Resolve<IGeoDataStore>();
                Console.WriteLine("No. of geonames in DB: {0}", geoRepository.GeonamesCount());
            }
            Console.ReadKey();
        }

        private static WindsorContainer InitializeIocContainer()
        {
            var container = new WindsorContainer();
            container.Register(Component.For<IImportData>().ImplementedBy<GeoDataImporter>());
            container.Register(Component.For<ILog>().ImplementedBy<Log>());
            container.Register(Component.For<IGeoDataStore>().ImplementedBy<MongoGeoDataStore>());

            RegisterAopInterceptors(container);

            container.Kernel.ProxyFactory.AddInterceptorSelector(
                    new ModelInterceptorsSelector()
                );
            return container;
        }

        private static void RegisterAopInterceptors(WindsorContainer container)
        {
            string assemblyContainingInterceptors = ConfigurationManager.AppSettings["AssemblyContainingInterceptors"];
            Assembly asm = Assembly.Load(assemblyContainingInterceptors);
            Type ti = typeof(IInterceptor);
            foreach (Type t in asm.GetTypes())
            {
                if (ti.IsAssignableFrom(t))
                {
                    container.Register(Component.For(t));
                }
            }
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
