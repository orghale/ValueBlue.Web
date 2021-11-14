# ValueBlue-WebApi
ValueBlue Code Challenge Project to wrap the Open Movie Database API, Save response to MongoDb and create RESTful Api for search.
============================================================================
============================================================================

1. Estimated time to complete assessment: 5hrs

2. Research time: 1hr

3. Project implementation: 3hrs

4. An additional 30 mins to implement: movie poster(image) search for admin was implemented as an additional feature. with an additional 30 mins development time
4. An additional 30 mins to implement: searched movies report by title for admin was implemented as an additional feature. with an additional 30 mins development time
============================================================================

Implementation:

A. Project setup and configuration
B. Git repo created
C. Initial commit 
E. Api key implemented
D. Swagger doc configured
F. Image(Poster) fetch and saved to db implemeted - added feature
G. Search Endpoint implementation completed
H. Admin Endpoints implementation completed
I. Additional enpoint to get movie poster image (base64 encoded string) implemented - added feature
J. Api testing done!
K. Git push done!
L. Git pull request
M. Merge to master from dev branch done 
============================================================================

Project TargetFramework = netcoreapp3.1
============================================================================


The following endpoints were implemented:

1.	api/Search/GetByTitle/{title}: Retrieve OMDB info by title search
2.	api/Search/GetMoviePoster/{title}: Get movie poster (image) by title - addition
3.	api/Admin/GetAll: Get all movies searched by users
4.	api/Admin/GetByTitle/{title}: Get searched movie by title
5.	api/Admin/GetByDateRange/{startDate}/{endDate}: Get searched movies by date range
6.	api/Admin/Report/GetByDate/{date}: Get searched movies report by date using string format "dd-MM-yyyy"
7.	api/Admin/Report/GetMostRequestByTitle/{title}: Get searched movies report by title - additiom
8.	api/Admin/DeleteMovie/{title}: Delete searched movie by title
============================================================================


NOTE: The following configuration files and settings are essential to run the service.
Configuration file: appsettings.json

Api-Key: key = "ApiKey", Value = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9"
And should be passed in the header
AppConfigs.ApiKeyConfig.Secret : holds Api-key value. 
============================================================================


AppConfigs.mongo : holds values for mongodb connections
	.Ip = mongoDb installation ip address
	.Port = port
	.Db = database
	.Collections = document name 
	.User = username,
	.Secret = password
============================================================================


AppConfigs.CommonEndpoint hold settings/values for external services
	.BaseUrl = Open Movie Database send request uri
	.ImageUrl = Open Movie Database Poster request uri
	.Apikey = Api key for OMDB
============================================================================

Log files can be found in \Logs in the root directory of the application.
Directory may be change by editing the nlog.config file

Unit Test was not implemented due to time constraint
============================================================================