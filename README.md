# STOCK Management API with AWS Infrastructure Setup

This project is aimed at providing API endpoints for stock management, implemented using .NET 6.0 framework. Additionally, it includes Terraform configurations for setting up all AWS cloud resources required for the project to run seamlessly. The deployment utilizes AWS CodeDeploy to deploy the Docker image built from the source code, orchestrated with Docker Compose, onto an EC2 instance. The build process is handled by AWS CodeBuild using predefined scripts in the `STOCKPOS/Deploy/buildspec.yml`.

## Project Structure

The project is divided into three main components:

1. **Stock Management API**: This contains the .NET 6.0 application responsible for handling stock management operations through API endpoints.

2. **Terraform Configurations**: This directory includes `Terraform` configurations to provision all AWS cloud resources necessary for the project, including VPC, subnets, security groups, IAM, EC2 instance, RDS, S3 buckets, CodePipeline, CloudWatch monitoring/alarms and SNS.

3. **AWS CodeBuild Configuration**: This project includes a `STOCKPOS/Deploy/buildspec.yml` file defining the build steps for AWS CodeBuild to compile the project and prepare it for deployment.

## Requirements

To run this project, you need:

- .NET 6.0 SDK installed on your local machine.
- Terraform installed on your local machine.
- An AWS account with appropriate permissions to provision resources.
- AWS CodeBuild service configured with the necessary permissions to build the project.

## Getting Started

1. **Clone the Repository**: 
    ```bash
    git clone git@github.com:aung-myatkyaw/StockPOS.git
    ```

2. **Set Up AWS Credentials**:
    Ensure that you have AWS credentials configured on your machine with appropriate permissions.

3. **Provision AWS Infrastructure**:
    Navigate to the `Terraform` directory and run:
    ```bash
    terraform init
    terraform plan
    terraform apply
    ```

4. **Build and Deploy the API**:
    - AWS CodeBuild will automatically trigger the build process based on the `STOCKPOS/Deploy/buildspec.yml` configuration.
    - Once the build is successful, AWS CodeDeploy will deploy the application using predefined configurations onto the provisioned EC2 instance.

5. **Access the API**:
    Once the deployment is successful, you can access the API endpoints via the public IP address of the EC2 instance.

## Additional Notes

- Ensure that you review and customize the Terraform configurations (`Terraform/*.tf`) according to your specific requirements, such as region, instance type, etc.
- Make sure to configure Docker Compose (`docker-compose.yml`) and AWS CodeDeploy (`STOCKPOS/Deploy/*`) configurations as per your project needs.
- For security purposes, always keep your AWS credentials and sensitive information secure and avoid committing them to version control.

## Support

For any issues or questions, feel free to raise an issue on the GitHub repository or contact the project maintainers directly.

