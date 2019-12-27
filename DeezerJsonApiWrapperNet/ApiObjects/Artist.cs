using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using DeezerJsonApiWrapperNet;
using Newtonsoft.Json;

namespace DeezerJsonApiWrapperNet.ApiObjects
{
	[DebuggerDisplay("{Name}")]
	public class Artist : JsonDeserialized, IDefaultSelfProvider<Artist>
	{
		#region Properties
		[JsonProperty(ApiConsts.Artist.Id)]
		public long Id { get; set; }

		[JsonProperty(ApiConsts.Artist.Name)]
		public string Name { get; set; }

		[JsonProperty(ApiConsts.Artist.Link)]
		public Uri Link { get; set; }

		[JsonProperty(ApiConsts.Artist.ShareLink)]
		public Uri ShareLink { get; set; }

		[JsonProperty(ApiConsts.Artist.PictureUrl)]
		public Uri PictureUrl { get; set; }

		[JsonProperty(ApiConsts.Artist.PictureSmallUrl)]
		public Uri PictureSmallUrl { get; set; }

		[JsonProperty(ApiConsts.Artist.PictureMediumUrl)]
		public Uri PictureMediumUrl { get; set; }

		[JsonProperty(ApiConsts.Artist.PictureBigUrl)]
		public Uri PictureBigUrl { get; set; }

		[JsonProperty(ApiConsts.Artist.PictureXlUrl)]
		public Uri PictureXlUrl { get; set; }

		[JsonProperty(ApiConsts.Artist.AlbumsCount)]
		public long AlbumsCount { get; set; }

		[JsonProperty(ApiConsts.Artist.FansCount)]
		public long FansCount { get; set; }

		[JsonProperty(ApiConsts.Artist.HasRadio)]
		public bool HasRadio { get; set; }

		[JsonProperty(ApiConsts.Artist.TopTracksUrl)]
		public Uri TopTracksUrl { get; set; }

		public List<Album> Albums { get; private set; }
		public List<Comment> Comments { get; private set; }
		public List<User> Fans { get; private set; }
		public List<Artist> Related { get; private set; }
		public List<Track> Radio { get; private set; }
		public List<Playlist> Playlists { get; private set; }

		#endregion

		public override void Init(string jsonContent, IRuntime runtime)
		{
			base.Init(jsonContent, runtime);
			UriPrefix = $"{ApiConsts.Artist.Self}/{Id}/";
		}

		public override async Task LoadAsync()
		{
			JsonContent = Runtime.ExecuteHttpGetAsync(UriPrefix).Result;

			Albums = await Runtime.DeserializeEntitiesFromData<Album>(
				await Runtime.ExecuteHttpGetAsync(UriPrefix + ApiConsts.Artist.Albums));
			Comments = await Runtime.DeserializeEntitiesFromData<Comment>(
				await Runtime.ExecuteHttpGetAsync(UriPrefix + ApiConsts.Artist.Comments));
			Fans = await Runtime.DeserializeEntitiesFromData<User>(
				await Runtime.ExecuteHttpGetAsync(UriPrefix + ApiConsts.Artist.Fans));
			Related = await Runtime.DeserializeEntitiesFromData<Artist>(
				await Runtime.ExecuteHttpGetAsync(UriPrefix + ApiConsts.Artist.Related));
			Radio = await Runtime.DeserializeEntitiesFromData<Track>(
				await Runtime.ExecuteHttpGetAsync(UriPrefix + ApiConsts.Artist.Top));
			Playlists = await Runtime.DeserializeEntitiesFromData<Playlist>(
				await Runtime.ExecuteHttpGetAsync(UriPrefix + ApiConsts.Artist.Playlists));
		}

		public Artist CreateDefault()
		{
			var unknownArtist = new Artist();
			unknownArtist.Albums = new List<Album>();
			unknownArtist.Comments = new List<Comment>();
			unknownArtist.Fans = new List<User>();
			unknownArtist.Id = long.MinValue;
			unknownArtist.Name = @"N\A";
			unknownArtist.Playlists = new List<Playlist>();
			unknownArtist.Radio = new List<Track>();
			unknownArtist.Related = new List<Artist>();

			return unknownArtist;
		}

		#region Actions

		/// <summary>
		/// Add a comment to the artist
		/// </summary>
		/// <remarks>basic_access</remarks>
		/// <param name="comment">The content of the comment</param>
		public async Task CommentArtist(string comment)
		{
			await SendPostAsync(ApiConsts.Artist.Comments, "comment", comment);
		}

		public async Task AddToFavorites()
		{
			//	await Runtime.Me.AddArtistAsync(this);
		}

		public async Task RemoveFromFavorites()
		{
			//	await Runtime.Me.RemoveArtistAsync(this);
		}

		#endregion
	}
}
