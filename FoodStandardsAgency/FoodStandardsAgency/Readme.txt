
This solution has been created in .Net Core 2 from Visual Studio 2017 Professional for Mac

Solution Structure
==================

FoodStandardsAgency     - MVC Website
Rating                  - Ratings Calculator
ServiceClient           - Client used to communicate with Food Standards Agency webservice
ServiceClient.Contracts - Programmatic Interfaces implemented in the ServiceClient project.
ServiceClient.Models    - Contains the model classes the service client deserialized into.
Tests                   - MSTest based unit tests

AutoMapper is used to map the ServiceClient models to the website view models

Solution Dependencies
=====================
The solution has dependencies on

AutoMapper
AspNetCore
Microsoft.Extensions.Logging
Moq
MSTest

all dependencies are controlled via Nuget.

Notes
=====
Only the ratings detailed in the technical test have been consumed, alhought the specification
calls for GB 1-5 & Exempt ratings to be used and Pass / Needs Improvement in Scotland. It should be
noted that other ratings exist and are returned from the service. As in line with the specification, 
only those ratings mentioned in the specification have been processed.

Improvements
============
Get authority cache settings from a config file, eg cache expiration time span etc.

Remove business logic that has bled into the user interface (AuthorityRatingPartial.cshtml) 
to set the desired css class so that the result is formatted correctly in line with its related image

In the RatingCalculator the ratings are held in a List<AuthorityRatings> if this proves not to be satifactory 
in terms of performance then a dictionary could be used.

Caching
=======
Cache is a simple in memory cache.
Only the list of authorites is cached, at present each set of ratings results is not cached.
If it was determined that these ratings are updated on a non-regular basis that the rating
results could also be cached.




