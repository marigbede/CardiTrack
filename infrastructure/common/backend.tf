terraform {
  backend "gcs" {
    # Bucket and prefix are provided at init time by CI/CD:
    # terraform init -backend-config="bucket=carditrack-tf-state-common" -backend-config="prefix=carditrack/common"
  }
}
