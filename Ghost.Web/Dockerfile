FROM node:20.18.0-alpine
WORKDIR /app
COPY package.json ./
COPY package-lock.json ./
RUN npm i
COPY . .

CMD ["npm", "run", "host"]