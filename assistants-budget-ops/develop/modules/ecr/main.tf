resource "aws_ecr_repository" "this" {
  name                 = "assistants-budget-api"
  image_tag_mutability = "MUTABLE"

  image_scanning_configuration {
    scan_on_push = true
  }
}
resource "aws_ecr_lifecycle_policy" "this" {
  repository = aws_ecr_repository.this.name
  policy = jsonencode({
    rules : [{
      "rulePriority" : 2,
      "description" : "Expire `develop` images older than 1 day and not latest",
      "selection" : {
        "tagStatus" : "tagged",
        "tagPrefixList" : ["develop-"]
        "countType" : "imageCountMoreThan",
        "countNumber" : 1
      },
      "action" : {
        "type" : "expire"
      }
      }, {
      "rulePriority" : 1,
      "description" : "Expire `production` images older than 1 day and not latest",
      "selection" : {
        "tagStatus" : "tagged",
        "tagPrefixList" : ["production-"]
        "countType" : "imageCountMoreThan",
        "countNumber" : 1
      },
      "action" : {
        "type" : "expire"
      }
    }]
  })
}
