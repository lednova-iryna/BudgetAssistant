# resource "aws_apigatewayv2_authorizer" "jwt_auth" {
#   api_id                            = aws_apigatewayv2_api.http_api.id
#   authorizer_type                   = "JWT"
#   name                              = "http-authorizer"
#   identity_sources                  = toset(["$request.header.authorization"])
#   jwt_configuration                 { 
#     audience = toset(split(",", var.api_gateway_http_api_post_authorizer_audience))
#     issuer = var.api_gateway_http_api_post_authorizer_issuer
#   }
# }
