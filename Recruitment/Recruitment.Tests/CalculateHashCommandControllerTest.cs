using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using NUnit.Framework;
using Recruitment.Contracts.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Recruitment.Tests
{
	public class CalculateHashCommandControllerTest : IntegrationTest
	{
		[Test]
		[TestCase()]
		public async Task Command_CalculateHash_HashedResult()
		{
			// Arrange

			var url = "http://WebApiTests.com/api/records";

			var record = new CalculateHashCommand { Login = "Elvin", Password = "Hello!" };

			var request = new HttpRequestMessage { RequestUri = new Uri(url) };
			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			request.Method = HttpMethod.Post;
			request.Content = new ObjectContent<CalculateHashCommand>(record, new JsonMediaTypeFormatter());

			// Act
			var response = await TestClient.PostAsync("http://localhost/api/command/CalculateHashCommand", request.Content);

			var result = JsonConvert.DeserializeObject<HashedResult>(await response.Content.ReadAsStringAsync());

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			result.Result.ToUpper().Should().Be("0B00C3A282E7F3BE33B86EC974C8B0CB0A26F4A953034FC5A49B109E6BDA91D5");
		}
	}
}