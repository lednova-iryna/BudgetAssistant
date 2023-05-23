resource "aws_apigatewayv2_route" "assistants_budget_be_api" {
  api_id    = aws_apigatewayv2_api.http_api.id
  route_key = "$default"
  #   authorization_scopes = toset(split(" ", var.audit_read_scope))
  #   authorization_type   = "JWT"
  #   authorizer_id        = aws_apigatewayv2_authorizer.jwt_auth.id

  target = "integrations/${aws_apigatewayv2_integration.this.id}"

  depends_on = [
    aws_apigatewayv2_api.http_api,
    aws_apigatewayv2_stage.default_stage
  ]
}

resource "aws_apigatewayv2_integration" "this" {
  api_id                 = aws_apigatewayv2_api.http_api.id
  integration_type       = "AWS_PROXY"
  connection_type        = "INTERNET"
  description            = "BE API"
  integration_method     = "ANY"
  integration_uri        = var.assistants_budget_be_api_lambda_invoke_arn
  passthrough_behavior   = "WHEN_NO_MATCH"
  payload_format_version = "2.0"
}

resource "aws_lambda_permission" "lambda_audit_data_permission" {
  action        = "lambda:InvokeFunction"
  function_name = var.assistants_budget_be_api_lambda_function_name
  principal     = "apigateway.amazonaws.com"
  source_arn    = "${aws_apigatewayv2_api.http_api.execution_arn}/*"
}
