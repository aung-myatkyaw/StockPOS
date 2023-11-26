resource "aws_db_instance" "stockpos_db_instance" {
  provider                   = aws.hyderabad
  allocated_storage          = 20
  auto_minor_version_upgrade = true
  copy_tags_to_snapshot      = true
  db_subnet_group_name       = aws_db_subnet_group.stockpos_db_subnet_group.name
  deletion_protection        = false
  apply_immediately          = true #Comment this out to apply only within maintenance window in production environment
  enabled_cloudwatch_logs_exports = [
    "error",
    "slowquery"
  ]
  engine                              = "mysql"
  engine_version                      = "8.0"
  iam_database_authentication_enabled = false
  identifier                          = var.server_tag_value
  instance_class                      = "db.t4g.micro"
  maintenance_window                  = "sun:00:00-sun:00:30"
  max_allocated_storage               = 0
  monitoring_interval                 = 60
  monitoring_role_arn                 = aws_iam_role.rds_monitoring_role.arn
  multi_az                            = false
  option_group_name                   = aws_db_option_group.stockpos_db_og.name
  parameter_group_name                = aws_db_parameter_group.stockpos_db_pg.name
  password                            = random_password.stockpos_db_password.result
  # performance_insights_enabled        = true
  port                   = 3306
  publicly_accessible    = true
  skip_final_snapshot    = true
  username               = "admin"
  vpc_security_group_ids = [aws_security_group.stockpos_db_sg.id]
}

resource "random_password" "stockpos_db_password" {
  length           = 12
  override_special = "!#$%&*()-_=+[]{}<>:?"
}

resource "aws_ssm_parameter" "hyderabad_mysql_server_url" {
  provider    = aws.hyderabad
  name        = "mysql-server-url"
  description = "Server URL for MySQL"
  type        = "SecureString"
  value       = aws_db_instance.stockpos_db_instance.address
}

resource "aws_ssm_parameter" "hyderabad_mysql_username" {
  provider    = aws.hyderabad
  name        = "mysql-dbusername"
  description = "Server username for MySQL"
  type        = "SecureString"
  value       = aws_db_instance.stockpos_db_instance.username
}

resource "aws_ssm_parameter" "hyderabad_mysql_password" {
  provider    = aws.hyderabad
  name        = "mysql-dbpassword"
  description = "Admin Password for MySQL RDS"
  type        = "SecureString"
  value       = random_password.stockpos_db_password.result
}

resource "aws_ssm_parameter" "mumbai_mysql_server_url" {
  provider    = aws.mumbai
  name        = "mysql-server-url"
  description = "Server URL for MySQL"
  type        = "SecureString"
  value       = aws_db_instance.stockpos_db_instance.address
}

resource "aws_ssm_parameter" "mumbai_mysql_username" {
  provider    = aws.mumbai
  name        = "mysql-dbusername"
  description = "Server username for MySQL"
  type        = "SecureString"
  value       = aws_db_instance.stockpos_db_instance.username
}

resource "aws_ssm_parameter" "mumbai_mysql_password" {
  provider    = aws.mumbai
  name        = "mysql-dbpassword"
  description = "Admin Password for MySQL RDS"
  type        = "SecureString"
  value       = random_password.stockpos_db_password.result
}

resource "aws_ssm_parameter" "mysql_server_url" {
  name        = "mysql-server-url"
  description = "Server URL for MySQL"
  type        = "SecureString"
  value       = aws_db_instance.stockpos_db_instance.address
}

resource "aws_ssm_parameter" "mysql_username" {
  name        = "mysql-dbusername"
  description = "Server username for MySQL"
  type        = "SecureString"
  value       = aws_db_instance.stockpos_db_instance.username
}

resource "aws_ssm_parameter" "mysql_password" {
  name        = "mysql-dbpassword"
  description = "Admin Password for MySQL RDS"
  type        = "SecureString"
  value       = random_password.stockpos_db_password.result
}

resource "aws_db_subnet_group" "stockpos_db_subnet_group" {
  provider    = aws.hyderabad
  name        = "stockpos-db-subnet-group"
  description = "Subnet Group for RDS"
  subnet_ids  = data.aws_subnets.public_subnets_data.ids
  depends_on = [
    aws_subnet.public_subnets
  ]
}

resource "aws_db_parameter_group" "stockpos_db_pg" {
  provider    = aws.hyderabad
  description = "PG with logging"
  family      = "mysql8.0"
  name        = "mysql-8-parameter-group"

  # parameter {
  #   name  = "general_log"
  #   value = "1"
  # }
  parameter {
    name  = "log_output"
    value = "FILE"
  }
  # parameter {
  #   name  = "log_queries_not_using_indexes"
  #   value = "0"
  # }
  parameter {
    name  = "long_query_time"
    value = "4"
  }
  parameter {
    name  = "slow_query_log"
    value = "1"
  }
}

resource "aws_db_option_group" "stockpos_db_og" {
  provider                 = aws.hyderabad
  engine_name              = "mysql"
  major_engine_version     = "8.0"
  name                     = "default-mysql-8"
  option_group_description = "Default option group for mysql 8"

  # option {
  #   option_name = "MARIADB_AUDIT_PLUGIN"

  #   option_settings {
  #     name  = "SERVER_AUDIT_EVENTS"
  #     value = "CONNECT"
  #   }
  #   option_settings {
  #     name  = "SERVER_AUDIT_EXCL_USERS"
  #     value = "rdsadmin"
  #   }
  #   option_settings {
  #     name  = "SERVER_AUDIT_FILE_ROTATE_SIZE"
  #     value = "1000000"
  #   }
  #   option_settings {
  #     name  = "SERVER_AUDIT_FILE_ROTATIONS"
  #     value = "9"
  #   }
  # }
}

# Security Group for RDS
resource "aws_security_group" "stockpos_db_sg" {
  provider    = aws.hyderabad
  name        = var.server_tag_value
  vpc_id      = aws_vpc.vpc.id
  description = "Security Group for StockPOS RDS"
  ingress {
    cidr_blocks = ["0.0.0.0/0"]
    from_port   = 3306
    protocol    = "tcp"
    to_port     = 3306
  }
  ingress {
    cidr_blocks = [aws_vpc.vpc.cidr_block]
    from_port   = 3306
    protocol    = "tcp"
    to_port     = 3306
  }
  egress {
    cidr_blocks = ["0.0.0.0/0"]
    from_port   = 0
    protocol    = "-1"
    to_port     = 0
  }
}