#!/bin/bash

export TOKEN=`/usr/bin/curl -X PUT "http://169.254.169.254/latest/api/token" -H "X-aws-ec2-metadata-token-ttl-seconds: 21600"`

export INSTANCE_ID=`/usr/bin/curl -H "X-aws-ec2-metadata-token: $TOKEN" -s http://169.254.169.254/latest/meta-data/instance-id`

export REGION=`/usr/bin/curl -H "X-aws-ec2-metadata-token: $TOKEN" -s http://169.254.169.254/latest/meta-data/placement/region`

export TAG=`/usr/bin/curl -H "X-aws-ec2-metadata-token: $TOKEN" -s http://169.254.169.254/latest/meta-data/tags/instance/Name`

export ASSOCIATED_ID=`aws ec2 describe-addresses --output text --region $REGION --query 'Addresses[*].InstanceId' --filters Name="tag:Name",Values="$TAG"`

source /opt/stockpos/.env
# Login ECR
aws ecr get-login-password --region $REGION | docker login --username AWS --password-stdin $ECR

# Get the parameters
export DB_SERVER_URL=`aws ssm get-parameter --name mysql-server-url --with-decryption --query 'Parameter.Value' --output text`
export DB_PASSWORD=`aws ssm get-parameter --name mysql-dbpassword --with-decryption --query 'Parameter.Value' --output text`

# Pull the new image
docker compose -p $DEPLOYMENT_GROUP_NAME -f /opt/stockpos/docker-compose.yml pull

# Recreate the containers with new image
docker compose -p $DEPLOYMENT_GROUP_NAME -f /opt/stockpos/docker-compose.yml up -d --force-recreate

# Clean Up the Stop Containers
docker container prune -f

# Try to remove the Old Image

pwd

export OLD_IMAGE=`cat old_image`

echo Old Image applicationStart: $OLD_IMAGE

[ ! -z "$OLD_IMAGE" ] && docker rmi $ECR/$REPO:$OLD_IMAGE || true

# Clean Up the Dangling Images
docker image prune -f

# Clean Up the Old Volumes
docker volume prune -f

rm -f old_image

if [ "$INSTANCE_ID" != "$ASSOCIATED_ID" ]
then
    # Retrieve the Elastic IP using the meta-data
    export EID=$(echo $(aws ec2 describe-addresses --output text --region $REGION --query 'Addresses[*].AllocationId' --filters Name="tag:Name",Values="$TAG") | cut --delimiter " " --fields 1)

    # Check the IP and associate with current instance
    [ ! -z "$EID" ] && aws ec2 associate-address --instance-id $INSTANCE_ID --allocation-id $EID --allow-reassociation
fi
