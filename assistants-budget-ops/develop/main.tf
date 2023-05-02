module "assistants-budget-ui-distribution" {
  source                         = "./modules/assistants-budget-ui-distribution"
  common_tags                    = local.common_tags
  bucket_name                    = "assistants-budget-ui-distribution-${local.common_tags.environment}"
  cloudfront_domain_name         = "${local.common_tags.environment}.budget.lednova.io"
  cloudfront_acm_certificate_arn = var.acm_certificate_arn
}
