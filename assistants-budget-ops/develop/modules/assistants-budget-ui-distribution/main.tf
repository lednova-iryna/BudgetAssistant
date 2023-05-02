resource "aws_s3_bucket" "this" {
  bucket = var.bucket_name
  tags = merge(
    var.common_tags,
    {
      project = "assistants-budget-ui"
    }
  )
}

resource "aws_s3_bucket_public_access_block" "this" {
  bucket                  = aws_s3_bucket.this.id
  block_public_acls       = true
  block_public_policy     = true
  ignore_public_acls      = true
  restrict_public_buckets = true
}

resource "aws_cloudfront_origin_access_identity" "this" {
  comment = "For ${aws_s3_bucket.this.id} bucket"
}

resource "aws_cloudfront_distribution" "this" {

  enabled             = true
  is_ipv6_enabled     = false
  default_root_object = "index.html"
  http_version        = "http2"

  origin {
    domain_name = aws_s3_bucket.this.bucket_regional_domain_name #var.cloudfront_domain_name
    origin_id   = aws_s3_bucket.this.id
    s3_origin_config {
      origin_access_identity = aws_cloudfront_origin_access_identity.this.cloudfront_access_identity_path
    }
  }
  custom_error_response {
    error_code         = "403"
    response_code      = "200"
    response_page_path = "/index.html"
  }

  custom_error_response {
    error_code         = "404"
    response_code      = "200"
    response_page_path = "/index.html"
  }

  aliases = [var.cloudfront_domain_name]
  viewer_certificate {
    acm_certificate_arn      = var.cloudfront_acm_certificate_arn
    ssl_support_method       = "sni-only"
    minimum_protocol_version = "TLSv1.2_2019"
  }

  restrictions {
    geo_restriction {
      restriction_type = "none"
    }
  }

  default_cache_behavior {
    allowed_methods  = ["DELETE", "GET", "HEAD", "OPTIONS", "PATCH", "POST", "PUT"]
    cached_methods   = ["GET", "HEAD"]
    target_origin_id = aws_s3_bucket.this.id

    forwarded_values {
      query_string = false

      cookies {
        forward = "none"
      }
    }
    compress               = true
    viewer_protocol_policy = "redirect-to-https"
    min_ttl                = 0
    default_ttl            = 86400
    max_ttl                = 31536000
  }

  tags = merge(
    var.common_tags,
    {
      project = "assistants-budget-ui"
    }
  )
}

data "aws_iam_policy_document" "this" {
  statement {
    actions   = ["s3:GetObject"]
    resources = ["${aws_s3_bucket.this.arn}/*"]

    principals {
      type        = "AWS"
      identifiers = ["${aws_cloudfront_origin_access_identity.this.iam_arn}"]
    }
  }

  statement {
    actions   = ["s3:ListBucket"]
    resources = ["${aws_s3_bucket.this.arn}"]

    principals {
      type        = "AWS"
      identifiers = ["${aws_cloudfront_origin_access_identity.this.iam_arn}"]
    }
  }
}
resource "aws_s3_bucket_policy" "s3_bucket_policy" {
  bucket = aws_s3_bucket.this.id
  policy = data.aws_iam_policy_document.this.json
}

resource "aws_route53_record" "root_domain" {
  zone_id = var.aws_route53_hostedzone_id
  name    = var.cloudfront_domain_name
  type    = "A"

  alias {
    name                   = aws_cloudfront_distribution.this.domain_name
    zone_id                = var.aws_route53_hostedzone_id
    evaluate_target_health = false
  }
}
