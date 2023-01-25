resource "aws_ecr_repository" "stockpos_backend" {
  provider             = aws.mumbai
  image_tag_mutability = "MUTABLE"
  name                 = "stockpos-backend-mumbai"

  image_scanning_configuration {
    scan_on_push = false
  }
}

resource "aws_ecr_lifecycle_policy" "stockpos_backend_lifecycle_policy" {
  provider   = aws.mumbai
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
resource "aws_ssm_parameter" "ecr_url" {
  provider    = aws.mumbai
  name        = "stockpos-ecr-url"
  description = "ECR URL for stockpos"
  type        = "SecureString"
  value       = aws_ecr_repository.stockpos_backend.repository_url
}
