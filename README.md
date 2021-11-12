# ValueBlue-WebApi
ValueBlue Code Challenge Project to wrap the Open Movie Database API, Save response to MongoDb and create RESTful Api for search.

1. Estimated time to complete assessment: 4hrs

2. Research time: 1hr

3. Project implementation: 3hrs

Implementation:

A. Project setup and configuration
B. Git repo created
C. Initial commit 
E. Api key implemented
D. Swagger doc configured
F. Image(Poster) Api implemeted
G. Search Endpoint implementation completed
H. Admin Endpoints implementation completed
I. Api testing done!
J. Git push done!
K. Git pull request
L. Merge to master from dev branch done 

Project TargetFramework = netcoreapp3.1


The following endpoints were implemented:

api/Search/{title}: Retrieve OMDB info by title search
api/Admin/all: Get all movies searched by users
api/Admin/title/{title}: Get searched movie by title
api/Admin/daterange/{startDate}/{endDate}: Get searched movies by date range
api/Admin/report/{date}: Get searched movies report by date
api/Admin/delete/{title}: Delete searched movie by title


NOTE: The following configuration files and settings are essential to run the service.
Configuration file: appsettings.json

Api-Key: key = "ApiKey", Value = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9"
And should be passed in the header
AppConfigs.ApiKeyConfig.Secret : holds Api-key value. 

AppConfigs.mongo : holds values for mongodb connections
	.Ip = mongoDb installation ip address
	.Port = port
	.Db = database
	.Collections = document name 
	.User = username,
	.Secret = password

AppConfigs.CommonEndpoint hold settings/values for external services
	.BaseUrl = Open Movie Database send request uri
	.ImageUrl = Open Movie Database Poster request uri
	.Apikey = Api key for OMDB


Log files can be found in \Logs in the root directory of the application.
Directory may be change by editing the nlog.config file

