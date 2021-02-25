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
