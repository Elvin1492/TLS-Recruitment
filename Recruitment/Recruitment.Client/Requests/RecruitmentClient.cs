using Recruitment.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace Recruitment.Client.Requests
{
	public class RecruitmentClient : IRecruitmentClient
	{
		public async Task<HashedResult> CalculateHashCommandRequestAsync(CalculateHashCommand command)
		{ 
			var json = JsonConvert.SerializeObject(command);
			var data = new StringContent(json, Encoding.UTF8, "application/json");

			var url = "http://localhost:28330";
			using var client = new HttpClient();

			var response = await client.PostAsync(url, data);

			var result = await response.Content.ReadAsStringAsync();
			var hashedResult = JsonConvert.DeserializeObject<HashedResult>(result);

			return hashedResult;
		}
	}

	public interface IRecruitmentClient
	{
		Task<HashedResult> CalculateHashCommandRequestAsync(CalculateHashCommand command);
	}
}
