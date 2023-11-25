// Store Terraform Backend State on S3 Bucket
terraform {
  backend "s3" {
    bucket         = "terraform-backend-state-wkh"
    key            = "stockpos/backend-state"
    region         = "ap-southeast-1"
    dynamodb_table = "terraform_state_locks"
    encrypt        = true
    profile        = "wkh"
  }
}

// Define Provider and Region
provider "aws" {
  region  = "ap-south-1"
  profile = "wkh"
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
  profile = "wkh"
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
  profile = "wkh"
  default_tags {
    tags = {
      Project = "StockPOS"
    }
  }
}
