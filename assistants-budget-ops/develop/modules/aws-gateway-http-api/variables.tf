variable "common_tags" {
  description = "Common tags values"
  default     = {}
}

variable "api_gateway_http_api_name" {
  description = "Name of HTTP API"
}

variable "api_gateway_cors_options" {
  description = "CORS settings for API Gateway"
  default = {
    allow_headers  = ""
    allow_origin   = ""
    allow_methods  = ""
    expose_headers = ""
  }
}

variable "default_route_settings" {
  description = "Default route setting for API Gateway"
  default = {
    metrics_enabled        = false
    data_trace_enabled     = false
    logging_level          = "INFO"
    throttling_rate_limit  = 100
    throttling_burst_limit = 500
  }
}

variable "assistants_budget_be_api_lambda_invoke_arn" {

}

variable "assistants_budget_be_api_lambda_function_name" {

}

variable "api_gateway_http_api_post_stage_name" {
  description = "https://docs.aws.amazon.com/apigateway/latest/developerguide/http-api-stages.html"
}
