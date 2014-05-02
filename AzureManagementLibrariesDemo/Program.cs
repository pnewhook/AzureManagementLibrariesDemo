using System;
using Nito.AsyncEx;
using AzuremanagementLibrariesDemo;


namespace AzureManagementLibrariesDemo
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			var swapper = new Swapper();
			return AsyncContext.Run(() => swapper.SwapWebSiteAsync("swap-test", "swap-test-staging", "swap-test"));
		}
	}
}
