variable "common_tags" {
  description = "Common tags values"
  default     = {}
}
variable "aws_lambda_architecture" {
  description = "Instruction set architecture for your Lambda function. 'x86_64' or 'arm64'"
}
variable "aws_lambda_name" {
  description = "Name of the lambda function"
}
variable "aws_lambda_memory_size" {
  description = "Amount of memory in MB your Lambda Function can use at runtime."
}
variable "aws_lambda_timeout" {
  description = "Amount of time your Lambda Function has to run in seconds."
}
variable "aws_lambda_docker_image_url" {
  description = "ECR image URI containing the function's deployment package."
}
variable "aws_lambda_parameter_name" {
  description = "AWS Parameter Store variable name used for BE configs distribution."
}
