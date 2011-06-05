using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NUnit.Framework;

namespace FindAPark.Controllers
{
    [TestFixture]
    public class TestGeoController
    {
        [Test]
        public void ShouldSplitCityAndState()
        {
            string cityState = "Bute, Mo";
            string city = null;
            string state = null;
            GeoController.ParseInputForCityAndState(cityState, out city, out state);

            Assert.AreEqual("Bute", city);
            Assert.AreEqual("Mo", state);
        }


        [Test]
        public void ShouldSplitCity()
        {
            const string cityState = "Bute";
            string city = null;
            string state = null;
            GeoController.ParseInputForCityAndState(cityState, out city, out state);

            Assert.AreEqual("", city);
            Assert.AreEqual("", state);
        }

        [Test]
        public void ShouldSplitState()
        {
            const string cityState = ",Mo";
            string city = null;
            string state = null;
            GeoController.ParseInputForCityAndState(cityState, out city, out state);

            Assert.AreEqual("", city);
            Assert.AreEqual("", state);
        }
    }
}