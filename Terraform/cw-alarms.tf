resource "aws_cloudwatch_metric_alarm" "stockpos_ec2_cpu_alarm" {
  provider        = aws.hyderabad
  actions_enabled = true
  alarm_actions = [
    aws_sns_topic.notification_topic.arn
  ]
  alarm_description   = "Alarm for High CPU in StockPOS"
  alarm_name          = "ec2-stockpos-high-cpu"
  comparison_operator = "GreaterThanThreshold"
  threshold           = 40
  datapoints_to_alarm = 3
  evaluation_periods  = 5
  dimensions = {
    "InstanceId" = aws_instance.stockpos_staging.id
  }
  metric_name = "CPUUtilization"
  namespace   = "AWS/EC2"
  statistic   = "Average"
  period      = 300
  ok_actions = [
    aws_sns_topic.notification_topic.arn
  ]
  treat_missing_data = "missing"
}

resource "aws_cloudwatch_metric_alarm" "stockpos_ec2_status_check_alarm" {
  provider        = aws.hyderabad
  actions_enabled = true
  alarm_actions = [
    aws_sns_topic.notification_topic.arn
  ]
  alarm_description   = "Alarm for Status Check Fail in StockPOS"
  alarm_name          = "ec2-stockpos-status-check"
  comparison_operator = "GreaterThanThreshold"
  threshold           = 0
  datapoints_to_alarm = 1
  evaluation_periods  = 1
  dimensions = {
    "InstanceId" = aws_instance.stockpos_staging.id
  }
  metric_name = "StatusCheckFailed"
  namespace   = "AWS/EC2"
  period      = 60
  statistic   = "Average"
  ok_actions = [
    aws_sns_topic.notification_topic.arn
  ]
  treat_missing_data = "missing"
}

resource "aws_cloudwatch_metric_alarm" "stockpos_ec2_memory_alarm" {
  provider        = aws.hyderabad
  actions_enabled = true
  alarm_actions = [
    aws_sns_topic.notification_topic.arn
  ]
  alarm_description   = "Alarm for High Memory Usage in StockPOS"
  alarm_name          = "ec2-stockpos-high-memory"
  comparison_operator = "GreaterThanThreshold"
  threshold           = 80
  datapoints_to_alarm = 3
  evaluation_periods  = 5
  dimensions = {
    "InstanceId" = aws_instance.stockpos_staging.id
  }
  metric_name = "mem_used_percent"
  namespace   = "CWAgent"
  period      = 60
  statistic   = "Average"
  ok_actions = [
    aws_sns_topic.notification_topic.arn
  ]
  treat_missing_data = "missing"
}

resource "aws_cloudwatch_metric_alarm" "stockpos_ec2_disk_usage_alarm" {
  provider        = aws.hyderabad
  actions_enabled = true
  alarm_actions = [
    aws_sns_topic.notification_topic.arn
  ]
  alarm_description   = "Alarm for High Disk Usage in StockPOS"
  alarm_name          = "ec2-stockpos-high-disk-usage"
  comparison_operator = "GreaterThanOrEqualToThreshold"
  threshold           = 90
  datapoints_to_alarm = 2
  evaluation_periods  = 3
  dimensions = {
    "InstanceId" = aws_instance.stockpos_staging.id
  }
  metric_name = "disk_used_percent"
  namespace   = "CWAgent"
  period      = 300
  statistic   = "Average"
  ok_actions = [
    aws_sns_topic.notification_topic.arn
  ]
  treat_missing_data = "missing"
}

resource "aws_cloudwatch_metric_alarm" "stockpos_ec2_cpu_credit_alarm" {
  provider        = aws.hyderabad
  actions_enabled = true
  alarm_actions = [
    aws_sns_topic.notification_topic.arn
  ]
  alarm_description   = "Alarm for No CPU Credit in StockPOS"
  alarm_name          = "ec2-stockpos-no-cpu-credit"
  comparison_operator = "LessThanThreshold"
  threshold           = 1
  datapoints_to_alarm = 2
  evaluation_periods  = 2
  dimensions = {
    "InstanceId" = aws_instance.stockpos_staging.id
  }
  metric_name = "CPUCreditBalance"
  namespace   = "AWS/EC2"
  period      = 300
  statistic   = "Average"
  ok_actions = [
    aws_sns_topic.notification_topic.arn
  ]
  treat_missing_data = "missing"
}

resource "aws_cloudwatch_metric_alarm" "stockpos_rds_cpu_credit_alarm" {
  provider        = aws.hyderabad
  actions_enabled = true
  alarm_actions = [
    aws_sns_topic.notification_topic.arn
  ]
  alarm_description   = "Alarm for No CPU Credit in StockPOS DB Instance"
  alarm_name          = "rds-stockpos-no-cpu-credit"
  comparison_operator = "LessThanThreshold"
  threshold           = 1
  datapoints_to_alarm = 2
  evaluation_periods  = 2
  dimensions = {
    "DBInstanceIdentifier" = aws_db_instance.stockpos_db_instance.identifier
  }
  metric_name = "CPUCreditBalance"
  namespace   = "AWS/RDS"
  period      = 300
  statistic   = "Average"
  ok_actions = [
    aws_sns_topic.notification_topic.arn
  ]
  treat_missing_data = "missing"
}
