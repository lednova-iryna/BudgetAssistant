terraform {
  cloud {
    organization = "lednov"

    workspaces {
      name = "assistants-budget"
    }
  }
  required_providers {
    aws = {
      version = "5.0.0"
    }
  }

}

provider "aws" {
  region = local.aws_region
}
