#!/bin/bash

export ENVIRONMENT=staging
export MOBILE_ENV=staging-mobile

# install docker
curl -fsSL https://get.docker.com -o get-docker.sh
sh get-docker.sh
usermod -aG docker ubuntu

# install cloudwatch agent and apply configuration from ssm parameter store
# wget https://s3.amazonaws.com/amazoncloudwatch-agent/ubuntu/arm64/latest/amazon-cloudwatch-agent.deb
# sudo dpkg -i -E ./amazon-cloudwatch-agent.deb
# sudo /opt/aws/amazon-cloudwatch-agent/bin/amazon-cloudwatch-agent-ctl -a fetch-config -m ec2 -s -c ssm:AmazonCloudWatch-agent-config-staging

# install aws cli
apt-get update
apt-get install -y unzip
curl "https://awscli.amazonaws.com/awscli-exe-linux-aarch64.zip" -o "awscliv2.zip"
unzip awscliv2.zip
./aws/install

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

# aws s3 cp --recursive s3://karzo-kms-config-files/github-ssh/ ~/.ssh/ && chmod 400 ~/.ssh/id*

# ssh-keyscan -t ed25519 github.com > ~/.ssh/known_hosts

# git clone --branch $ENVIRONMENT git@github.com:karzo-mm/karzo-kms-backend.git /home/ubuntu/src && export DIR=$_

# export WORKDIR=/opt/optimus/operation/$ENVIRONMENT
# export MOBILE_WORKDIR=/opt/optimus/operation/$MOBILE_ENV

# if [ -d "$DIR" ]; then
#     # Take action if $DIR exists. #
#     mkdir -p $WORKDIR/ && cp $DIR/deploy/compose/.env.$ENVIRONMENT $DIR/deploy/compose/docker-compose.$ENVIRONMENT.yml -t $WORKDIR/

#     mkdir -p $MOBILE_WORKDIR/ && cp $DIR/deploy/compose/.env.$MOBILE_ENV $DIR/deploy/compose/docker-compose.$MOBILE_ENV.yml -t $MOBILE_WORKDIR/
#     cp -rf $DIR/deploy/nginx $WORKDIR

#     # Download zip file for app.karzo.com
#     aws s3 cp s3://karzo-kms-config-files/kargo-app/kargo-app.zip ~/ && unzip ~/kargo-app.zip -d /opt/optimus/operation/ && rm -f ~/kargo-app.zip && chown -R ubuntu:ubuntu /opt/optimus/operation

#     # Login ECR
#     aws ecr get-login-password --region ap-southeast-1 | docker login --username AWS --password-stdin 515210838824.dkr.ecr.ap-southeast-1.amazonaws.com

#     # Recreate the containers with new image
#     docker compose -p $ENVIRONMENT -f $WORKDIR/docker-compose.$ENVIRONMENT.yml --env-file $WORKDIR/.env.$ENVIRONMENT up -d --force-recreate
#     docker compose -p $ENVIRONMENT -f $MOBILE_WORKDIR/docker-compose.$MOBILE_ENV.yml --env-file $MOBILE_WORKDIR/.env.$MOBILE_ENV up -d --force-recreate

#     # Folder for scripts
#     export SCRIPTS_DIR=/opt/optimus/operation/scripts

#     # Configure cronjobs
#     mkdir -p $SCRIPTS_DIR/log && rm -rf $SCRIPTS_DIR/*.sh && cp -rf $DIR/deploy/cron/$ENVIRONMENT/*.sh -t $SCRIPTS_DIR && chown -R ubuntu:ubuntu $SCRIPTS_DIR && chmod +x $SCRIPTS_DIR/*.sh
#     crontab -u ubuntu $DIR/deploy/cron/$ENVIRONMENT/jobs

#     # Clean Up the Stop Containers
#     docker container prune -f

#     # Clean Up the Dangling Images
#     docker image prune -f

#     # Clean Up the Old Volumes
#     docker volume prune -f

#     # Clean up the files
#     rm -r $DIR ~/.ssh/id*
# fi

if [ "$INSTANCE_ID" != "$ASSOCIATED_ID" ]
then
    # Retrieve the Elastic IP using the meta-data
    export EID=$(echo $(aws ec2 describe-addresses --output text --region $REGION --query 'Addresses[*].AllocationId' --filters Name="tag:Name",Values="$TAG") | cut --delimiter " " --fields 1)

    # Check the IP and associate with current instance
    [ ! -z "$EID" ] && aws ec2 associate-address --instance-id $INSTANCE_ID --allocation-id $EID --allow-reassociation
fi
