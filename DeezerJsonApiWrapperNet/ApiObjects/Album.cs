using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using DeezerJsonApiWrapperNet;
using DeezerJsonApiWrapperNet.ApiConsts;
using DeezerJsonApiWrapperNet.ApiObjects.Enums;
using Newtonsoft.Json;

namespace DeezerJsonApiWrapperNet.ApiObjects
{
	[DebuggerDisplay("{Title}")]
	public class Album : JsonDeserialized, IDefaultSelfProvider<Album>
	{
		#region Properties
		[JsonProperty(ApiConsts.Album.Id)]
		public long Id { get; set; }

		[JsonProperty(ApiConsts.Album.Title)]
		public string Title { get; set; }

		[JsonProperty(ApiConsts.Album.Upc)]
		public string Upc { get; set; }

		[JsonProperty(ApiConsts.Album.Link)]
		public Uri Link { get; set; }

		[JsonProperty(ApiConsts.Album.ShareLink)]
		public Uri ShareLink { get; set; }

		[JsonProperty(ApiConsts.Album.CoverUrl)]
		public Uri CoverUrl { get; set; }

		[JsonProperty(ApiConsts.Album.CoverSmallUrl)]
		public Uri CoverSmallUrl { get; set; }

		[JsonProperty(ApiConsts.Album.CoverMediumUrl)]
		public Uri CoverMediumUrl { get; set; }

		[JsonProperty(ApiConsts.Album.CoverBigUrl)]
		public Uri CoverBigUrl { get; set; }

		[JsonProperty(ApiConsts.Album.CoverXlUrl)]
		public Uri CoverXlUrl { get; set; }

		[JsonProperty(ApiConsts.Album.GenreId)]
		public int Genre { get; set; }

		[JsonIgnore]
		public List<Genre> Genres { get; set; }

		[JsonProperty(ApiConsts.Album.Label)]
		public string Label { get; set; }

		[JsonProperty(ApiConsts.Album.Duration)]
		public long Duration { get; set; }

		[JsonProperty(ApiConsts.Album.FansCount)]
		public long FansCount { get; set; }

		[JsonIgnore]
		public List<User> Fans { get; set; }

		[JsonProperty(ApiConsts.Album.Rating)]
		public int Rating { get; set; }

		[JsonProperty(ApiConsts.Album.ReleaseDate)]
		public DateTime ReleaseDate { get; set; }

		[JsonProperty(ApiConsts.Album.RecordType)]
		private string _recordType;
		[JsonIgnore]
		public RecordType RecordType => Parser.Parse<RecordType>(_recordType);

		[JsonProperty(ApiConsts.Album.Available)]
		public bool Available { get; set; }

		[JsonProperty(ApiConsts.Album.Alternative)]
		public Album Alternative { get; set; }

		[JsonProperty(ApiConsts.Album.TracklistUrl)]
		public Uri TracklistUrl { get; set; }

		[JsonProperty(ApiConsts.Album.ExplicitLyrics)]
		public bool ExplicitLyrics { get; set; }

		[JsonIgnore]
		public List<Contributor> Contributors { get; set; }

		[JsonProperty(ApiConsts.Album.Artist)]
		public Artist Artist { get; set; }

		[JsonIgnore]
		public List<Track> Tracks { get; set; }

		[JsonIgnore]
		public List<Comment> Comments { get; set; }

		#endregion

		public override void Init(string jsonContent, IRuntime runtime)
		{
			base.Init(jsonContent, runtime);
			UriPrefix = $"{ApiConsts.Album.Self}/{Id}/";
		}

		public override async Task LoadAsync()
		{
			JsonContent = await Runtime.ExecuteHttpGetAsync(UriPrefix);

			Tracks = await Runtime.DeserializeEntitiesFromData<Track>(
				await Runtime.ExecuteHttpGetAsync(TracklistUrl.AbsolutePath));
			Artist = await Runtime.DeserializeEntityFromSection<Artist>(JsonContent, ApiConsts.Album.Artist);
			Comments = await Runtime.DeserializeEntitiesFromData<Comment>(await SendGetAsync(ApiConsts.Album.Comments));
			Fans = await Runtime.DeserializeEntitiesFromData<User>(await SendGetAsync(ApiConsts.Album.Fans));
			Contributors =
				await Runtime.DeserializeEntitiesFromSection<Contributor>(JsonContent, ApiConsts.Album.Contributors);
			Genres = await Runtime.DeserializeEntitiesFromSectionData<Genre>(JsonContent, ApiConsts.Album.Genres);
		}

		public async Task Rate(int note)
		{
			if (note < 1 || note > 5)
				throw new ArgumentException("Rating must be in range from 1 to 5");

			await SendPostAsync(string.Empty, "note", note.ToString());
		}

		public async Task AddComment(string comment)
		{
			await SendPostAsync(ApiConsts.Album.Comments, "comment", comment);
		}

		public Album CreateDefault()
		{
			var unknownAlbum = new Album();
			unknownAlbum.Artist = Artist.CreateDefault();
			unknownAlbum.Comments = new List<Comment>();
			unknownAlbum.Contributors = new List<Contributor>();
			unknownAlbum.Fans = new List<User>();
			unknownAlbum.Genres = new List<Genre>();
			unknownAlbum.Tracks = new List<Track>();

			return unknownAlbum;
		}
	}
}