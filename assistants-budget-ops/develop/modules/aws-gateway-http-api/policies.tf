resource "aws_iam_role" "api_gateway_http_execution_role" {
  name               = "audit-api-gateway-http-execution-role"
  description        = "Assumed role for the HTTP API gateway"
  assume_role_policy = data.aws_iam_policy_document.api_gateway_http_assume_role.json

  tags = merge(
    var.common_tags,
    {
      project = "assistants-budget-be-api"
    }
  )
}

data "aws_iam_policy_document" "api_gateway_http_assume_role" {
  statement {
    effect = "Allow"

    principals {
      type        = "Service"
      identifiers = ["apigateway.amazonaws.com"]
    }

    actions = ["sts:AssumeRole"]
  }
}
