data "aws_ssm_parameter" "ubuntu_image" {
  provider = aws.mumbai
  name     = "/aws/service/canonical/ubuntu/server/20.04/stable/current/amd64/hvm/ebs-gp2/ami-id"
}

# Get the Subnets in Mumbai Region
data "aws_subnets" "mumbai_public_subnets_data" {
  provider = aws.mumbai
  depends_on = [
    aws_subnet.mumbai_public_subnets
  ]
  filter {
    name   = "vpc-id"
    values = [aws_vpc.mumbai_vpc.id]
  }
  filter {
    name   = "tag:Tier"
    values = ["Public"]
  }
}

resource "random_shuffle" "subnet_for_stockpos" {
  input        = data.aws_subnets.mumbai_public_subnets_data.ids
  result_count = 1
}
