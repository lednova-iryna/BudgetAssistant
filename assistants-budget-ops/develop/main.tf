module "assistants-budget-ui-distribution" {
  source                         = "./modules/assistants-budget-ui-distribution"
  common_tags                    = local.common_tags
  bucket_name                    = var.assistants_budget_ui_s3_bucket_name
  cloudfront_domain_name         = var.assistants_budget_ui_cloudfront_domain_name
  cloudfront_acm_certificate_arn = var.acm_certificate_arn
  aws_route53_hostedzone_id      = var.assistants_budget_ui_route53_hostedzone_id
}
