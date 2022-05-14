# Building

# Ghost Data

`docker build -t ghost-updater -f Ghost.Data/Dockerfile Ghost.Data`

`docker run -d -v $(pwd)/data:/data -e DATABASE_PATH=/data/Ghost.db --name ghost-updater ghost-updater`

# Ghost Web

`docker build -t ghost-media -f Dockerfile .`

`docker run -d -p 8080:80 -e DATABASE_PATH=/data/Ghost.db -v $(pwd)/assets:/media/assets -v $(pwd)/data:/data --name ghost-media ghost-media`

# Ghost Web React

`docker build -t ghost-media-react -f Ghost.Web.React/Dockerfile Ghost.Web.React`

`docker run -d -p 3001:3000 --name ghost-media-web ghost-media-react`
