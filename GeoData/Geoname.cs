using System;
using FileHelpers;
using MongoDB.Bson.Serialization.Attributes;

namespace GeoData
{
    /*
        Geodata from http://download.geonames.org/export/dump/US.zip
     
        geonameid         : integer id of record in geonames database
        name              : name of geographical point (utf8) varchar(200)
        asciiname         : name of geographical point in plain ascii characters, varchar(200)
        alternatenames    : alternatenames, comma separated varchar(5000)
        latitude          : latitude in decimal degrees (wgs84)
        longitude         : longitude in decimal degrees (wgs84)
        feature class     : see http://www.geonames.org/export/codes.html, char(1)
        feature code      : see http://www.geonames.org/export/codes.html, varchar(10)
        country code      : ISO-3166 2-letter country code, 2 characters
        cc2               : alternate country codes, comma separated, ISO-3166 2-letter country code, 60 characters
        admin1 code       : fipscode (subject to change to iso code), see exceptions below, see file admin1Codes.txt for display names of this code; varchar(20)
        admin2 code       : code for the second administrative division, a county in the US, see file admin2Codes.txt; varchar(80) 
        admin3 code       : code for third level administrative division, varchar(20)
        admin4 code       : code for fourth level administrative division, varchar(20)
        population        : bigint (8 byte int) 
        elevation         : in meters, integer
        gtopo30           : average elevation of 30'x30' (ca 900mx900m) area in meters, integer
        timezone          : the timezone id (see file timeZone.txt)
        modification date : date of last modification in yyyy-MM-dd format
     */
    [DelimitedRecord("\t")]
    public class Geoname
    {
        [BsonId]
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
        [FieldConverter(ConverterKind.Date, "yyyy-MM-dd")]
        public DateTime? ModificationDate;
    }

    public class Location   
    {
        public decimal Long;
        public decimal Lat;
    }
}
