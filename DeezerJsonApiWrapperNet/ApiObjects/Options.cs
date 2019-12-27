using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DeezerJsonApiWrapperNet.ApiObjects
{
	public class Options : JsonDeserialized
	{
		[JsonProperty(ApiConsts.Options.CanUseStreaming)]
		public bool CanUseStreaming { get; set; }

		[JsonProperty(ApiConsts.Options.StreamingDuration)]
		public long StreamingDuration { get; set; }

		[JsonProperty(ApiConsts.Options.CanListenOffline)]
		public bool CanListenOffline { get; set; }

		[JsonProperty(ApiConsts.Options.CanListenHq)]
		public bool CanListenHq { get; set; }

		[JsonProperty(ApiConsts.Options.HasVisualAds)]
		public bool HasVisualAds { get; set; }

		[JsonProperty(ApiConsts.Options.HasAudioAds)]
		public bool HasAudioAds { get; set; }

		[JsonProperty(ApiConsts.Options.HasReachedDeviceCountLimit)]
		public bool HasReachedDeviceCountLimit { get; set; }

		[JsonProperty(ApiConsts.Options.CanSubscribe)]
		public bool CanSubscribe { get; set; }

		[JsonProperty(ApiConsts.Options.RadioSkipsLimit)]
		public long RadioSkipsLimit { get; set; }

		[JsonProperty(ApiConsts.Options.CanListenLossless)]
		public bool CanListenLossless { get; set; }

		[JsonProperty(ApiConsts.Options.CanUsePreview)]
		public bool CanUsePreview { get; set; }

		[JsonProperty(ApiConsts.Options.CanUseRadio)]
		public bool CanUseRadio { get; set; }

		public override async Task LoadAsync()
		{
			throw new NotImplementedException();
		}
	}
}
