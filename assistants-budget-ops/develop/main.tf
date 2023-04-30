module "s3" {
  source                              = "../modules/assistants-budget-ui-s3"
  common_tags = local.common_tags
  bucket_name = "assistants-budget-ui-s3-${local.common_tags.environment}"
}