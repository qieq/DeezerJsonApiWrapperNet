using System.Diagnostics;
using Newtonsoft.Json;

namespace DeezerJsonApiWrapperNet.ApiObjects
{
	[DebuggerDisplay("{ContryName}")]
	public class Infos
	{
		[JsonProperty(ApiConsts.Infos.CountryIso)]
		public string CountryIso { get; set; }

		[JsonProperty(ApiConsts.Infos.CountryName)]
		public string CountryName { get;set; }

		[JsonProperty(ApiConsts.Infos.IsDeezerAvailable)]
		public bool IsDeezerAvailable { get; set; }

		[JsonProperty(ApiConsts.Infos.Offers)]
		public string Offers { get; set; }
	}
}
