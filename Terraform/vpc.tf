data "aws_region" "current" {
  provider = aws.hyderabad
}

data "aws_region" "mumbai" {
  provider = aws.mumbai
}

resource "aws_vpc" "vpc" {
  provider             = aws.hyderabad
  cidr_block           = cidrsubnet(var.main_cidr_block, 8, var.aws_region_list_for_cidr[data.aws_region.current.name])
  enable_dns_hostnames = true
  tags = {
    "Name" = "${data.aws_region.current.name}-vpc"
  }
}

data "aws_availability_zones" "azs" {
  provider = aws.hyderabad
  filter {
    name   = "opt-in-status"
    values = ["opt-in-not-required"]
  }
}

resource "aws_subnet" "public_subnets" {
  provider                = aws.hyderabad
  count                   = length(data.aws_availability_zones.azs.names)
  vpc_id                  = aws_vpc.vpc.id
  availability_zone       = data.aws_availability_zones.azs.names[count.index]
  cidr_block              = cidrsubnet(aws_vpc.vpc.cidr_block, 4, count.index)
  map_public_ip_on_launch = true
  tags = {
    "Name" = "${data.aws_region.current.name}-public-subnet-${count.index + 1}"
    "Tier" = "Public"
  }
}

resource "aws_subnet" "private_subnets" {
  provider          = aws.hyderabad
  count             = length(data.aws_availability_zones.azs.names)
  vpc_id            = aws_vpc.vpc.id
  availability_zone = data.aws_availability_zones.azs.names[count.index]
  cidr_block        = cidrsubnet(aws_vpc.vpc.cidr_block, 4, count.index + 8) // to divide the /16 subnet into half/half for public and private
  tags = {
    "Name" = "${data.aws_region.current.name}-private-subnet-${count.index + 1}"
    "Tier" = "Private"
  }
}

resource "aws_internet_gateway" "igw" {
  provider = aws.hyderabad
  vpc_id   = aws_vpc.vpc.id

  tags = {
    "Name" = "${data.aws_region.current.name}-igw"
  }
}

resource "aws_route_table" "public_route_table" {
  provider = aws.hyderabad
  vpc_id   = aws_vpc.vpc.id

  route {
    cidr_block = "0.0.0.0/0"
    gateway_id = aws_internet_gateway.igw.id
  }

  tags = {
    "Name" = "${data.aws_region.current.name}-rtb-public"
  }
}

resource "aws_route_table" "private_route_table" {
  provider = aws.hyderabad
  vpc_id   = aws_vpc.vpc.id

  tags = {
    "Name" = "${data.aws_region.current.name}-rtb-private"
  }
}


resource "aws_route_table_association" "public_subnets_associations" {
  provider       = aws.hyderabad
  count          = length(aws_subnet.public_subnets)
  subnet_id      = aws_subnet.public_subnets[count.index].id
  route_table_id = aws_route_table.public_route_table.id
}

resource "aws_route_table_association" "private_subnets_associations" {
  provider       = aws.hyderabad
  count          = length(aws_subnet.private_subnets)
  subnet_id      = aws_subnet.private_subnets[count.index].id
  route_table_id = aws_route_table.private_route_table.id
}
