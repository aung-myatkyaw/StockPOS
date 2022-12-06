data "aws_ssm_parameter" "ubuntu_image" {
  provider = aws.mumbai
  name     = "/aws/service/canonical/ubuntu/server/20.04/stable/current/arm64/hvm/ebs-gp2/ami-id"
}
