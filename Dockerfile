FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY SiteConstructor/SiteConstructor.csproj SiteConstructor/
RUN dotnet restore SiteConstructor/SiteConstructor.csproj

COPY SiteConstructor/ SiteConstructor/
RUN dotnet publish SiteConstructor/SiteConstructor.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "SiteConstructor.dll"]