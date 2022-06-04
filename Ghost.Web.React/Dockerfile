FROM node:latest
WORKDIR /app
COPY package.json ./
RUN npm i
COPY . .
RUN npm run build

RUN rm -rf ./node_modules

RUN npm i -g serve

CMD ["serve", "build"]