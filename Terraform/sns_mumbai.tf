// Mumbai Region Notification Topic
resource "aws_sns_topic" "mumbai_notification_topic" {
  provider = aws.mumbai
  name     = "mumbai-sns-topic"
}

// Mumbai Region Notification Subscriptions
resource "aws_sns_topic_subscription" "mumbai_notification_topic_subscriptions" {
  provider  = aws.mumbai
  count     = length(var.mumbai_subscription_emails)
  topic_arn = aws_sns_topic.mumbai_notification_topic.arn
  protocol  = "email"
  endpoint  = var.mumbai_subscription_emails[count.index]
}

// Topic Policy for Mumbai Staging
resource "aws_sns_topic_policy" "mumbai_notification_topic_policy" {
  provider = aws.mumbai
  arn      = aws_sns_topic.mumbai_notification_topic.arn
  policy   = data.aws_iam_policy_document.mumbai_sns_topic_policy.json
}

// policy document
data "aws_iam_policy_document" "mumbai_sns_topic_policy" {
  statement {
    actions = ["SNS:Publish"]

    effect = "Allow"

    principals {
      type        = "Service"
      identifiers = ["codestar-notifications.amazonaws.com", "events.amazonaws.com"]
    }

    resources = [
      aws_sns_topic.mumbai_notification_topic.arn,
    ]
  }
  version = "2008-10-17"
}

// EventBridge Rule for spot fleet change in Mumbai Region
resource "aws_cloudwatch_event_rule" "spot_fleet_change_event_notification_rule_mumbai" {
  provider = aws.mumbai
  name     = "ec2-spot-fleet-change-mumbai"
  event_pattern = jsonencode(
    {
      detail-type = [
        "EC2 Spot Fleet Instance Change",
        "EC2 Spot Fleet Information",
        "EC2 Spot Fleet Error",
        "EC2 Spot Instance Interruption Warning",
        "EC2 Instance Rebalance Recommendation"
      ]
      source = [
        "aws.ec2spotfleet",
        "aws.ec2"
      ]
    }
  )
}

// EventBridge Spot Change Noti Target in Mumbai Region
resource "aws_cloudwatch_event_target" "spot_fleet_change_event_noti_target_mumbai" {
  provider  = aws.mumbai
  target_id = "spot-change-trigger-lambda-function-to-process-sns-mumbai"
  rule      = aws_cloudwatch_event_rule.spot_fleet_change_event_notification_rule_mumbai.name
  arn       = aws_lambda_function.sns_email_process_mumbai.arn
}

// EventBridge Rule for Pipeline State Change in Mumbai Region
resource "aws_cloudwatch_event_rule" "codepipeline_event_notification_rule_mumbai" {
  provider = aws.mumbai
  name     = "codepipeline-noti-mumbai"
  event_pattern = jsonencode(
    {
      detail = {
        state = [
          "CANCELED",
          "SUPERSEDED",
          "FAILED",
          "SUCCEEDED",
          "RESUMED",
          "STARTED"
        ]
      }
      detail-type = [
        "CodePipeline Pipeline Execution State Change"
      ]
      source = [
        "aws.codepipeline"
      ]
    }
  )
}

// EventBridge Pipeline Noti Target in Mumbai Region
resource "aws_cloudwatch_event_target" "codepipeline_event_noti_target_mumbai" {
  provider  = aws.mumbai
  target_id = "codepipeline-trigger-lambda-function-to-process-sns-mumbai"
  rule      = aws_cloudwatch_event_rule.codepipeline_event_notification_rule_mumbai.name
  arn       = aws_lambda_function.sns_email_process_mumbai.arn
}

// Archive Code For Lambda Function
data "archive_file" "sns_email_process_lambda_code_mumbai" {
  type                    = "zip"
  source_content          = replace(file("./lambda/sns-email-process/main.py"), "{{TOPIC_ARN}}", aws_sns_topic.mumbai_notification_topic.arn)
  source_content_filename = "main.py"
  output_path             = "./lambda/sns-email-process-mumbai.zip"
}

// Lambda Function for Processing SNS
resource "aws_lambda_function" "sns_email_process_mumbai" {
  provider      = aws.mumbai
  architectures = ["arm64"]
  function_name = "sns-email-process-mumbai"
  description   = "Lambda Function for Processing SNS Notifications from AWS Events"
  role          = aws_iam_role.lambda_process_sns_role.arn

  filename         = data.archive_file.sns_email_process_lambda_code_mumbai.output_path
  handler          = "main.lambda_handler"
  package_type     = "Zip"
  runtime          = "python3.9"
  source_code_hash = data.archive_file.sns_email_process_lambda_code_mumbai.output_base64sha256
  timeout          = 15

  memory_size = 128
  ephemeral_storage {
    size = 512
  }
}

// Spot Change Lambda Function Trigger Permission
resource "aws_lambda_permission" "sns_process_message_spot_mumbai_permission" {
  provider      = aws.mumbai
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.sns_email_process_mumbai.function_name
  principal     = "events.amazonaws.com"
  source_arn    = aws_cloudwatch_event_rule.spot_fleet_change_event_notification_rule_mumbai.arn
  statement_id  = "AWSEvents_spot_fleet_change_invoke_lambda_process_sns_mumbai"
}

// Codepipeline Lambda Function Trigger Permission
resource "aws_lambda_permission" "sns_process_message_codepipeline_mumbai_permission" {
  provider      = aws.mumbai
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.sns_email_process_mumbai.function_name
  principal     = "events.amazonaws.com"
  source_arn    = aws_cloudwatch_event_rule.codepipeline_event_notification_rule_mumbai.arn
  statement_id  = "AWSEvents_codepipeline_change_invoke_lambda_process_sns_mumbai"
}
