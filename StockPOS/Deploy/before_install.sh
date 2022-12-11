#!/bin/bash

# Get the old image from .env file
export OLD_IMAGE=$(grep -Po 'STOCKPOS_BACKEND_TAG=\K.*' /opt/stockpos/.env) || true

echo $OLD_IMAGE > /home/ubuntu/old_image.txt
