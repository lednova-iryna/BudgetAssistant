terraform {
  cloud {
    organization = "lednov"

    workspaces {
      name = local.common_tags.product
    }
  }
  required_providers {
    aws = {
      version = "4.65.0"
    }
  }

}

provider "aws" {
  region = local.aws_region
}
