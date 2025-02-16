terraform {
  backend "s3" {
    bucket = "tc-tf-acompanhamento"
    key    = "backend/terraform.tfstate"
    region = "us-east-1"
  }
}