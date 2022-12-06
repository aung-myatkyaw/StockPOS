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
