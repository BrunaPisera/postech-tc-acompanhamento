FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY . .

RUN dotnet restore Acompanhamento.sln

RUN dotnet publish Acompanhamento.API/Acompanhamento.API.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build ./app/out .
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "Acompanhamento.API.dll"]