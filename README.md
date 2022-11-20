# TraceView

TLDR; What is TraceView?
1. a logging and tracing visualizer with developer focus instead of SRE/DevOps. see the right data instead of "a lot" of data.
2. an OpenTelemetry Collector

![](images/traceview.png)

> **Note**<br>
> TraceView is **NOT** opensource<br>
> TraceView is **NOT** free for commercial usage<br>
> TraceView **IS** free for personal use.



## Deployment

### Docker Compose 

#### Linux AMD64 Distro: 
* [docker-compose.yml](amd64/docker-compose.yml)

#### Linux ARM64 Distro:
* [docker-compose.yml](arm64/docker-compose.yml)

### Docker

>**Note**<br>
>Requires access to Redis

#### Linux AMD64 Distro: 
```
docker run -p 5001:5001 -p 4317:4317 docker.io/rogeralsing/traceview:amd64
```

#### Linux ARM64 Distro: 
```
docker run -p 5001:5001 -p 4317:4317 docker.io/rogeralsing/traceview:arm64
```

#### Configuration

TraceView uses the following configuration block to access Redis

```json
"Redis": {
    "Server": "host.docker.internal",
    "Port": 6379,
    "Ssl": false,
    "CertificatePath": "",
    "UseCaCertificate": false
}
```

These settings can be overridden using environment variables like so:

```bash
docker run -p 5001:5001 -p 4317:4317 --env Redis__Server=RedisIp docker.io/rogeralsing/traceview:amd64
```



## Data Privacy

TraceView does not share data, all data is stored in your own Redis instance.

**TraceView does however use PlantUML for rendering diagrams**. all data required to render the diagrams will be sent to a 3rd party PlantUML Server.
