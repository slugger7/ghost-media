FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY . .

RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
RUN rm -rf /app/out
RUN apt-get update && apt-get install -y ffmpeg
ENTRYPOINT ["dotnet", "Ghost.Web.dll", "--urls=http://0.0.0.0:5120"]