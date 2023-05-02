variable "acm_certificate_arn" {
  description = "ACM Certificate ARN to setup over Assistants - Budget UI project"
}

variable "assistants_budget_ui_s3_bucket_name" {
  description = "S3 bucket where UI assets are stored for Assistants - Budget UI project"
}
variable "assistants_budget_ui_cloudfront_domain_name" {
  description = "CloudFront Domain name which should be assigned to Assistants - Budget UI project"
}
variable "assistants_budget_ui_route53_hostedzone_id" {
  description = "AWS Route53 Hosted Zone ID for CloudFront distribution alias"
}
