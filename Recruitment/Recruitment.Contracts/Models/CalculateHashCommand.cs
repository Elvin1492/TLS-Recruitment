using CommandQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recruitment.Contracts.Models
{
	public class CalculateHashCommand : ICommand<HashedResult>
	{
		public string Login { get; set; }
		public string Password { get; set; }
	}
}
