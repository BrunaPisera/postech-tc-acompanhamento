provider "helm" {
  kubernetes {
    host                   = data.aws_eks_cluster.tc_eks_cluster.endpoint
    cluster_ca_certificate = base64decode(data.aws_eks_cluster.tc_eks_cluster.certificate_authority[0].data)
    exec {
      api_version = "client.authentication.k8s.io/v1beta1"
      args        = ["eks", "get-token", "--cluster-name", "eks_lanchonete-do-bairro"]
      command     = "aws"
    }
  }
}

resource "helm_release" "acompanhamento" {
  name             = "acompanhamento"
  namespace        = "dev"
  create_namespace = true
  chart            = "../helm/acompanhamento-chart"

  values = [
    file("../helm/acompanhamento-chart/values.yaml"),
    file("../helm/acompanhamento-chart/values-dev.yaml")
  ]

  set {
    name  = "configmap.data.DB_HOST"
    value = var.rdsHost
  }

  set {
    name  = "rabbitmq.password"
    value = var.brokerpassword
  }
}