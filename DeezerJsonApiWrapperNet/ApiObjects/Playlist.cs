using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace DeezerJsonApiWrapperNet.ApiObjects
{
	[DebuggerDisplay("{Title}")]
	public class Playlist : JsonDeserialized
	{
		[JsonProperty(ApiConsts.Playlist.Id)]
		public long Id { get; set; }

		[JsonProperty(ApiConsts.Playlist.Title)]
		public string Title { get; set; }

		[JsonProperty(ApiConsts.Playlist.Description)]
		public string Description { get; set; }

		[JsonProperty(ApiConsts.Playlist.Duration)]
		public long Duration { get; set; }

		[JsonProperty(ApiConsts.Playlist.IsPublic)]
		public bool IsPublic { get; set; }

		[JsonProperty(ApiConsts.Playlist.IsFavoriteTracksPlaylist)]
		public bool IsFavoriteTracksPlaylist { get; set; }

		[JsonProperty(ApiConsts.Playlist.IsCollaborative)]
		public bool IsCollaborative { get; set; }

		[JsonProperty(ApiConsts.Playlist.Rating)]
		public long Rating { get; set; }

		[JsonProperty(ApiConsts.Playlist.TracksCount)]
		public long TracksCount { get; set; }

		[JsonProperty(ApiConsts.Playlist.UnseenTracksCount)]
		public long UnseenTracksCount { get; set; }

		[JsonProperty(ApiConsts.Playlist.FansCount)]
		public long FansCount { get; set; }

		[JsonProperty(ApiConsts.Playlist.Link)]
		public Uri Link { get; set; }

		[JsonProperty(ApiConsts.Playlist.ShareLink)]
		public Uri ShareLink { get; set; }

		[JsonProperty(ApiConsts.Playlist.PictureUrl)]
		public Uri PictureUrl { get; set; }

		[JsonProperty(ApiConsts.Playlist.PictureSmallUrl)]
		public Uri PictureSmallUrl { get; set; }

		[JsonProperty(ApiConsts.Playlist.PictureMediumUrl)]
		public Uri PictureMediumUrl { get; set; }

		[JsonProperty(ApiConsts.Playlist.PictureBigUrl)]
		public Uri PictureBigUrl { get; set; }

		[JsonProperty(ApiConsts.Playlist.PictureXlUrl)]
		public Uri PictureXlUrl { get; set; }

		[JsonProperty(ApiConsts.Playlist.Checksum)]
		public string Checksum { get; set; }

		[JsonProperty(ApiConsts.Playlist.Creator)]
		public User Creator { get; set; }

		[JsonProperty(ApiConsts.Playlist.Tracks)]
		public List<Track> Tracks { get; set; }

		public override async void Init(string jsonContent, IRuntime runtime)
		{
			base.Init(jsonContent, runtime);
			UriPrefix = $"{ApiConsts.Playlist.Self}/{Id}/";
		}

		public override async Task LoadAsync()
		{
			JsonContent = await Runtime.ExecuteHttpGetAsync(UriPrefix);

			var tracksRequest = ApiConsts.Playlist.Tracks;
			PagedList<Track> tracksPortion;
			Tracks = new List<Track>();

			do
			{
				string queryString = new Uri(tracksRequest).Query;
				var queryDictionary = HttpUtility.ParseQueryString(queryString);

				var nextIndexParameter = new[] { $"index={queryDictionary.Get("index")}" };
				tracksPortion = await Runtime.DeserializeEntitiesFromData<Track>(await SendGetAsync(ApiConsts.Playlist.Tracks, nextIndexParameter));
				Tracks.AddRange(tracksPortion);
				tracksRequest = tracksPortion.NextUri;
			}
			while (!string.IsNullOrEmpty(tracksRequest));

		}

		public async Task AddComment(int rating)
		{
			await SendPostAsync(ApiConsts.Playlist.Self, "note", rating);
		}
	}
}
