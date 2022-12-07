#!/bin/bash

# Get the old image from .env file
export OLD_IMAGE=$(grep -Po 'STOCKPOS_BACKEND_TAG=\K.*' /opt/stockpos/.env) || true

echo `Old Image in afterInstall: $OLD_IMAGE`
