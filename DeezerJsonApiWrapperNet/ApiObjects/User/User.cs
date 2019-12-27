using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using DeezerJsonApiWrapperNet;
using Newtonsoft.Json;

namespace DeezerJsonApiWrapperNet.ApiObjects
{
	[DebuggerDisplay("{Name}")]
	public class User : JsonDeserialized, IDefaultSelfProvider<User>
	{
		public class Recommendations : JsonDeserialized
		{
			[JsonProperty(ApiConsts.User.Recommendations.Albums)]
			public List<Album> Albums { get; set; }

			[JsonProperty(ApiConsts.User.Recommendations.Artists)]
			public List<Artist> Artists { get; set; }

			[JsonProperty(ApiConsts.User.Recommendations.Playlists)]
			public List<Playlist> Playlists { get; set; }

			[JsonProperty(ApiConsts.User.Recommendations.Tracks)]
			public List<Track> Tracks { get; set; }

			[JsonProperty(ApiConsts.User.Recommendations.Radios)]
			public List<Radio> Radios { get; set; }

			private readonly User _user;

			public Recommendations(User user)
			{
				_user = user;
				Init(user.JsonContent, user.Runtime);
				UriPrefix = $"{_user.UriPrefix}/{ApiConsts.User.Recommendations.Self}/";
			}

			public override async Task LoadAsync()
			{
				Albums = await Runtime.DeserializeEntitiesFromData<Album>(
					await SendGetAsync(ApiConsts.User.Recommendations.Albums));
				Artists = await Runtime.DeserializeEntitiesFromData<Artist>(
					await SendGetAsync(ApiConsts.User.Recommendations.Artists));
				Playlists = await Runtime.DeserializeEntitiesFromData<Playlist>(
					await SendGetAsync(ApiConsts.User.Recommendations.Playlists));
				Tracks = await Runtime.DeserializeEntitiesFromData<Track>(
					await SendGetAsync(ApiConsts.User.Recommendations.Tracks));
				Radios = await Runtime.DeserializeEntitiesFromData<Radio>(
					await SendGetAsync(ApiConsts.User.Recommendations.Radios));

				OnPropertyChanged();
			}
		}

		public class Charts : JsonDeserialized
		{
			[JsonProperty(ApiConsts.User.Charts.Top25Tracks)]
			public List<Track> Top25Tracks { get; set; }

			[JsonProperty(ApiConsts.User.Charts.TopAlbums)]
			public List<Album> TopAlbums { get; set; }

			[JsonProperty(ApiConsts.User.Charts.TopPlaylists)]
			public List<Playlist> TopPlaylists { get; set; }

			[JsonProperty(ApiConsts.User.Charts.TopArtists)]
			public List<Artist> TopArtists { get; set; }

			private User _user;

			public Charts(User user)
			{
				_user = user;
				Init(user.JsonContent, user.Runtime);
				UriPrefix = $"{_user.UriPrefix}/{ApiConsts.User.Charts.Self}/";
			}

			public override async Task LoadAsync()
			{
				Top25Tracks =
					await Runtime.DeserializeEntitiesFromData<Track>(
						await SendGetAsync(ApiConsts.User.Charts.Top25Tracks));
				TopAlbums = await Runtime.DeserializeEntitiesFromData<Album>(
					await SendGetAsync(ApiConsts.User.Charts.TopAlbums));
				TopPlaylists =
					await Runtime.DeserializeEntitiesFromData<Playlist>(
						await SendGetAsync(ApiConsts.User.Charts.TopPlaylists));
				TopArtists =
					await Runtime.DeserializeEntitiesFromData<Artist>(
						await SendGetAsync(ApiConsts.User.Charts.TopArtists));
			}
		}

		[JsonProperty(ApiConsts.User.Id)] public long Id { get; set; } // The user's Deezer ID

		[JsonProperty(ApiConsts.User.Name)]
		public string Name { get; set; } // The user's Deezer nickname

		[JsonProperty(ApiConsts.User.PictureUrl)]
		public Uri
			PictureUrl
		{
			get;
			set;
		} // The url of the user's profile picture. Add 'size' parameter to the url to change size. Can be 'small', 'medium', 'big', 'xl'

		[JsonProperty(ApiConsts.User.PictureSmallUrl)]
		public Uri PictureSmallUrl { get; set; } // The url of the user's profile picture in size small.

		[JsonProperty(ApiConsts.User.PictureMediumUrl)]
		public Uri PictureMediumUrl { get; set; } // The url of the user's profile picture in size medium.

		[JsonProperty(ApiConsts.User.PictureBigUrl)]
		public Uri PictureBigUrl { get; set; } // The url of the user's profile picture in size big.

		[JsonProperty(ApiConsts.User.PictureXlUrl)]
		public Uri PictureXlUrl { get; set; } // The url of the user's profile picture in size xl.

		[JsonProperty(ApiConsts.User.Country)]
		public string Country { get; set; } // The user's country

		[JsonProperty(ApiConsts.User.Lang)]
		public string Lang { get; set; } // The user's language

		[JsonProperty(ApiConsts.User.IsKid)]
		public bool IsKid { get; set; } // If the user is a kid or not

		[JsonProperty(ApiConsts.User.FlowUrl)]
		public Uri FlowUrl { get; set; } // API Link to the flow of this user

		[JsonProperty(ApiConsts.User.Type)]
		public string Type { get; set; }

		[JsonProperty(ApiConsts.User.LastName)]
		public string LastName { get; set; } // The user's last name

		[JsonProperty(ApiConsts.User.FirstName)]
		public string FirstName { get; set; } // The user's first name

		[JsonProperty(ApiConsts.User.Email)]
		public string Email { get; set; } // The user's email

		[JsonProperty(ApiConsts.User.Status)]
		public int Status { get; set; } // The user's status

		[JsonProperty(ApiConsts.User.Birthday)]
		public string Birthday { get; set; } // The user's birthday

		[JsonProperty(ApiConsts.User.InscriptionDate)]
		public string InscriptionDate { get; set; } // The user's inscription date

		[JsonProperty(ApiConsts.User.Gender)]
		public string Gender { get; set; } // The user's gender : F or M

		[JsonProperty(ApiConsts.User.Link)]
		public Uri Link { get; set; } // The url of the profile for the user on Deezer

		public List<Album> Albums { get; set; }
		public List<Artist> Artists { get; set; }
		public List<Track> Flow { get; set; }
		public List<Folder> Folders { get; set; }
		public List<User> Followings { get; set; }
		public List<User> Followers { get; set; }
		public List<Track> History { get; set; }
		public string Permissions { get; set; }
		public Options Options { get; set; }
		public List<Track> PersonalSongs { get; set; }
		public List<Playlist> Playlists { get; set; }
		public List<Radio> Radios { get; set; }
		public Recommendations MusicRecommendations { get; set; }
		public List<Track> Tracks { get; set; }
		public Charts PersonalCharts { get; set; }

		public override void Init(string jsonContent, IRuntime runtime)
		{
			base.Init(jsonContent, runtime);
			UriPrefix = $"/{ApiConsts.User.Self}/{Id}/";
		}

		public override async Task LoadAsync()
		{
			JsonContent = await Runtime.ExecuteHttpGetAsync(UriPrefix).ConfigureAwait(false);
			Albums = await Runtime.DeserializeEntitiesFromData<Album>(await SendGetAsync(ApiConsts.User.Albums)
				.ConfigureAwait(false));
			Artists = await Runtime.DeserializeEntitiesFromData<Artist>(await SendGetAsync(ApiConsts.User.Artists)
				.ConfigureAwait(false));
			Flow = await Runtime.DeserializeEntitiesFromData<Track>(await SendGetAsync(ApiConsts.User.Flow)
				.ConfigureAwait(false));
			Folders = await Runtime.DeserializeEntitiesFromData<Folder>(await SendGetAsync(ApiConsts.User.Folders)
				.ConfigureAwait(false));
			Followings =
			await Runtime.DeserializeEntitiesFromData<User>(await SendGetAsync(ApiConsts.User.Followings)
				.ConfigureAwait(false));
			Followers = await Runtime.DeserializeEntitiesFromData<User>(
			await SendGetAsync(ApiConsts.User.Followers)
				.ConfigureAwait(false));
			History = await Runtime.DeserializeEntitiesFromData<Track>(await SendGetAsync(ApiConsts.User.History)
				.ConfigureAwait(false));
			// TODO: add permissions enum or object
			Permissions = await SendGetAsync(ApiConsts.User.Permissions);
			Options = await Runtime.DeserializeEntity<Options>(await SendGetAsync(ApiConsts.User.Options)
				.ConfigureAwait(false));
			PersonalSongs =
			await Runtime.DeserializeEntitiesFromData<Track>(await SendGetAsync(ApiConsts.User.PersonalSongs)
				.ConfigureAwait(false));
			Playlists = await Runtime.DeserializeEntitiesFromData<Playlist>(
			await SendGetAsync(ApiConsts.User.Playlists).ConfigureAwait(false));
			Radios = await Runtime.DeserializeEntitiesFromData<Radio>(await SendGetAsync(ApiConsts.User.Radios)
				.ConfigureAwait(false));
			Tracks = await Runtime.DeserializeEntitiesFromData<Track>(await SendGetAsync(ApiConsts.User.Tracks)
				.ConfigureAwait(false));

			MusicRecommendations = new Recommendations(this);
			await MusicRecommendations.LoadAsync().ConfigureAwait(false);

			PersonalCharts = new Charts(this);
			await PersonalCharts.LoadAsync().ConfigureAwait(false);

			OnPropertyChanged();
		}

		public User CreateDefault()
		{
			var defaultUser = new User();
			defaultUser.Albums = new List<Album>();
			defaultUser.Artists = new List<Artist>();
			defaultUser.Flow = new List<Track>();
			defaultUser.Folders = new List<Folder>();
			defaultUser.Followers = new List<User>();
			defaultUser.Followings= new List<User>();
			defaultUser.History = new List<Track>();
			defaultUser.PersonalSongs = new List<Track>();
			defaultUser.Playlists = new List<Playlist>();
			defaultUser.Radios = new List<Radio>();
			defaultUser.Tracks = new List<Track>();

			return new User();
		}
	}
}
