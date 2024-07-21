#!/bin/bash

export $(grep -v '^#' .env | xargs)

PROJECT_FILE="Launches/Launches.csproj"
VERSION=$(grep -oPm1 "(?<=Version>)[^<]+" "${PROJECT_FILE}" | xargs)
if [ -z "${VERSION}" ]; then
  echo "Unable to determine application version."
  exit 1
fi

LATEST_NAME="local-launches:latest"
IMAGE_NAME="local-launches:${VERSION}"

echo "Build ${IMAGE_NAME}"
docker build -t "${LATEST_NAME}" -f "Launches/Dockerfile" "Launches"

if [ -z "${APP_REGISTRY}" ]; then
  echo "Registry has not been set. Image will not be published."
else
  docker tag "${LATEST_NAME}" "${APP_REGISTRY}/${IMAGE_NAME}"
  docker tag "${LATEST_NAME}" "${APP_REGISTRY}/${LATEST_NAME}"
  docker push "${APP_REGISTRY}/${IMAGE_NAME}"
  docker push "${APP_REGISTRY}/${LATEST_NAME}"
fi
