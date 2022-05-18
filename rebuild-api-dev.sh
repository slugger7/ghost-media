#!/bin/bash
echo "Stopping ghost-media-dev"
docker stop ghost-media-dev
echo "Removing ghost-media-dev"
docker container rm ghost-media-dev
echo "Building ghost-media-dev"
docker build -t ghost-media-dev -f Dockerfile.dev .
echo "Creating and running ghosts-media-dev container"
docker run -d \
  -p 5120:5120 \
  -v $(pwd):/app \
  -v $(pwd)/assets:/media/assets \
  -e DATABASE_PATH=/app/data/Ghost.db \
  --name ghost-media-dev ghost-media-dev
docker logs -f ghost-media-dev