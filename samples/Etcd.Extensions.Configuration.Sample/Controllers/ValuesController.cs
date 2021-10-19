using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace Etcd.Sample.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ValuesController : ControllerBase
	{
		private readonly IConfiguration _configuration;

		public ValuesController(IConfiguration configuration)
		{
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}

		[HttpGet("{key}")]
		public ActionResult<string> Get(string key)
		{
			return _configuration[key];
		}

		[HttpPut("{key}")]
		public void Put(string key, [FromBody] string value)
		{
			_configuration[key] = value;
		}
	}
}
