// Store Terraform Backend State on S3 Bucket
terraform {
  backend "s3" {
    bucket         = "terraform-backend-state-on-s3-amk"
    key            = "stockpos/backend-state"
    region         = "ap-southeast-1"
    dynamodb_table = "terraform_state_locks"
    encrypt        = true
    profile        = "aws-admin"
  }
}

// Define Provider and Region
provider "aws" {
  region  = "ap-south-1"
  profile = "aws-admin"
  alias   = "mumbai"
  default_tags {
    tags = {
      Project = "StockPOS"
    }
  }
}

// Define Provider and Region
provider "aws" {
  region  = "ap-south-2"
  profile = "aws-admin"
  alias   = "hyderabad"
  default_tags {
    tags = {
      Project = "StockPOS"
    }
  }
}

// Define Provider and Region
provider "aws" {
  region  = "ap-southeast-1"
  profile = "aws-admin"
  default_tags {
    tags = {
      Project = "StockPOS"
    }
  }
}
