// Pipeline for building and deploying stockpos backend docker image to ec2 instance
resource "aws_codepipeline" "stockpos_backend_pipeline" {
  provider = aws.mumbai
  name     = replace(var.backend_pipeline_name, "{{REGION_NAME}}", data.aws_region.current.name)

  # depends_on = [
  #   aws_spot_fleet_request.stockpos_staging_fleet_request
  # ]

  artifact_store {
    location = aws_s3_bucket.codepipeline_bucket.bucket
    region   = data.aws_region.mumbai.name
    type     = "S3"
  }

  artifact_store {
    location = aws_s3_bucket.codedeploy_bucket.bucket
    region   = data.aws_region.current.name
    type     = "S3"
  }

  role_arn = aws_iam_role.codepipeline_role.arn

  //Stage for Pulling Codes from GitHub
  stage {
    name = "Source"

    action {
      name             = "Source"
      category         = "Source"
      owner            = "AWS"
      provider         = "CodeStarSourceConnection"
      version          = "1"
      output_artifacts = ["SourceArtifact"]
      namespace        = "SourceVariables"

      configuration = {
        ConnectionArn        = aws_codestarconnections_connection.github_connection.arn
        FullRepositoryId     = var.backend_git_url
        BranchName           = var.git_branch
        OutputArtifactFormat = "CODE_ZIP"
      }
    }
  }

  stage {
    name = "Confirm"

    action {
      category = "Approval"
      configuration = {
        "CustomData"      = "Please approve this change. Commit Id: #{SourceVariables.CommitId}, Trigger Branch: #{SourceVariables.BranchName}, Commit message: #{SourceVariables.CommitMessage}"
        "NotificationArn" = aws_sns_topic.mumbai_notification_topic.arn
      }
      name     = "Approval"
      owner    = "AWS"
      provider = "Manual"
      version  = "1"
    }
  }

  stage {
    name = "Build"

    action {
      name             = "Build"
      category         = "Build"
      owner            = "AWS"
      provider         = "CodeBuild"
      input_artifacts  = ["SourceArtifact"]
      output_artifacts = ["BuildArtifact"]
      version          = "1"
      region           = data.aws_region.mumbai.name
      namespace        = "BuildVariables"

      configuration = {
        ProjectName = aws_codebuild_project.stockpos_backend_build.name
      }
    }
  }

  //Deploying to ec2 instance with CodeDeploy
  stage {
    name = "Deploy"

    action {
      name            = "Deploy"
      category        = "Deploy"
      owner           = "AWS"
      provider        = "CodeDeploy"
      input_artifacts = ["BuildArtifact"]
      version         = "1"
      region          = data.aws_region.current.name
      namespace       = "DeployVariables"

      configuration = {
        ApplicationName     = aws_codedeploy_app.stockpos_codedeploy_application.name
        DeploymentGroupName = aws_codedeploy_deployment_group.stockpos_codedeploy_group.deployment_group_name
      }
    }
  }
}

// CodeBuild Project
resource "aws_codebuild_project" "stockpos_backend_build" {
  provider               = aws.mumbai
  name                   = var.backend_build_name
  description            = "CodeBuild Project for Building and Pushing KMS Backend Docker Image to ECR"
  concurrent_build_limit = 1
  artifacts {
    type = "CODEPIPELINE"
    name = var.backend_build_name
  }

  lifecycle {
    ignore_changes = [project_visibility]
  }

  # cache {
  #   type  = "LOCAL"
  #   modes = ["LOCAL_DOCKER_LAYER_CACHE", "LOCAL_SOURCE_CACHE"]
  # }

  environment {
    compute_type = "BUILD_GENERAL1_SMALL"

    image = var.machine_type == "amd64" ? "aws/codebuild/standard:6.0" : "aws/codebuild/amazonlinux2-aarch64-standard:2.0"
    type  = var.machine_type == "amd64" ? "LINUX_CONTAINER" : "ARM_CONTAINER"

    image_pull_credentials_type = "CODEBUILD"
    privileged_mode             = true
  }
  project_visibility = "PRIVATE"

  service_role = aws_iam_role.ecr_codebuild_role.arn

  source {
    buildspec       = "StockPOS/Deploy/buildspec.yml"
    type            = "CODEPIPELINE"
    git_clone_depth = 0
  }

  logs_config {
    cloudwatch_logs {
      group_name = aws_cloudwatch_log_group.stockpos_backend_log_group.name
    }
  }
}

// Cloud Watch Log Group
resource "aws_cloudwatch_log_group" "stockpos_backend_log_group" {
  provider          = aws.mumbai
  name              = format("%s%s", "/aws/codebuild/", var.backend_build_name)
  retention_in_days = 30
}

// StockPOS Code Deploy Application For Current Region
resource "aws_codedeploy_app" "stockpos_codedeploy_application" {
  provider         = aws.hyderabad
  compute_platform = "Server"
  name             = replace(var.codedeploy_application_name, "{{REGION_NAME}}", data.aws_region.current.name)
}

// Deployment Group for ec2 provisioning
resource "aws_codedeploy_deployment_group" "stockpos_codedeploy_group" {
  provider              = aws.hyderabad
  app_name              = aws_codedeploy_app.stockpos_codedeploy_application.name
  deployment_group_name = var.codedeploy_group_name
  service_role_arn      = aws_iam_role.codedeploy_role.arn

  auto_rollback_configuration {
    enabled = true
    events  = ["DEPLOYMENT_FAILURE"]
  }

  ec2_tag_set {
    ec2_tag_filter {
      key   = "Name"
      type  = "KEY_AND_VALUE"
      value = var.server_tag_value
    }
  }
}
