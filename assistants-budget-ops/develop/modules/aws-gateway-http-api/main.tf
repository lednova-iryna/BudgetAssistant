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
  name        = var.api_gateway_http_api_stage_name
  auto_deploy = true

  # access_log_settings {
  #   destination_arn = aws_cloudwatch_log_group.api_gw.arn

  #   format = jsonencode({
  #     requestId               = "$context.requestId"
  #     sourceIp                = "$context.identity.sourceIp"
  #     requestTime             = "$context.requestTime"
  #     protocol                = "$context.protocol"
  #     httpMethod              = "$context.httpMethod"
  #     resourcePath            = "$context.resourcePath"
  #     routeKey                = "$context.routeKey"
  #     status                  = "$context.status"
  #     responseLength          = "$context.responseLength"
  #     integrationErrorMessage = "$context.integrationErrorMessage"
  #     }
  #   )
  # }

  default_route_settings {
    logging_level            = var.default_route_settings.logging_level
    data_trace_enabled       = var.default_route_settings.data_trace_enabled
    detailed_metrics_enabled = var.default_route_settings.metrics_enabled
    throttling_burst_limit   = var.default_route_settings.throttling_burst_limit
    throttling_rate_limit    = var.default_route_settings.throttling_rate_limit
  }
}

resource "aws_apigatewayv2_domain_name" "this" {
  domain_name = var.api_gateway_http_api_domain_name

  domain_name_configuration {
    certificate_arn = var.acm_certificate_arn
    endpoint_type   = "REGIONAL"
    security_policy = "TLS_1_2"
  }
}

resource "aws_route53_record" "this" {
  name    = aws_apigatewayv2_domain_name.this.domain_name
  type    = "A"
  zone_id = var.route53_hostedzone_id

  alias {
    name                   = aws_apigatewayv2_domain_name.this.domain_name_configuration[0].target_domain_name
    zone_id                = aws_apigatewayv2_domain_name.this.domain_name_configuration[0].hosted_zone_id
    evaluate_target_health = false
  }
}


resource "aws_apigatewayv2_api_mapping" "this" {
  api_id      = aws_apigatewayv2_api.http_api.id
  domain_name = aws_apigatewayv2_domain_name.this.id
  stage       = aws_apigatewayv2_stage.default_stage.id
}
