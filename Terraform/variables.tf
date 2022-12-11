variable "main_cidr_block" {
  description = "Main CIDR Block for All Regions"
  default     = "10.0.0.0/8"
}

variable "aws_region_list_for_cidr" {
  description = "AWS Region List for creating CIDR Blocks"
  default = {
    "ap-southeast-1" = 0
    "ap-south-1"     = 1
    "us-west-2"      = 2
  }
}

variable "mumbai_pipeline_bucket_name" {
  description = "Storage Bucket for Pipeline Artifacts"
  default     = "codepipeline-artifacts-storage-mumbai-amk-152"
}

variable "server_tag_value" {
  description = "Tag Value for server"
  default     = "stockpos"
}

variable "staging_ec2_key_name" {
  description = "Staging Server EC2 Key Name"
  default     = "stockpos-staging"
}

variable "singapore_configs_bucket_name" {
  description = "Storage Bucket for Config files"
  default     = "config-files-amk-152"
}

variable "staging_spot_instance_types" {
  description = "Spot Instance Types for staging server"
  type        = list(string)
  default     = ["t4g.small"]
}

variable "ecr_build_role_name" {
  description = "Role for CodeBuild for ECR"
  default     = "stockpos-codebuild-role"
}

variable "ecr_build_policy_name" {
  description = "Role for CodeBuild for ECR"
  default     = "stockpos-codebuild-policy"
}

variable "github_connection_name" {
  description = "GitHub Connection Name"
  default     = "stockpos-github-connection"
}

variable "backend_git_url" {
  description = "Backend GitHub Repo name"
  default     = "yinko2/StockPOS"
}

variable "git_branch" {
  description = "Branch Name"
  default     = "master"
}

variable "backend_build_name" {
  description = "Backend CodeBuild Name"
  default     = "stockpos-backend-mumbai-build-and-push-docker-image"
}

variable "pipeline_role_name" {
  description = "Role for pipeline"
  default     = "AWSCodePipelineServiceRole-StockPOS"
}

variable "backend_pipeline_name" {
  description = "Backend Pipeline Name"
  default     = "stockpos-backend-mumbai"
}

variable "codedeploy_application_name" {
  description = "Codedeploy Application Name"
  default     = "StockPOS-CodeDeploy-Application-Mumbai"
}

variable "codedeploy_group_name" {
  description = "Codedeploy Group Name"
  default     = "production"
}

variable "codedeploy_role_name" {
  description = "Role for CodeDeploy"
  default     = "CodeDeployRole"
}
