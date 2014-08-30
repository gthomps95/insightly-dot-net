Insightly library for .Net
==========================

This library provides user friendly access to the version 2.1 REST API
for Insightly. The library provides several services, including:
   
* HTTPS request generation
* Response parsing
   
Dependencies
------------

This library depends on the Json.NET library
to serialize data sent to the server
and deserialize the response.
If you are building the Insightly library for .NET,
the project file expects the Json.NET to be installed
under the Dependencies sub-directory.
You may install Json.NET by running the following command
from the project root:

```
nuget.exe install Newtonsoft.Json -O Dependencies
```

or you can change the assembly reference.

If you are using mono, the following has been tested to work
with mono 2.10.8 on Fedora 20 (note the `--runtime=v4.0` option):

```
mono --runtime=v4.0 NuGet.exe install Newtonsoft.Json -O Dependencies
```

USAGE:
------
   
Simply include the Insightly-SDK.dll assembly in your project file.
You can then run a simple test suite with the following code:

```C#   
using System;

using InsightlySDK;

namespace InsightlyTest{
	class TestMain{
		public static void Main (string[] args){
			string api_key = args[0];
			Insightly i = new Insightly(api_key);
			i.Test();
		}
	}
}
```
   
This will run an automatic test suite against your Insightly account.
If the methods you need all pass, you're good to go!

For your convenience, the InsightlyTest project included in this repository
will run the above code.
Simply build the project, then run InsightlyTest.exe passing your api key
as a command-line parameter.
   
If you are working with very large recordsets,
you should use ODATA filters to access data in smaller chunks.
This is a good idea in general to minimize server response times.
   
BASIC USE PATTERNS:
-------------------
   
CREATE/UPDATE ACTIONS
*********************
   
These methods expect a dictionary containing valid data fields for the object.
They will return a dictionary containing the object
as stored on the server (if successful)
or raise an exception if the create/update request fails.
You indicate that you want to create a new item
by setting the record id to 0 or omitting it.
   
SEARCH ACTIONS
**************
   
These methods return a list of dictionaries containing the matching items. For example to request a list of all contacts, you call:

```C#
Insightly i = Insightly("your API key")
JArray contacts = i.GetContacts()
```
   
SEARCH ACTIONS USING ODATA
**************************
   
Search methods recognize top, skip, orderby and filters parameters,
which you can use to page, order and filter recordsets.

```   
// returns the top 200 contacts
contacts = i.GetContacts(top: 200);

// returns the top 200 contacts, with first name descending order
contacts = i.GetContacts(orderby: "FIRST_NAME desc", top: 200);

// returns 200 records, after skipping the first 200 records
contacts = i.GetContacts(top: 200, skip: 200);

// get contacts where FIRST_NAME='Brian'
List<string> filters;
filters.Add("FIRSTNAME='Brian'");
contacts = i.GetContacts(filters: filters);
```
   
IMPORTANT NOTE: when using OData filters,
be sure to include escaped quotes around the search term,
otherwise you will get a 400 (bad request) error
   
These methods will raise an exception if the lookup fails,
or return a (possibly empty) list of objects (as a JArray) if successful.
   
READ ACTIONS (SINGLE ITEM)
**************************
   
These methods will return a single JObject
containing the requested item's details.

For example:

```C#
JObject contact = i.getContact(123456);
```
   
DELETE ACTIONS
**************
   
These methods will return if successful, or raise an exception.
e.g. `i.deleteContact(123456)`
   
IMAGE AND FILE ATTACHMENT MANAGEMENT
************************************
   
The API calls to manage images and file attachments
have not yet been implemented by this library.
However you can access these directly via our REST API
   
ISSUES TO BE AWARE OF
*********************
   
This library makes it easy to integrate with Insightly,
and by automating HTTPS requests for you,
eliminates the most common causes of user issues.
That said, the service is picky about rejecting requests
that do not have required fields, or have invalid field values
(such as an invalid USER_ID).
When this happens, you'll get a 400 (bad request) error.
Your best bet at this point is to consult the API documentation
and look at the required request data.
   
If you are working with large recordsets,
we strongly recommend that you use ODATA functions,
such as top and skip to page through recordsets
rather than trying to fetch entire recordsets in one go.
This not only improves client/server communication,
but also minimizes memory requirements on your end.
    
TROUBLESHOOTING TIPS
********************
    
One of the main issues API users run into during write/update operations
is a 400 error (bad request) due to missing required fields.
If you are unclear about what the server is expecting,
a good way to troubleshoot this is to do the following:
    
* Using the web interface, create the object in question
  (contact, project, team, etc), and add sample data and child elements to it
* Use the corresponding getNNNN() method
  to retrieve this object via the web API
* Inspect the object's contents and structure
    
Read operations via the API are generally quite straightforward,
so if you get struck on a write operation, this is a good workaround,
as you are probably just missing a required field
or using an invalid element ID when referring
to something such as a link to a contact.
