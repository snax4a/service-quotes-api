build:
	dotnet build
clean:
	dotnet clean
restore:
	dotnet restore
watch:
	dotnet watch --project src/ServiceQuotes.Api run
start:
	dotnet run --project src/ServiceQuotes.Api
remove-migration:
	dotnet ef migrations remove --startup-project ./src/ServiceQuotes.Api --project ./src/ServiceQuotes.Infrastructure
update-database:
	dotnet ef database update --startup-project ./src/ServiceQuotes.Api --project ./src/ServiceQuotes.Infrastructure
