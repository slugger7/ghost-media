FROM mcr.microsoft.com/dotnet/sdk:7.0
WORKDIR /app

COPY . .

RUN dotnet restore
RUN dotnet tool install --global dotnet-ef
ENTRYPOINT ["/root/.dotnet/tools/dotnet-ef", "database", "update"]
