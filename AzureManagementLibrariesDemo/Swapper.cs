using System;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.Compute;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.WindowsAzure.Management.Compute.Models;
using Microsoft.WindowsAzure.Management.WebSites;
using Microsoft.WindowsAzure.Management.WebSites.Models;

namespace AzuremanagementLibrariesDemo
{
	public class Swapper
	{
		SubscriptionCloudCredentials _credentials;
		public Swapper ()
		{
			string certBase64String = ConfigurationManager.AppSettings["certificate"];
			var cert = new X509Certificate2(Convert.FromBase64String(certBase64String));
			string subscriptionId = ConfigurationManager.AppSettings["subscriptionId"];
			_credentials = new CertificateCloudCredentials(subscriptionId, cert);

		}

		public async Task SwapCloudServiceAsync(string serviceName)
		{
			ComputeManagementClient client = CloudContext.Clients.CreateComputeManagementClient(_credentials);
			var detailed = await client.HostedServices.GetDetailedAsync(serviceName);
			string stagingName = detailed.Deployments.Single(d => d.DeploymentSlot == DeploymentSlot.Staging).Name;
			await client.Deployments.SwapAsync(serviceName, new DeploymentSwapParameters{ SourceDeployment = stagingName });
		}

		public async Task SwapWebSiteAsync(string siteName, string sourceSite, string targetSlot)
		{
			WebSiteManagementClient client =  CloudContext.Clients.CreateWebSiteManagementClient(_credentials);
			// the management library is very fond of static string constants
			await client.WebSites.SwapSlotsAsync(WebSpaceNames.EastUSWebSpace, siteName, sourceSite, targetSlot);
		}
	}
}

