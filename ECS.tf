# Specify the AWS provider
provider "aws" {
  region = "us-east-1"  # Change this to your desired region
}

# Create a VPC with subnets and an internet gateway
resource "aws_vpc" "main" {
  cidr_block = "10.0.0.0/16"
}

resource "aws_subnet" "public" {
  vpc_id                  = aws_vpc.main.id
  cidr_block              = "10.0.1.0/24"
  availability_zone       = "us-east-1a"
}

resource "aws_internet_gateway" "gw" {
  vpc_id = aws_vpc.main.id
}

resource "aws_route_table" "public" {
  vpc_id = aws_vpc.main.id
}

resource "aws_route" "public_internet" {
  route_table_id         = aws_route_table.public.id
  destination_cidr_block = "0.0.0.0/0"
  gateway_id             = aws_internet_gateway.gw.id
}

resource "aws_route_table_association" "public_subnet_association" {
  subnet_id      = aws_subnet.public.id
  route_table_id = aws_route_table.public.id
}

# Create a security group for ECS tasks
resource "aws_security_group" "ecs_sg" {
  name_prefix = "ecs-sg-"

  # Allow incoming traffic from the VPC
  vpc_id = aws_vpc.main.id

  # Allow incoming traffic on ports required by your application
  # Example for HTTP:
  ingress {
    description = "HTTP"
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  # Example for HTTPS:
  ingress {
    description = "HTTPS"
    from_port   = 443
    to_port     = 443
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  # Add more ingress rules as needed for your specific application
}

# Create an ECS Fargate cluster
resource "aws_ecs_cluster" "fargate_cluster" {
  name = "my-fargate-cluster"
}

# Create an ECS task definition
resource "aws_ecs_task_definition" "fargate_task_definition" {
  family                = "my-fargate-task"
  container_definitions = <<EOF
  [
    {
      "name": "my-container",
      "image": "your-docker-image-url",
      "portMappings": [
        {
          "containerPort": 80,
          "hostPort": 80,
          "protocol": "tcp"
        }
      ],
      "cpu": 256,
      "memory": 512,
      "essential": true,
      "logConfiguration": {
        "logDriver": "awslogs",
        "options": {
          "awslogs-group": "my-log-group",
          "awslogs-region": "us-east-1",
          "awslogs-stream-prefix": "my-container"
        }
      }
    }
  ]
  EOF

  network_mode = "awsvpc"

  # Task execution role ARN (you can create this role separately)
  execution_role_arn = "arn:aws:iam::ACCOUNT_ID:role/ecs-task-execution-role"

  # Specify the task role ARN if your container requires IAM permissions
  # task_role_arn = "arn:aws:iam::ACCOUNT_ID:role/ecs-task-role"

  requires_compatibilities = ["FARGATE"]
}

# Create an ECS service to run tasks in the Fargate cluster
resource "aws_ecs_service" "fargate_service" {
  name            = "my-fargate-service"
  cluster         = aws_ecs_cluster.fargate_cluster.id
  task_definition = aws_ecs_task_definition.fargate_task_definition.arn
  desired_count   = 1  # Number of tasks you want to run
  launch_type     = "FARGATE"

  # Subnets where your Fargate tasks will run
  network_configuration {
    subnets          = [aws_subnet.public.id]
    security_groups  = [aws_security_group.ecs_sg.id]
    assign_public_ip = true
  }
}
