resource "aws_s3_bucket" "this" {
  bucket = var.bucket_name
  tags = merge(
    var.common_tags,
    {
      project = "assistants-budget-ui"
    }
  )
}