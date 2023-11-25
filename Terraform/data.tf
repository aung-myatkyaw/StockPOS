data "aws_ssm_parameter" "ubuntu_image" {
  provider = aws.hyderabad
  name     = replace("/aws/service/canonical/ubuntu/server/22.04/stable/current/{{MACHINE_TYPE}}/hvm/ebs-gp2/ami-id", "{{MACHINE_TYPE}}", var.machine_type)
}

# Get the Subnets in Region
data "aws_subnets" "public_subnets_data" {
  provider = aws.hyderabad
  depends_on = [
    aws_subnet.public_subnets
  ]
  filter {
    name   = "vpc-id"
    values = [aws_vpc.vpc.id]
  }
  filter {
    name   = "tag:Tier"
    values = ["Public"]
  }
}

resource "random_shuffle" "subnet_for_stockpos" {
  input        = data.aws_subnets.public_subnets_data.ids
  result_count = 1
}
