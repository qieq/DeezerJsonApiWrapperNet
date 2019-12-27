using System.Diagnostics;
using System.Threading.Tasks;

namespace DeezerJsonApiWrapperNet.ApiObjects
{
	[DebuggerDisplay("{Name}")]
	public class Me : User
	{
		public async Task AddAlbumAsync(Album album)
		{
			await SendPostAsync(ApiConsts.User.Albums, "album_id", album.Id);
		}

		public async Task RemoveAlbumAsync(Album album)
		{
			await SendDeleteAsync(ApiConsts.User.Albums, "album_id", album.Id);
		}

		public async Task AddArtistAsync(Artist artist)
		{
			await SendPostAsync(ApiConsts.User.Artists, "artist_id", artist.Id);
		}

		public async Task RemoveArtistAsync(Artist artist)
		{
			await SendDeleteAsync(ApiConsts.User.Artists, "artist_id", artist.Id);
		}

		public async Task CreateFolder(string title)
		{
			await SendPostAsync(ApiConsts.User.Folders, "title", title);
		}

		public async Task FollowUser(User user)
		{
			await SendPostAsync(ApiConsts.User.Followings, "user_id", user.Id);
		}

		public async Task UnfollowUser(User user)
		{
			await SendDeleteAsync(ApiConsts.User.Followings, "user_id", user.Id);
		}

		public async Task AddNotification(string message)
		{
			await SendPostAsync(ApiConsts.User.Notifications, "message", message);
		}

		public async Task CreatePlaylistAsync(string title)
		{
			await SendPostAsync(ApiConsts.User.Playlists, "title", title);
		}

		public async Task AddPlaylistToFavoritesAsync(Playlist playlist)
		{
			await SendPostAsync(ApiConsts.User.Playlists, "playlist_id", playlist.Id);
		}

		public async Task RemovePlaylistFromFavoritesAsync(Playlist playlist)
		{
			await SendDeleteAsync(ApiConsts.User.Playlists, "playlist_id", playlist.Id);
		}

		public async Task AddRadioToFavoritesAsync(Radio radio)
		{
			await SendPostAsync(ApiConsts.User.Playlists, "radio_id", radio.Id);
		}

		public async Task RemoveRadioFromFavoritesAsync(Radio radio)
		{
			await SendDeleteAsync(ApiConsts.User.Playlists, "radio_id", radio.Id);
		}

		public async Task AddTrackToFavoritesAsync(Track track)
		{
			await SendPostAsync(ApiConsts.User.Tracks, "track_id", track.Id);
		}

		public async Task RemoveTrackFromFavoritesAsync(Track track)
		{
			await SendDeleteAsync(ApiConsts.User.Tracks, "track_id", track.Id);
		}
	}
}
