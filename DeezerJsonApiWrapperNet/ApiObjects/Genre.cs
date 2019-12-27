using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DeezerJsonApiWrapperNet.ApiObjects
{
	[DebuggerDisplay("{Name}")]
	public class Genre : JsonDeserialized
	{
		[JsonProperty(ApiConsts.Genre.Id)]
		public long Id { get; set; }

		[JsonProperty(ApiConsts.Genre.Name)]
		public string Name { get; set; }

		[JsonProperty(ApiConsts.Genre.PictureUrl)]
		public Uri PictureUrl { get; set; }

		[JsonProperty(ApiConsts.Genre.PictureSmallUrl)]
		public Uri PictureSmallUrl { get; set; }

		[JsonProperty(ApiConsts.Genre.PictureMediumUrl)]
		public Uri PictureMediumUrl { get; set; }

		[JsonProperty(ApiConsts.Genre.PictureBigUrl)]
		public Uri PictureBigUrl { get; set; }

		[JsonProperty(ApiConsts.Genre.PictureXlUrl)]
		public Uri PictureXlUrl { get; set; }

		public override void Init(string jsonContent, IRuntime runtime)
		{
			base.Init(jsonContent, runtime);
			UriPrefix = $"{ApiConsts.Genre.Self}/{Id}/";
		}

		public override async Task LoadAsync()
		{
			JsonContent = await Runtime.ExecuteHttpGetAsync(UriPrefix);
		}
	}
}
