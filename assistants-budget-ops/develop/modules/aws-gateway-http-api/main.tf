resource "aws_apigatewayv2_api" "http_api" {
  name                         = var.api_gateway_http_api_name
  protocol_type                = "HTTP"
  description                  = "Instance of the main BE API"
  disable_execute_api_endpoint = true
  cors_configuration {
    allow_origins  = toset(split(",", var.api_gateway_cors_options.allow_origin))
    allow_methods  = toset(split(",", var.api_gateway_cors_options.allow_methods))
    allow_headers  = toset(split(",", var.api_gateway_cors_options.allow_headers))
    expose_headers = toset(split(",", var.api_gateway_cors_options.expose_headers))
    max_age        = 0
  }

  tags = merge(
    var.common_tags,
    {
      project = "assistants-budget-be-api"
    }
  )
}

resource "aws_apigatewayv2_stage" "default_stage" {
  api_id      = aws_apigatewayv2_api.http_api.id
  name        = var.api_gateway_http_api_post_stage_name
  auto_deploy = true
  access_log_settings {
    destination_arn = var.logging_group_arn
    format          = "{ \"requestId\":\"$context.requestId\", \"ip\": \"$context.identity.sourceIp\", \"requestTime\":\"$context.requestTime\", \"httpMethod\":\"$context.httpMethod\",\"routeKey\":\"$context.routeKey\", \"status\":\"$context.status\",\"protocol\":\"$context.protocol\", \"responseLength\":\"$context.responseLength\", \"error\":\"$context.integrationErrorMessage\" }"
  }
  default_route_settings {
    logging_level            = var.default_route_settings.logging_level
    data_trace_enabled       = var.default_route_settings.data_trace_enabled
    detailed_metrics_enabled = var.default_route_settings.metrics_enabled
    throttling_burst_limit   = var.default_route_settings.throttling_burst_limit
    throttling_rate_limit    = var.default_route_settings.throttling_rate_limit
  }
}
