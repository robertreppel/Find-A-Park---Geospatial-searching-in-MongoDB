using System;
using System.Web.Mvc;
using FindAPark.Models;
using GeoData;

namespace FindAPark.Controllers
{
    public class GeoController : Controller
    {
        private readonly IMuseums _museums;
        private readonly IGeoDataStore _geoDataStore;

        public GeoController(IGeoDataStore geoDataStore)
        {
            _geoDataStore = geoDataStore;
        }

        //
        // GET: /Geo/
        // Museum: http://127.0.0.1:28017/local/geonames/?filter_FeatureClass=S&filter_FeatureCode=MUS 
        // Parks: http://127.0.0.1:28017/local/geonames/?filter_FeatureClass=L&filter_FeatureCode=PRK

        public ActionResult Cities(string q)
        {
            //We are using http://ericdotnet.wordpress.com/2009/04/09/jquery-search-box-and-aspnet-mvc/ 
            //for the autocomplete.
            string searchResult = String.Empty;
            if(!String.IsNullOrWhiteSpace(q))
            {
                var result = _geoDataStore.PlacesStartingWith(q);
                foreach (var city in result)
                {
                    searchResult += string.Format("{0}, {1}|\r\n", city.Name, city.StateCode);
                }            
            }
            return Content(searchResult);
        }

        public JsonResult Search(string cityAndState)
        {
            if(String.IsNullOrEmpty(cityAndState))
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            string city;
            string stateCode;
            ParseInputForCityAndState(cityAndState, out city, out stateCode);

            var searchResult = _geoDataStore.FindBy(city, stateCode);
            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FindParksNear(double latitude, double longitude, int kmDistance)
        {
            var searchResult = _geoDataStore.FindParksWithin(latitude, longitude, kmDistance);
            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }

        internal static void ParseInputForCityAndState(string cityAndState, out string city, out string stateCode)
        {
            if (cityAndState.Contains(","))
            {

                string[] inputSplit = cityAndState.Split(',');
                city = inputSplit[0].Trim();
                stateCode = inputSplit[1].Trim();
                if(String.IsNullOrEmpty(city) || String.IsNullOrEmpty(stateCode))
                {
                    city = "";
                    stateCode = "";
                }
            } else
            {
                city = "";
                stateCode = "";
            }
        }

    }
}
