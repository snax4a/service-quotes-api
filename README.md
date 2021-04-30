# service-quotes-api
A `.NET 5.0` WebApi project. Repositories, Swagger, Mapper, Serilog and more implemented. 

![Build, test and CodeQL](https://github.com/snax4a/service-quotes-api/workflows/CodeQL/badge.svg)

# How to run
- Clone/download to your local workplace.
- Download the latest .NET SDK and Visual Studio/Code.

## Standalone
1. You may need a running instance of PostgreSQL, with appropriate migrations initialized.
	- If you want, you can change the DatabaseExtension to use UseInMemoryDatabase, instead of PostgreSQL.
2. Go to the src/ServiceQuotes.Api folder and run ``dotnet run``, or, in visual studio set the api project as startup and run as console or docker (not IIS).
3. Visit http://localhost:5000/api-docs or https://localhost:5001/api-docs to access the application's swagger.

## Docker
1. Run ``docker-compose up -d`` in the root directory, or, in visual studio, set the docker-compose project as startup and run. This should start the application and DB.
 - 1. For docker-compose, you should run this command on the root folder: ``dotnet dev-certs https --trust -ep https/servicequotes.pfx -p yourpassword``
		Replace "yourpassword" with something else in this command and the docker-compose.override.yml file.
This creates the https certificate.
2. Visit http://localhost:5000/api-docs or https://localhost:5001/api-docs to access the application's swagger.

## Running tests
In the root folder, run ``dotnet test``. This command will try to find all test projects associated with the sln file.

# This project contains:
- SwaggerUI
- EntityFramework
- AutoMapper
- Generic repository (to easily bootstrap a CRUD repository)
- Serilog with request logging and easily configurable sinks
- .Net Dependency Injection
- Resource filtering
- Response compression
- CI (Github Actions)
- Unit tests
- Integration tests
- Container support with [docker](src/ServiceQuotes.Api/dockerfile) and [docker-compose](docker-compose.yml)


# Project Structure
1. Services
	- This folder stores your apis and any project that sends data to your users.
	1. ServiceQuotes.Api
		- This is the main api project. Here are all the controllers and initialization for the api that will be used.
	2. docker-compose
		- This project exists to allow you to run docker-compose with Visual Studio. It contains a reference to the docker-compose file and will build all the projects dependencies and run it.
2. Application
	-  This folder stores all data transformations between your api and your domain layer. It also contains your business logic.
3. Domain
	- This folder contains common interfaces like UoW interface.
	1. ServiceQuotes.Domain.Entities
		- This folder contains base entity, business models, enums.
	2. ServiceQuotes.Domain.Repositories
		- This folder contains repositories interfaces.
4. Infra
	- This folder contains all data access repositories, database contexts, anything that reaches for outside data.
	1. ServiceQuotes.Infrastructure
		- This project contains the dbcontext, an generic implementation of repository pattern and a Account(domain class) repository.

# Migrations
1. To add migration on this project, run the following command on the root folder:
	- ``dotnet ef migrations add InitialCreate --startup-project ./src/ServiceQuotes.Api --project ./src/ServiceQuotes.Infrastructure``

    This command will set the entrypoint for the migration (the responsible to selecting the dbprovider { sqlserver, mysql, etc } and the connection string) and the project itself will be the infrastructure, which is where the dbcontext is.

2. To update database run the following command on the root folder:
    - ``dotnet ef database update --startup-project ./src/ServiceQuotes.Api --project ./src/ServiceQuotes.Infrastructure``
    
    Note that if you are running database and api in docker containers and your connection string in appsettings.json points to the internal docker network host you will have to pass connection string as a parameter which points to the exposed database port on your localhost for example:
    - ``dotnet ef database update --startup-project ./src/ServiceQuotes.Api --project ./src/ServiceQuotes.Infrastructure --connection "Server=127.0.0.1;Database=master;User=sa;Password=DevDbPassword123"``
