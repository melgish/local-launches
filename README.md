# local-launches

 [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](./LICENSE)
 [![.NET](https://github.com/melgish/local-launches/actions/workflows/dotnet.yml/badge.svg)](https://github.com/melgish/local-launches/actions/workflows/dotnet.yml)
 [![CodeQL](https://github.com/melgish/local-launches/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/melgish/local-launches/actions/workflows/github-code-scanning/codeql)
 [![codecov](https://codecov.io/github/melgish/local-launches/graph/badge.svg?token=Q7HPKX12NH)](https://codecov.io/github/melgish/local-launches)

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
