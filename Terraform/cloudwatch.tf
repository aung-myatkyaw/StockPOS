resource "aws_cloudwatch_dashboard" "main" {
  provider       = aws.hyderabad
  dashboard_name = "StockPOS"
  dashboard_body = jsonencode(
    {
      widgets = [
        {
          height = 6
          properties = {
            legend = {
              position = "bottom"
            }
            liveData = false
            metrics = [
              [
                "AWS/EC2",
                "CPUUtilization",
                "InstanceId",
                aws_instance.stockpos_staging.id
              ],
            ]
            period   = 60
            region   = "ap-south-1"
            stacked  = true
            stat     = "Average"
            timezone = "+0630"
            title    = "StockPOS's CPU Utilization Graph"
            view     = "timeSeries"
          }
          type  = "metric"
          width = 8
          x     = 0
          y     = 0
        },
        {
          height = 6
          properties = {
            metrics = [
              [
                "CWAgent",
                "mem_used_percent",
                "InstanceId",
                aws_instance.stockpos_staging.id,
                {
                  color = "#ff7f0e"
                },
              ],
            ]
            period   = 60
            region   = "ap-south-1"
            stacked  = true
            stat     = "Average"
            timezone = "+0630"
            title    = "StockPOS's Memory Usage Percent"
            view     = "timeSeries"
          }
          type  = "metric"
          width = 8
          x     = 8
          y     = 0
        },
        {
          height = 6
          properties = {
            metrics = [
              [
                "CWAgent",
                "disk_used_percent",
                "InstanceId",
                aws_instance.stockpos_staging.id,
                {
                  color = "#2ca02c"
                },
              ],
            ]
            period   = 300
            region   = "ap-south-1"
            stacked  = true
            stat     = "Average"
            timezone = "+0630"
            title    = "StockPOS's Disk Usage Percent"
            view     = "timeSeries"
          }
          type  = "metric"
          width = 8
          x     = 16
          y     = 0
        },
        {
          height = 6
          properties = {
            metrics = [
              [
                "AWS/EC2",
                "CPUCreditBalance",
                "InstanceId",
                aws_instance.stockpos_staging.id,
                {
                  color = "#17becf"
                },
              ],
            ]
            period    = 300
            region    = "ap-south-1"
            sparkline = true
            stacked   = true
            stat      = "Average"
            timezone  = "+0630"
            title     = "StockPOS's CPU Credit Balance"
            view      = "timeSeries"
          }
          type  = "metric"
          width = 8
          x     = 0
          y     = 6
        },
        {
          height = 6
          properties = {
            metrics = [
              [
                "AWS/RDS",
                "CPUUtilization",
                "DBInstanceIdentifier",
                aws_db_instance.stockpos_db_instance.identifier
              ],
            ]
            period    = 60
            region    = "ap-south-1"
            sparkline = true
            stacked   = true
            stat      = "Average"
            timezone  = "+0630"
            title     = "RDS CPU Utilization Graph"
            view      = "timeSeries"
          }
          type  = "metric"
          width = 8
          x     = 8
          y     = 6
        },
        {
          height = 6
          properties = {
            metrics = [
              [
                "AWS/RDS",
                "CPUCreditBalance",
                "DBInstanceIdentifier",
                aws_db_instance.stockpos_db_instance.identifier,
                {
                  color = "#17becf"
                },
              ],
            ]
            period    = 300
            region    = "ap-south-1"
            sparkline = true
            stacked   = true
            stat      = "Average"
            timezone  = "+0630"
            title     = "RDS CPU Credit Balance"
            view      = "timeSeries"
          }
          type  = "metric"
          width = 8
          x     = 16
          y     = 6
        }
      ]
    }
  )
}
