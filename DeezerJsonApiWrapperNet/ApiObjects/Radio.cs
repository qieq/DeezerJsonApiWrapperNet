using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DeezerJsonApiWrapperNet.ApiObjects
{
	[DebuggerDisplay("{Title}")]
	public class Radio : JsonDeserialized
	{
		[JsonProperty(ApiConsts.Radio.Id)]
		public long Id { get; set; }

		[JsonProperty(ApiConsts.Radio.Title)]
		public string Title { get; set; }

		[JsonProperty(ApiConsts.Radio.Description)]
		public string Description { get; set; }

		[JsonProperty(ApiConsts.Radio.ShareUrl)]
		public Uri ShareUrl { get; set; }

		[JsonProperty(ApiConsts.Radio.PictureUrl)]
		public Uri PictureUrl { get; set; }

		[JsonProperty(ApiConsts.Radio.PictureSmallUrl)]
		public Uri PictureSmallUrl { get; set; }

		[JsonProperty(ApiConsts.Radio.PictureMediumUrl)]
		public Uri PictureMediumUrl { get; set; }

		[JsonProperty(ApiConsts.Radio.PictureBigUrl)]
		public Uri PictureBigUrl { get; set; }

		[JsonProperty(ApiConsts.Radio.PictureXlUrl)]
		public Uri PictureXlUrl { get; set; }

		[JsonProperty(ApiConsts.Radio.TracklistUrl)]
		public Uri TracklistUrl { get; set; }

		public override void Init(string jsonContent, IRuntime runtime)
		{
			base.Init(jsonContent, runtime);
			UriPrefix = $"{ApiConsts.Radio.Self}/{Id}/";
		}

		public override Task LoadAsync()
		{
			throw new NotImplementedException();
		}
	}
}
