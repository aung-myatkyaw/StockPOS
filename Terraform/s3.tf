// Pipeline Artifact Store Bucket for Current Region
resource "aws_s3_bucket" "codedeploy_bucket" {
  provider = aws.hyderabad
  bucket   = replace(var.pipeline_bucket_name, "{{REGION_NAME}}", data.aws_region.current.name)
}

resource "aws_s3_bucket" "codepipeline_bucket" {
  provider = aws.mumbai
  bucket   = replace(var.pipeline_bucket_name, "{{REGION_NAME}}", data.aws_region.mumbai.name)
}

// PipeLine LifeCycle
resource "aws_s3_bucket_lifecycle_configuration" "codedeploy_bucket_lifecycle_rule" {
  bucket   = aws_s3_bucket.codedeploy_bucket.id
  provider = aws.hyderabad
  rule {
    id = "Remove after 30 days"
    abort_incomplete_multipart_upload {
      days_after_initiation = 1
    }

    expiration {
      days = 30
    }

    noncurrent_version_expiration {
      newer_noncurrent_versions = 4
      noncurrent_days           = 1
    }

    status = "Enabled"
  }
}

resource "aws_s3_bucket_lifecycle_configuration" "codepipeline_bucket_lifecycle_rule" {
  bucket   = aws_s3_bucket.codepipeline_bucket.id
  provider = aws.mumbai
  rule {
    id = "Remove after 30 days"
    abort_incomplete_multipart_upload {
      days_after_initiation = 1
    }

    expiration {
      days = 30
    }

    noncurrent_version_expiration {
      newer_noncurrent_versions = 4
      noncurrent_days           = 1
    }

    status = "Enabled"
  }
}

// Configurations Bucket for Singapore Region
data "aws_s3_bucket" "config_bucket" {
  bucket = var.configs_bucket_name
}
