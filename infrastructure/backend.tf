terraform {
  backend "gcs" {
    # Bucket and prefix are provided at init time by CI/CD:
    # terraform init -backend-config="bucket=<bucket>" -backend-config="prefix=carditrack/<env>"
  }
}
