data "aws_iam_policy_document" "assume_role" {
  statement {
    effect = "Allow"

    principals {
      type        = "Service"
      identifiers = ["lambda.amazonaws.com"]
    }

    actions = [
      "sts:AssumeRole"
    ]
  }
}

resource "aws_iam_role" "lambda_iam_role" {
  name               = "lambda_iam_role"
  assume_role_policy = data.aws_iam_policy_document.assume_role.json
}

#### AWS Lambda ####
resource "aws_lambda_function" "this" {
  function_name = var.aws_lambda_name
  description   = "A function which run BE API project."
  ephemeral_storage {
    size = 512 # Min 512 MB and the Max 10240 MB
  }
  architectures = [var.aws_lambda_architecture]
  publish       = true
  timeout       = var.aws_lambda_timeout
  memory_size   = var.aws_lambda_memory_size

  role = aws_iam_role.lambda_iam_role.arn

  environment {
    variables = {
      Aws__Parameters__Names                 = var.aws_lambda_parameter_names
      Aws__Parameters__Ignore                = false
      Aws__Parameters__SecretPollingInterval = 3600
    }
  }

  tags = merge(
    var.common_tags,
    {
      project = "assistants-budget-be-api"
    }
  )
  package_type = "Image"
  image_uri    = var.aws_lambda_docker_image_url

  lifecycle {
    ignore_changes = [
      image_uri
    ]
  }
}
