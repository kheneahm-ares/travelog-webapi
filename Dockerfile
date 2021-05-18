#build stage
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build
WORKDIR /app 

#restore our project and deps 
WORKDIR /app

COPY *.sln .
COPY ./TravelogApi/*.csproj ./TravelogApi/
COPY ./Domain/*.csproj ./Domain/
COPY ./DataAccess/*.csproj ./DataAccess/
COPY ./Persistence/*.csproj ./Persistence/
COPY ./Business/*.csproj ./Business/
COPY ./TravelogApi.Tests/*.csproj ./TravelogApi.Tests/

RUN dotnet restore

#copy rest and publish 
COPY ./TravelogApi/. ./TravelogApi/
COPY ./Domain/. ./Domain
COPY ./DataAccess/. ./DataAccess/
COPY ./Persistence/. ./Persistence/
COPY ./Business/. ./Business/
COPY ./TravelogApi.Tests/. ./TravelogApi.Tests/

RUN dotnet publish -c release -o out

#start our app stage
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet", "TravelogApi.dll" ]
