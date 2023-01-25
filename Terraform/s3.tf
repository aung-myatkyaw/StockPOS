// Pipeline Artifact Store Bucket for Mumbai Region
resource "aws_s3_bucket" "codepipeline_bucket_mumbai" {
  provider = aws.mumbai
  bucket   = var.mumbai_pipeline_bucket_name
}

// PipeLine LifeCycle
resource "aws_s3_bucket_lifecycle_configuration" "codepipeline_bucket_mumbai_lifecycle_rule" {
  bucket   = aws_s3_bucket.codepipeline_bucket_mumbai.id
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
