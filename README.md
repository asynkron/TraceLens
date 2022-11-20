# TraceView

> **Warning**<br>
> TraceView is not opensource<br>
> TraceView is not free for commercial usage

TraceView is free for personal use.

## Deployment

### Docker Compose 

#### Linux AMD64 Distro: 
[docker-compose.yml](amd64/docker-compose.yml)

#### Linux ARM64 Distro:
[docker-compose.yml](arm64/docker-compose.yml)

### Docker

>**Note**
>Requires Redis running

#### Linux AMD64 Distro: 
```
docker run -p 5001:5001 -p 4317:4317 docker.io/rogeralsing/traceview:amd64
```

#### Linux ARM64 Distro: 
```
docker run -p 5001:5001 -p 4317:4317 docker.io/rogeralsing/traceview:arm64
```
