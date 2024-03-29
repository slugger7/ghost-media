# Getting started

In order to get the project running locally for development here are a few steps:

## Database

1. `cd Ghost.Data`
1. `dotnet ef database update` or `~/.dotnet/tools/dotnet-ef database update`

## API

1. `sudo apt-get update && sudo apt-get install -y ffmpeg`
1. `cd Ghost.Web`
1. `dotnet watch run --urls=http://localhost:5120`

### API in docker container

1. `docker build -t ghost-media-dev -f Dockerfile.dev .`
1.

```bash
docker run -d \
  -p 5120:5120 \
  -v $(pwd):/app \
  -v $(pwd)/assets:/media/assets \
  -e DATABASE_PATH=/app/data/Ghost.db \
  --name ghost-media-dev ghost-media-dev
```

## Web

1. `cd Ghost.Web.React`
1. `npm i`
1. `npm start`

### Web in docker container

1. `cd Ghost.Web
1. `docker build -t ghost-media-react -f Dockerfile .` same as production build
1. `npm i`
1.

```bash
docker run -d \
  -p 3000:3000 \
  -v $(pwd):/app
  -e VITE_APP_SERVER_URL=http://192.168.178.27:5120 \
  --name ghost-media-react-dev ghost-media-react
```

## Building

### Ghost Data

This container is used to update the database if there are any migrations that need to be run on the data

1. `docker image rm ghost-updater`

1. `docker build -t ghost-updater -f Ghost.Data/Dockerfile Ghost.Data`

1.

```bash
docker run --rm \
  -v $(pwd)/data:/data \
  -e DATABASE_PATH=/data/Ghost.db \
  ghost-updater
```

### Ghost API

This container uses the database that we created or updated with the Ghost Data container to run the API

1. `docker build -t ghost-media -f Dockerfile .`

1. You can mount as many volumes inside of the `media` folder in the container. You will use this folder to setup your media.
   You can also replace the `DATABASE_PATH` with whatever you need as long as it is mounted in the volume.

```bash
docker run -d \
  -p 8080:80 \
  -e DATABASE_PATH=/data/Ghost.db \
  -v $(pwd)/assets:/media/assets \
  -v $(pwd)/data:/data \
  --name ghost-media ghost-media
```

### Ghost Web React

This container is the frontend container that connects to the API

1. `docker build -t ghost-media-react -f Ghost.Web/Dockerfile Ghost.Web`

1. You can replace the `VITE_APP_SERVER_URL` with the the IP address of the running server

```bash
docker run -d \
  -p 3001:3000 \
  -e VITE_APP_SERVER_URL=http://localhost:8080 \
  --name ghost-media-react ghost-media-react
```

## Adding a library

Currently this project is in MVP state so there is no fancy library path picker.
To add a library to your application

1. Make sure the folder is mounted
1. Go to the `Library` tab and click `Add Library`
1. Pick a name and add as many paths of the library as you wish
1. Finish by clicking `Create Library`
1. You will be redirected to the `Library` tab again with your newly created library
1. Click the options on the library and select `Sync` to search for any media within the library folders. (this may take a while depending on the size of your library)
1. Navigate to home and you should see your media and images start to load and be generated.

## Adding a user

There is no functionality yet to add a user from the frontend yet soo you will have to post to an endpoint of the API in order to create a user for the time being.

```rest
POST http://localhost:5120/api/user
Content-Type: application/json

{
  "username": "someUsername",
  "password": "somePassword"
}
```

This functionality is still very basic and every time the site loads for the first time you will have to go to the settings section and select the user which you want to be "logged in" as. There are plans to expand on this in the future at the moment the focus is more on the video management side of things.
