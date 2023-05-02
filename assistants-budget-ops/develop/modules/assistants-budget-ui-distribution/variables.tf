variable "common_tags" {
  description = "Common tags values"
  default     = {}
}

variable "bucket_name" {}

variable "cloudfront_acm_certificate_arn" {
  description = "ACM for ssl certificate in aws"
}

variable "cloudfront_domain_name" {
  description = "Domain Name of UI web app"
}