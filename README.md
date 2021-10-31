# Introduction 

A .NET 5 API when given a Pokemon name, returns its description or a translated description (Yoda or Shakespearean)


# Running the application

## Running without docker

Note: You may need to generate self-signed certificates to use https https://docs.microsoft.com/en-us/dotnet/core/additional-tools/self-signed-certificates-guide 

So the following steps just use http for simplicity.

1. Install .NET 5 SDK https://dotnet.microsoft.com/download/dotnet/5.0
2. Clone the repo
3. Navigate to \Pokedex.Api\src\Pokedex.Api
4. Execute dotnet run
5. You should be now able to access the swagger documentation at: http://localhost:5001/swagger/index.html

## Running with docker

1. Install Docker onto your machine
2. Clone the repo
3. Navigate to \Pokedex.Api\src\Pokedex.Api
4. Execute the following
    1. docker build -t pokedex .
    2. docker run -d -p 8080:80 --name pokedexapp pokedex
5. You should be now able to access the swagger documentation at: http://localhost:8080/swagger/index.html

#  Running the tests

1. Install .NET 5 SDK https://dotnet.microsoft.com/download/dotnet/5.0
2. Clone the repo
3. Navigate to \Pokedex.Api\test\Pokedex.Api.UnitTests
4. Execute dotnet test
5. Navigate to \Pokedex.Api\test\Pokedex.Api.IntegrationTests
6. Execute dotnet test

# Testing Approach

- For Unit Tests: Moq, FluentAssertions and AutoFixture
- For Integration Tests: FluentAssertions and WireMock