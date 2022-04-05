using Microsoft.AspNetCore.Mvc.Testing;
using Recruitment.Api;
using System.Net.Http;

namespace Recruitment.Tests
{
	public class IntegrationTest
	{
		protected readonly HttpClient TestClient;

		public IntegrationTest()
		{
			var appFactory = new WebApplicationFactory<Startup>();
			TestClient = appFactory.CreateClient();
		}
	}
}