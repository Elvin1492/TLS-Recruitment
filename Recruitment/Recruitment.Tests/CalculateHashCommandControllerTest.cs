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
		[TestCase("Elvin", "Hello!", "0B00C3A282E7F3BE33B86EC974C8B0CB0A26F4A953034FC5A49B109E6BDA91D5")]
		[TestCase("Asma", "Hello!", "E3E00467F73BFEB48CF2C5E6754E490C736783B76635BA043BA9AEBDA726CAFE")]
		[TestCase("Arzuman", "Hello!", "DE25D5FB07BA4409F525CB0DFACC7BE34A2E7A8B8A0AD442EC8ED58D7AC294B4")]
		public async Task Command_CalculateHash_HashedResult(string login, string password, string assert)
		{
			// Arrange

			var url = "http://WebApiTests.com/api/records";

			var record = new CalculateHashCommand { Login = login, Password = password };

			var request = new HttpRequestMessage { RequestUri = new Uri(url) };
			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			request.Method = HttpMethod.Post;
			request.Content = new ObjectContent<CalculateHashCommand>(record, new JsonMediaTypeFormatter());

			// Act
			var response = await TestClient.PostAsync("http://localhost/api/command/CalculateHashCommand", request.Content);

			var result = JsonConvert.DeserializeObject<HashedResult>(await response.Content.ReadAsStringAsync());

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			result.Result.ToUpper().Should().Be(assert);
		}
	}
}