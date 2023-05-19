module "assistants_budget_ui_distribution" {
  source                         = "./modules/assistants-budget-ui-distribution"
  common_tags                    = local.common_tags
  bucket_name                    = var.assistants_budget_ui_s3_bucket_name
  cloudfront_domain_name         = var.assistants_budget_ui_cloudfront_domain_name
  cloudfront_acm_certificate_arn = var.acm_certificate_arn
  aws_route53_hostedzone_id      = var.assistants_budget_ui_route53_hostedzone_id
}

module "assistants_budget_be_api_lambda" {
  source      = "./modules/assistants-budget-be-api-lambda"
  common_tags = local.common_tags

  aws_lambda_architecture     = "arm64"
  aws_lambda_docker_image_url = ""
  aws_lambda_memory_size      = 256
  aws_lambda_parameter_name   = "/develop/assistants-budget-be-api-lambda"
  aws_lambda_timeout          = 60
  aws_lambda_name             = "assistants-budget-be-api-lambda-develop"
}

module "assistants_budget_be_api_gateway" {
  source                                        = "./modules/aws-gateway-http-api"
  common_tags                                   = local.common_tags
  api_gateway_http_api_name                     = "Assistants: Budget. Develop"
  assistants_budget_be_api_lambda_function_name = module.assistants_budget_be_api_lambda.lambda_name
  assistants_budget_be_api_lambda_invoke_arn    = module.assistants_budget_be_api_lambda.lambda_arn
  api_gateway_http_api_post_stage_name          = "develop"
  api_gateway_cors_options = {
    allow_headers  = "*"
    allow_origin   = "*"
    allow_methods  = "*"
    expose_headers = "*"
  }
}
