using CommandQuery;
using Recruitment.Contracts.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Recruitment.Contracts.Handlers
{
	public class CalculateHashCommandHandler : ICommandHandler<CalculateHashCommand, HashedResult>
	{
		public async Task<HashedResult> HandleAsync(CalculateHashCommand command, CancellationToken cancellationToken)
		{
			var hashedResult = new HashedResult();

			StringBuilder Sb = new StringBuilder();

			using (var hash = SHA256.Create())
			{
				Encoding enc = Encoding.UTF8;

				using MemoryStream stream = new MemoryStream(enc.GetBytes(command.Login + command.Password));

				var result = await hash.ComputeHashAsync(stream, cancellationToken);

				foreach (var b in result)
				{
					Sb.Append(b.ToString("x2"));
				}
			}

			hashedResult.Result = Sb.ToString();

			return hashedResult;
		}
	}
}
