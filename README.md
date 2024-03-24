# PageCount

lightweight page analytics api written in .NET and using SQL Lite

## Server:
### Install:
### Docker:
pull the docker image from [here](https://hub.docker.com/r/belowh/pagecount) using

``` Console
docker pull belowh/pagecount:v0_1_1
```
and start the container like this
``` Console
docker run -d -p 80:8080 -e usernameHash=[Put the SHA256 hash of your username here] -e passwordHash=[Put the SHA256 hash of your password here] -e rateLimit=100 belowh/pagecount:v0_1_1
```
For the `usernameHash` and `passwordHash` fields,
please input the respective hashes of your chosen username and password.
These credentials will also be utilized for API Basic authentication.
Ensure to select the appropriate port for your application.
The `rateLimit` is per minute per ip

### Manual 
Of course, you can build the Docker image yourself by using the included Dockerfile.
Or just build and deploy the application yourself altogether.

### Authentication:

This api uses Basic auth.   
Provide the password and username as a Base64 encoded string in the authentication header:
``` HTTP
Authorization: Basic dXNlcm5hbWU6cGFzc3dvcmQ=
```
Note: here the username and password should not be a SHA256 hash.

### Endpoints:



#### GET /health 
```http
GET /health
```
Returns status and Version
```http
{
    "version": "0.1.0.0",
    "databaseConnection": "CONNECTED"
}
```
#### POST /count
```http
POST /count
Body:
{
    "userId" : "1234",
    "amount" : 1,
    "primaryCountType" : "test",
    "secondaryCountType" : "test secondary"
    "timestamp" : "2024-03-22T15:08:25.873Z"
}
```
Returns 200 OK if everything worked. Or a corresponding error if something goes wrong

#### GET /result
```http
GET /result?primaryIdentifier=test&secondaryIdentifier=testSecond
```
returns an array of `counts` like this:
```http
[
    {
        "countId": "6210884d-6a63-4746-b49a-ee0d8315047d",
        "userId": "45afe704-63da-41d1-89cb-93687cf92683",
        "amount": 1,
        "primaryCountType": "test",
        "secondaryCountType": "testSecond",
        "timestamp": "2024-03-22T15:11:07.891742"
    },
    {
        "countId": "6cd1477a-7705-49d5-8327-452b9cd49b66",
        "userId": "edbdc0ea-5852-44c3-a027-3eea466abcc0",
        "amount": 1,
        "primaryCountType": "test",
        "secondaryCountType": "testSecond",
        "timestamp": "2024-03-22T15:12:48.7185823"
    }
]
```

## Nuget Client Package:

### Install:

### Usage: 
In your `Startup.cs`, `Program.cs`
or wherever your `HostApplicationBuilder` or `WebApplicationBuilder` resides add PageCount to your Services
and pass in the options 
```C#
PageCountOptions options = new PageCountOptions()
{
    HostUri = new Uri("https://your.uri"),
    AnalyticsEnabledAsDefault = true,
    Password = "[whatever your password is]",
    Username = "[whatever your password is]",
    ThrowOnError = true
};

builder.Services.AddPageCount(options);
```
Next in the class where you want to register a count:
first inject the `IPageCountService`
```C#
private readonly IPageCountService _pageCountService;
    
public TestService(IPageCountService pageCountService)
{
    _pageCountService = pageCountService;
}
```
If you want to register a count aka call POST /count
```C#
_pageCountService.SubmitCount("test");
```

with `_pageCountService.DisableAnalytics();` you can disable submit calls
and re-enable them again with `_pageCountService.EnableAnalytics();`

