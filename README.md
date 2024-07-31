# local-launches

Periodically screen-scrapes [spaceflightnow.com](https://spaceflightnow.com) and
lists launches on the Space Coast making the list available as an HTML page.

## Building

### Via Command Line

```sh
dotnet build
dotnet run --project Launches
```

### Via Docker

In addition to the command below review ./publish.sh for creating container
with version tag.

```sh
docker compose build
docker compose up -d
```
