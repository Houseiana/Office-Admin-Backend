FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY AdminOffice.sln .
COPY src/Admin.Office.Shared/Admin.Office.Shared.csproj src/Admin.Office.Shared/
COPY src/Admin.Office.HumanResources/Admin.Office.HumanResources.csproj src/Admin.Office.HumanResources/
COPY src/Admin.Office.Recruitment/Admin.Office.Recruitment.csproj src/Admin.Office.Recruitment/
COPY src/Admin.Office.API/Admin.Office.API.csproj src/Admin.Office.API/
RUN dotnet restore

COPY . .
RUN dotnet publish src/Admin.Office.API/Admin.Office.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
EXPOSE 8080

ENTRYPOINT ["dotnet", "Admin.Office.API.dll"]
