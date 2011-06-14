using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Messages
{
public class Geoname
    {
        public long GeonameId;
        public string Name;
        public string Asciiname;
        public string Alternatenames;
        public double Latitude;
        public double Longitude;
        public string FeatureClass;
        public string FeatureCode;
        public string CountryCode;
        public string Cc2;
        public string Admin1Code;
        public string Admin2Code;
        public string Admin3Code;
        public string Admin4Code;
        public long Population;
        public decimal? Elevation;
        public decimal? Gtopo30;
        public string Timezone;
        public DateTime? ModificationDate;

        public bool IsCity { get; set; }

        public bool IsPark { get; set; }

        public string StateCode { get; set; }

}

    public class Location   
    {
        public decimal Long;
        public decimal Lat;
    }
}

