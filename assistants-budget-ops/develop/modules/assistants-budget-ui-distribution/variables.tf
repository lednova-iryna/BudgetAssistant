variable "common_tags" {
  description = "Common tags values"
  default     = {}
}

variable "bucket_name" {
  description = "S3 bucket where UI assets are stored"
}

variable "cloudfront_acm_certificate_arn" {
  description = "ACM for ssl certificate in aws"
}

variable "cloudfront_domain_name" {
  description = "Domain Name of UI web app"
}

variable "aws_route53_hostedzone_id" {
  description = "AWS Route53 Hosted Zone ID for CloudFront distribution alias"
}
