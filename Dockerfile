FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

ENV DATABASE_PATH "/data/Ghost.db"
RUN echo $DATABASE_PATH

COPY . .

RUN dotnet restore
RUN dotnet publish -c Release -o out

RUN mkdir /data
RUN dotnet tool install --global dotnet-ef
WORKDIR ./Ghost.Data
RUN ~/.dotnet/tools/dotnet-ef database update
RUN ls /data

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
COPY --from=build-env /data /data
RUN rm -rf /app/out
ENTRYPOINT ["dotnet", "Ghost.Web.dll"]