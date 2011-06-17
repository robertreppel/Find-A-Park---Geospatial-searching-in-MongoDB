## This sample application was used as the starting point for the "NServiceBus: From Batch Processing to Asynchronous Messaging, By Example" dojo at http://prairiedevcon.com/home

###Abstract:

    "This session is part design exercise, part code walkthrough: The example is a system which 
    is supposed to send emails and other types of notifications when certain events occur. The initial 
    proposition is to implement it using batch processing: A nightly job queries the database for the
    appropriate events and who may be interested in them and sends any notifications necessary. We'll 
    work from there to derive a design based on event sourcing and NserviceBus, examining pros & cons
    and design trade-offs along the way. The emphasis will be on testability and how the various
    decisions will impact the effort required to maintain and manage the system in the future. Time 
    permitting, we'll take a look at NserviceBus tuning and scaling."

**For the partial refactoring towards a publish-subscribe message based system, see the 'eventdriven2' branch of this project.**

## Find A Park Near You

* Written during the ALT.NET Seattle Conference 2011 http://altnet2011.heroku.com/. Wanted to try out some MongoDB. 

* Finds parks in the US located within 100km of a given city.

* Has two parts: 

	The FindAPark ASP.NET application allows searching for parks near US cities. 
	
	The GeoInfoImport command line utility loads geo data from a tab-delimited file into MongoDB. 
	(Usage: GeoInfoImport import <filename>)

* Uses data provided by http://www.geonames.org/.

* Tested with the US.txt datafile from http://download.geonames.org/export/dump/. A file with world-wide Geonames data countries.zip) is available at the site. I haven't tried it, but theoretically the thing should work as a world-wide park finder. 

* Requires a default MongoDB install on localhost ( http://www.mongodb.org/downloads ), with a geospatial index (http://www.mongodb.org/display/DOCS/Geospatial+Indexing) on "Location":

       db.parks.ensureIndex( { Location : "2d" } )

  You might also want to add some indexes on the "cities" collection to speed up the autocomplete in the FindAPark search field.

