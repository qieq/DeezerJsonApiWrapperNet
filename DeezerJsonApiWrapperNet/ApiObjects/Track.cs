using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading.Tasks;
using DeezerJsonApiWrapperNet;
using Newtonsoft.Json;

namespace DeezerJsonApiWrapperNet.ApiObjects
{
	[DebuggerDisplay("{Artist.Name} - {Title}")]
	public class Track : JsonDeserialized, IDefaultSelfProvider<Track>
	{
		[JsonProperty(ApiConsts.Track.Id)] public long Id { get; set; }

		[JsonProperty(ApiConsts.Track.IsReadable)]
		public bool IsReadable { get; set; }

		[JsonProperty(ApiConsts.Track.Title)] public string Title { get; set; }

		[JsonProperty(ApiConsts.Track.ShortTitle)]
		public string ShortTitle { get; set; }

		[JsonProperty(ApiConsts.Track.TrackVersion)]
		public string TrackVersion { get; set; }

		[JsonProperty(ApiConsts.Track.IsUnseen)]
		public bool IsUnseen { get; set; }

		[JsonProperty(ApiConsts.Track.Link)] public Uri Link { get; set; }

		[JsonProperty(ApiConsts.Track.ShareLink)]
		public Uri ShareLink { get; set; }

		[JsonProperty(ApiConsts.Track.Duration)]
		public long Duration { get; set; }

		[JsonProperty(ApiConsts.Track.Rank)] public long Rank { get; set; }

		[JsonProperty(ApiConsts.Track.HasExplicitLyrics)]
		public bool HasExplicitLyrics { get; set; }

		[JsonProperty(ApiConsts.Track.PreviewUrl)]
		public Uri PreviewUrl { get; set; }

		[JsonProperty(ApiConsts.Track.DateAdded)]
		private long _timeAdded;

		public DateTime DateAdded => new DateTime(_timeAdded);

		[JsonIgnore] public Artist Artist { get; set; }

		[JsonIgnore] public Album Album { get; set; }

		[JsonProperty(ApiConsts.Track.Isrc)] public string Isrc { get; set; }

		[JsonProperty(ApiConsts.Track.AlbumPosition)]
		public long AlbumPosition { get; set; }

		[JsonProperty(ApiConsts.Track.DiskNumber)]
		public long DiskNumber { get; set; }

		[JsonProperty(ApiConsts.Track.ReleaseDate)]
		public DateTime ReleaseDate { get; set; }

		[JsonProperty(ApiConsts.Track.Bpm)] public float Bpm { get; set; }

		[JsonProperty(ApiConsts.Track.Gain)] public float Gain { get; set; }

		[JsonProperty(ApiConsts.Track.AvailableIn)]
		// TODO: add array
		public string AvailableIn { get; set; }

		[JsonProperty(ApiConsts.Track.Alternative)]
		public Track Alternative { get; set; }

		[JsonIgnore] public List<Contributor> Contributors { get; private set; }

		public override async void Init(string jsonContent, IRuntime runtime)
		{
			base.Init(jsonContent, runtime);
			UriPrefix = $"{ApiConsts.Track.Self}/{Id}/";
		}

		public override async Task LoadAsync()
		{
			JsonContent = await Runtime.ExecuteHttpGetAsync(UriPrefix);

			Artist = await Runtime.DeserializeEntityFromSection<Artist>(JsonContent, ApiConsts.Track.Artist);
			Album = await Runtime.DeserializeEntityFromSection<Album>(JsonContent, ApiConsts.Track.Album);
			Contributors =
				await Runtime.DeserializeEntitiesFromSection<Contributor>(JsonContent, ApiConsts.Album.Contributors);
		}

		public Track CreateDefault()
		{
			var unknownTrack = new Track();
			unknownTrack.Artist = Artist.CreateDefault();
			unknownTrack.Album = Album.CreateDefault();
			unknownTrack.AlbumPosition = -1;
			unknownTrack.Contributors = new List<Contributor>();

			return unknownTrack;
		}
	}
}
