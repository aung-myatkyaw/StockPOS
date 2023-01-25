# Launch Template for Spot instance
resource "aws_launch_template" "stockpos_mumbai_template" {
  provider = aws.mumbai
  name     = "StockPOS-mumbai-Launch-Template"
  iam_instance_profile {
    arn = aws_iam_instance_profile.staging_ec2_instance_profile.arn
  }

  key_name = aws_key_pair.ec2_key.key_name

  ebs_optimized = true

  block_device_mappings {
    device_name = "/dev/sda1"

    ebs {
      delete_on_termination = true
      volume_size           = 10
      volume_type           = "gp3"
    }
  }

  description = "Spot Instance for mumbai environment"

  credit_specification {
    cpu_credits = "standard"
  }

  metadata_options {
    http_endpoint          = "enabled"
    http_tokens            = "required"
    instance_metadata_tags = "enabled"
  }

  monitoring {
    enabled = false
  }

  network_interfaces {
    associate_public_ip_address = true
    security_groups = [
      aws_security_group.stockpos_mumbai_sg.id
    ]
  }

  update_default_version = true
  image_id               = data.aws_ssm_parameter.ubuntu_image.value
  # user_data              = filebase64("startup_script.sh")
  user_data = base64encode(replace(file("startup_script.sh"), "{{Config-Bucket-Name}}", var.configs_bucket_name))

  tag_specifications {
    resource_type = "instance"
    tags = {
      "Name" = var.server_tag_value
    }
  }
  tag_specifications {
    resource_type = "volume"
    tags = {
      "Name" = var.server_tag_value
    }
  }
}

resource "aws_key_pair" "ec2_key" {
  provider   = aws.mumbai
  key_name   = var.staging_ec2_key_name
  public_key = tls_private_key.rsa.public_key_openssh
}

resource "tls_private_key" "rsa" {
  algorithm = "RSA"
  rsa_bits  = 4096
}

resource "local_sensitive_file" "private_key" {
  content         = tls_private_key.rsa.private_key_pem
  file_permission = "0400"
  filename        = "${var.staging_ec2_key_name}.pem"
}

resource "aws_s3_object" "stockpos_mumbai_staging_ssh_private_key" {
  key                    = "ec2-ssh/${var.staging_ec2_key_name}.pem"
  content                = tls_private_key.rsa.private_key_pem
  bucket                 = data.aws_s3_bucket.config_bucket.id
  server_side_encryption = "AES256"
  tags = {
    Region = "Mumbai"
  }
}

# resource "aws_spot_fleet_request" "stockpos_mumbai_staging_fleet_request" {
#   provider                            = aws.mumbai
#   allocation_strategy                 = "lowestPrice"
#   excess_capacity_termination_policy  = "Default"
#   fleet_type                          = "maintain"
#   iam_fleet_role                      = aws_iam_role.ec2_spot_fleet_tagging.arn
#   instance_interruption_behaviour     = "terminate"
#   instance_pools_to_use_count         = 1
#   on_demand_allocation_strategy       = "lowestPrice"
#   on_demand_target_capacity           = 0
#   replace_unhealthy_instances         = true
#   target_capacity                     = 1
#   terminate_instances_with_expiration = true
#   wait_for_fulfillment                = true

#   depends_on = [
#     aws_eip.stockpos_mumbai_ip
#   ]

#   spot_maintenance_strategies {
#     capacity_rebalance {
#       replacement_strategy = "launch"
#     }
#   }

#   launch_template_config {
#     launch_template_specification {
#       id      = aws_launch_template.stockpos_mumbai_template.id
#       version = "$Default"
#     }

#     dynamic "overrides" {
#       for_each = var.staging_spot_instance_types
#       content {
#         instance_type = overrides.value
#         subnet_id     = aws_subnet.mumbai_public_subnets[0].id
#       }
#     }

#     dynamic "overrides" {
#       for_each = var.staging_spot_instance_types
#       content {
#         instance_type = overrides.value
#         subnet_id     = aws_subnet.mumbai_public_subnets[1].id
#       }
#     }

#     dynamic "overrides" {
#       for_each = var.staging_spot_instance_types
#       content {
#         instance_type = overrides.value
#         subnet_id     = aws_subnet.mumbai_public_subnets[2].id
#       }
#     }
#   }
# }

resource "aws_instance" "stockpos_mumbai_staging" {
  provider = aws.mumbai
  # disable_api_termination = true

  depends_on = [
    aws_db_instance.stockpos_db_instance
  ]

  launch_template {
    id      = aws_launch_template.stockpos_mumbai_template.id
    version = "$Default"
  }
  instance_type = var.staging_spot_instance_types[0]
  subnet_id     = random_shuffle.subnet_for_stockpos.result[0]
  tags = {
    "Name" = var.server_tag_value
  }
  lifecycle {
    ignore_changes = [ami, user_data, launch_template]
  }
}

# Elastic IP for StockPOS mumbai Staging
resource "aws_eip" "stockpos_mumbai_ip" {
  provider = aws.mumbai
  instance = aws_instance.stockpos_mumbai_staging.id
  tags = {
    "Name" = var.server_tag_value
  }
  vpc = true
}

# Security Group for StockPOS Staging
resource "aws_security_group" "stockpos_mumbai_sg" {
  provider = aws.mumbai
  name     = "stockpos-mumbai-staging"
  vpc_id   = aws_vpc.mumbai_vpc.id
  tags = {
    "Name" = var.server_tag_value
  }
  description = "SG for StockPOS mumbai Staging Server"
  ingress {
    cidr_blocks = ["0.0.0.0/0"]
    from_port   = 22
    protocol    = "tcp"
    to_port     = 22
  }
  ingress {
    cidr_blocks      = ["0.0.0.0/0"]
    from_port        = 443
    ipv6_cidr_blocks = ["::/0"]
    protocol         = "tcp"
    to_port          = 443
  }
  ingress {
    cidr_blocks      = ["0.0.0.0/0"]
    from_port        = 80
    ipv6_cidr_blocks = ["::/0"]
    protocol         = "tcp"
    to_port          = 80
  }
  egress {
    cidr_blocks = ["0.0.0.0/0"]
    from_port   = 0
    protocol    = "-1"
    to_port     = 0
  }
}

resource "aws_ssm_association" "stockpos_staging_cloud_watch_update" {
  provider                    = aws.mumbai
  association_name            = "Cloud_Watch_Agent_For_StockPos_Staging"
  apply_only_at_cron_interval = true
  name                        = "AWS-ConfigureAWSPackage"
  parameters = {
    "action"           = "Install"
    "installationType" = "Uninstall and reinstall"
    "name"             = "AmazonCloudWatchAgent"
  }
  schedule_expression = "cron(0 23 ? * SUN *)"

  targets {
    key = "tag:Name"
    values = [
      var.server_tag_value
    ]
  }
}

resource "aws_ssm_maintenance_window" "stockpos_mumbai_maintenace_window" {
  provider                   = aws.mumbai
  allow_unassociated_targets = false
  cutoff                     = 0
  description                = "Patching Windows for StockPOS mumbai Staging Server"
  duration                   = 1
  enabled                    = true
  name                       = var.server_tag_value
  schedule                   = "cron(0 22 ? * SUN *)"
  schedule_timezone          = "Asia/Yangon"
}

resource "aws_ssm_maintenance_window_target" "stockpos_mumbai_target" {
  provider      = aws.mumbai
  window_id     = aws_ssm_maintenance_window.stockpos_mumbai_maintenace_window.id
  description   = "Target for StockPOS mumbai Staging EC2 Instance"
  name          = var.server_tag_value
  resource_type = "INSTANCE"

  # depends_on = [
  #   aws_spot_fleet_request.stockpos_mumbai_staging_fleet_request
  # ]

  targets {
    key = "tag:Name"
    values = [
      var.server_tag_value
    ]
  }
}

resource "aws_ssm_maintenance_window_task" "stockpos_mumbai_window_task" {
  provider         = aws.mumbai
  cutoff_behavior  = "CANCEL_TASK"
  description      = "Patching Run Command for Ubuntu"
  max_concurrency  = "100%"
  max_errors       = "50%"
  name             = "StockPOS-mumbai-Staging-Server-Ubuntu-Patching"
  priority         = 1
  service_role_arn = aws_iam_role.maintenance_window_run_command.arn
  task_arn         = "AWS-RunPatchBaseline"
  task_type        = "RUN_COMMAND"
  window_id        = aws_ssm_maintenance_window.stockpos_mumbai_maintenace_window.id

  targets {
    key = "WindowTargetIds"
    values = [
      aws_ssm_maintenance_window_target.stockpos_mumbai_target.id
    ]
  }

  task_invocation_parameters {

    run_command_parameters {
      document_version = "$LATEST"
      timeout_seconds  = 600

      cloudwatch_config {
        cloudwatch_log_group_name = aws_cloudwatch_log_group.stockpos_mumbai_patching_task_log_group.name
        cloudwatch_output_enabled = true
      }

      parameter {
        name = "Operation"
        values = [
          "Install",
        ]
      }
      parameter {
        name = "RebootOption"
        values = [
          "RebootIfNeeded",
        ]
      }
    }
  }
}

// Cloud Watch Log Group
resource "aws_cloudwatch_log_group" "stockpos_mumbai_patching_task_log_group" {
  provider          = aws.mumbai
  name              = "/aws/ssm/stockpos-mumbai-staging-ubuntu-server-patching"
  retention_in_days = 30
}
