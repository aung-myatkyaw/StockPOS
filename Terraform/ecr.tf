resource "aws_ecr_repository" "stockpos_backend" {
  image_tag_mutability = "MUTABLE"
  name                 = "stockpos-backend"

  image_scanning_configuration {
    scan_on_push = false
  }
}

resource "aws_ecr_lifecycle_policy" "stockpos_backend_lifecycle_policy" {
  repository = aws_ecr_repository.stockpos_backend.name
  policy = jsonencode(
    {
      rules = [
        {
          action = {
            type = "expire"
          }
          description  = "Remove Untagged Images"
          rulePriority = 1
          selection = {
            countNumber = 1
            countType   = "sinceImagePushed"
            countUnit   = "days"
            tagStatus   = "untagged"
          }
        },
      ]
    }
  )
}

# ECR URL in parameter store
resource "aws_ssm_parameter" "mumbai_ecr_url" {
  provider    = aws.mumbai
  name        = "stockpos-ecr-url"
  description = "ECR URL for stockpos"
  type        = "SecureString"
  value       = aws_ecr_repository.stockpos_backend.repository_url
}

# ECR URL in parameter store
resource "aws_ssm_parameter" "hyderabad_ecr_url" {
  provider    = aws.hyderabad
  name        = "stockpos-ecr-url"
  description = "ECR URL for stockpos"
  type        = "SecureString"
  value       = aws_ecr_repository.stockpos_backend.repository_url
}

# ECR URL in parameter store
resource "aws_ssm_parameter" "ecr_url" {
  name        = "stockpos-ecr-url"
  description = "ECR URL for stockpos"
  type        = "SecureString"
  value       = aws_ecr_repository.stockpos_backend.repository_url
}
