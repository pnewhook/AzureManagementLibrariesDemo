using System;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.Compute;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.WindowsAzure.Management.Compute.Models;

namespace AzurePro.Core
{
	public class Swapper
	{
		ComputeManagementClient _client;

		public Swapper ()
		{
			string certBase64String = ConfigurationManager.AppSettings["certificate"];
			var cert = new X509Certificate2(Convert.FromBase64String(certBase64String));
			string subscriptionId = ConfigurationManager.AppSettings["subscriptionId"];
			SubscriptionCloudCredentials credentials = new CertificateCloudCredentials(subscriptionId, cert);
			_client = CloudContext.Clients.CreateComputeManagementClient(credentials);
		}

		public async Task SwapCloudService(string siteName)
		{
			var detailed = await _client.HostedServices.GetDetailedAsync(siteName);
			string stagingName = detailed.Deployments.Single(d => d.DeploymentSlot == DeploymentSlot.Staging).Name;
			_client.Deployments.SwapAsync(siteName, new DeploymentSwapParameters{ SourceDeployment = stagingName });
		}
	}
}

