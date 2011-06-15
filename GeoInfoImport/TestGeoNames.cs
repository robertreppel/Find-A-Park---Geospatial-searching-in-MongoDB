using System.IO;
using System.Linq;
using System.Text;
using FakeItEasy;
using GeoData;
using NUnit.Framework;

namespace GeoInfoImport
{
    [TestFixture]
    class TestGeoNames
    {
        [Test]
        public void ShouldImport()
        {
            const string fileName =
                @"GeoDataFiles\US10000.txt";

            var log = A.Fake<ILog>();
            var geoRepository = A.Fake<IGeoDataStore>();
            var geoDataImporter = new GeoDataImporter(log, geoRepository);
            geoDataImporter.ImportFromFile(fileName);

            A.CallTo(() => log.WriteInfoAbout(null, 0)).WithAnyArguments().MustHaveHappened(Repeated.Exactly.Times(357));
            A.CallTo(() => geoRepository.Save(new Geoname())).WithAnyArguments().MustHaveHappened(Repeated.Exactly.Times(357));
        }

    }
}
