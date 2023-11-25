resource "aws_ssm_association" "ssm_agent_update" {
  provider            = aws.hyderabad
  association_name    = "SSM-Agent-Update"
  max_concurrency     = "50"
  max_errors          = "90%"
  name                = "AWS-UpdateSSMAgent"
  schedule_expression = "rate(14 days)"

  targets {
    key    = "InstanceIds"
    values = ["*"]
  }
}

resource "aws_ssm_parameter" "cloudwatch_agent_config" {
  provider  = aws.hyderabad
  data_type = "text"
  name      = "AmazonCloudWatch-agent-config"
  type      = "String"
  value     = file("cw-agent-config.json")
}
