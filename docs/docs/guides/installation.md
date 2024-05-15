# Installation

## Prerequisites

Before you begin, ensure you have Docker installed on your system. You can download and install Docker from the [official Docker website](https://www.docker.com/get-started).
Currently only docker is supported. Please visit the [Docker Hub](https://hub.docker.com/u/restackproject) page to view all the tags.

## Option 1: Use docker compose

The easiest way to get the application running is to use a docker_compose.yml file. You can copy the code below and make some adjustments to your needs.

```yaml
services:
  restack-api:
    image: restackproject/restack-api:latest
    container_name: restack-api
    restart: unless-stopped
    hostname: restack-api
    ports:
      - 5000:8080
    environment:
      - ConnectionStrings__ReStack=Host=restack-db;Database=restack;Username=restack_user;Password=CHANGE_ME
    depends_on:
      - restack-db
    volumes:
      - restack-api-data:/restack/data

  restack-web:
    image: restackproject/restack-web:latest
    container_name: restack-web
    restart: unless-stopped
    hostname: restack-web
    environment:
      - WebSettings__ApiUrl=http://restack-api:8080
    ports:
      - 5010:8080
    depends_on:
      - restack-api
  
  restack-db:
    container_name: restack-db
    image: postgres:16-alpine
    hostname: restack-db
    restart: always
    ports:
      - 5432:5432
    volumes:
      - restack-db-data:/var/lib/postgresql/data
    environment:
      - POSTGRES_PASSWORD=CHANGE_ME
      - POSTGRES_USER=restack_user
      - POSTGRES_DB=restack

volumes:
  restack-db-data:
  restack-api-data:
```

## Options 2:  Use docker run

It is also possible to use docker run commands to get the application installed. You'll need to run 3 commands:

### Database
```yaml
docker run -d \
    --name restack-db \
    --restart always \
    --hostname restack-db \
    -p 5432:5432 \
    -v restack-db-data:/var/lib/postgresql/data \
    -e POSTGRES_PASSWORD=CHANGE_ME \
    -e POSTGRES_USER=restack_user \
    -e POSTGRES_DB=restack \
    postgres:16-alpine
```

### Api
```yaml
docker run -d \
    --name restack-api \
    --restart unless-stopped \
    --hostname restack-api \
    -p 5000:8080 \
    -e ConnectionStrings__ReStack="Host=restack-db;Database=restack;Username=restack_user;Password=CHANGE_ME" \
    --depends-on restack-db \
    -v restack-api-data:/restack/data \
    restackproject/restack-api:latest
```

### Web
```yaml
docker run -d \
    --name restack-web \
    --restart unless-stopped \
    --hostname restack-web \
    -e WebSettings__ApiUrl=http://restack-api:8080 \
    -p 5010:8080 \
    --depends-on restack-api \
    restackproject/restack-web:latest
```

## Environment variables

There are a few variables available to configure.

### Web

| Variable              | Explanation    |
|-----------------------|----------------------------------|
| WebSettings__ApiUrl   | Url of the API container. <br>Default: http://restack-api.  |

### API

| Variable                      | Explanation    |
|-------------------------------|------------|
| ConnectionStrings__ReStack    | Connection string for the ReStack database. |
| ApiSettings__Storage          | Folder that stores all the data of the application. Folder constists of: keys, stacks, components & working directories. <br>Default: /restack/data|
| ApiSettings__SshKey_Default   | Default SSH key name. <br>Default: id_rsa. |
| ApiSettings__JobQueue         | Max items for in queue. <br>Default: 100. |
| ApiSettings__JobWorkers       | Number of concurrent jobs. <br>Default: 2. |


## Access the Application

Once the Docker containers are up and running, you can access the ReStack application in your web browser:

- [http://localhost:5010](http://localhost:5010)

## Additional Notes

If you encounter any issues during the installation process, refer to the project's documentation. If the documentation is unclear or not complete, please [log an issue](https://github.com/restack-project/restack/issues) and let us know how to improve the installation process or guide(s).