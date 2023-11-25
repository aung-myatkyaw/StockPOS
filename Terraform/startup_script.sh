#!/bin/bash

export CONFIG_BUCKET="{{Config-Bucket-Name}}"

# install docker
curl -fsSL https://get.docker.com -o get-docker.sh
sh get-docker.sh
usermod -aG docker ubuntu

rm -rf ./get-docker.sh

# install cloudwatch agent and apply configuration from ssm parameter store
wget https://s3.amazonaws.com/amazoncloudwatch-agent/ubuntu/$(dpkg --print-architecture)/latest/amazon-cloudwatch-agent.deb

sudo dpkg -i -E ./amazon-cloudwatch-agent.deb
sudo /opt/aws/amazon-cloudwatch-agent/bin/amazon-cloudwatch-agent-ctl -a fetch-config -m ec2 -s -c ssm:AmazonCloudWatch-agent-config

rm -rf ./amazon-cloudwatch-agent.deb

# install aws cli
apt-get update
apt-get install -y unzip
curl "https://awscli.amazonaws.com/awscli-exe-linux-$(uname -m).zip" -o "awscliv2.zip"
unzip awscliv2.zip
./aws/install

rm -rf ./aws ./awscliv2.zip

export TOKEN=`/usr/bin/curl -X PUT "http://169.254.169.254/latest/api/token" -H "X-aws-ec2-metadata-token-ttl-seconds: 21600"`

export INSTANCE_ID=`/usr/bin/curl -H "X-aws-ec2-metadata-token: $TOKEN" -s http://169.254.169.254/latest/meta-data/instance-id`

export REGION=`/usr/bin/curl -H "X-aws-ec2-metadata-token: $TOKEN" -s http://169.254.169.254/latest/meta-data/placement/region`

export TAG=`/usr/bin/curl -H "X-aws-ec2-metadata-token: $TOKEN" -s http://169.254.169.254/latest/meta-data/tags/instance/Name`

export ASSOCIATED_ID=`aws ec2 describe-addresses --output text --region $REGION --query 'Addresses[*].InstanceId' --filters Name="tag:Name",Values="$TAG"`

# install codedeploy agent
apt-get install -y ruby-full
apt-get install -y wget
wget https://aws-codedeploy-$REGION.s3.$REGION.amazonaws.com/latest/install
chmod +x ./install
./install auto

aws s3 cp --recursive --region ap-southeast-1 s3://$CONFIG_BUCKET/github-ssh/ ~/.ssh/ && chmod 400 ~/.ssh/id*

ssh-keyscan -t ed25519 github.com > ~/.ssh/known_hosts

git clone --branch master git@github.com:yinko2/StockPOS.git $HOME/src && export DIR=$_

export WORKDIR=/opt/stockpos

if [ -d "$DIR" ]; then
    # Take action if $DIR exists. #
    mkdir -p $WORKDIR/ && cp $DIR/StockPOS/Deploy/.env $DIR/StockPOS/Deploy/docker-compose.yml -t $WORKDIR/

    # source $WORKDIR/.env

    # Get the parameters
    export DB_SERVER_URL=`aws ssm get-parameter --name mysql-server-url --with-decryption --query 'Parameter.Value' --output text`
    export DB_USERNAME=`aws ssm get-parameter --name mysql-dbusername --with-decryption --query 'Parameter.Value' --output text`
    export DB_PASSWORD=`aws ssm get-parameter --name mysql-dbpassword --with-decryption --query 'Parameter.Value' --output text`
    export REPO=`aws ssm get-parameter --name stockpos-ecr-url --with-decryption --query 'Parameter.Value' --output text`

    # Login ECR
    aws ecr get-login-password --region ap-southeast-1 | docker login --username AWS --password-stdin $REPO

    sed -i "/^REPO=/c REPO=$REPO" $WORKDIR/.env

    # Recreate the containers with new image
    docker compose -p production -f $WORKDIR/docker-compose.yml --env-file $WORKDIR/.env up -d --force-recreate

    # Clean Up the Stop Containers
    docker container prune -f

    # Clean Up the Dangling Images
    docker image prune -f

    # Clean Up the Old Volumes
    docker volume prune -f

    # Clean up the files
    rm -r $DIR ~/.ssh/id*
fi

if [ "$INSTANCE_ID" != "$ASSOCIATED_ID" ]
then
    # Retrieve the Elastic IP using the meta-data
    export EID=$(echo $(aws ec2 describe-addresses --output text --region $REGION --query 'Addresses[*].AllocationId' --filters Name="tag:Name",Values="$TAG") | cut --delimiter " " --fields 1)

    # Check the IP and associate with current instance
    [ ! -z "$EID" ] && aws ec2 associate-address --instance-id $INSTANCE_ID --allocation-id $EID --allow-reassociation
fi
