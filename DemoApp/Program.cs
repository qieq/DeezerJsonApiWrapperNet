using DeezerJsonApiWrapperNet.ApiObjects;
using DeezerJsonApiWrapperNet.Runtime;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DemoApp
{
	class Program
	{
		public static IConfigurationRoot Configuration { get; set; }

		static void Main(string[] args)
		{
			var builder = new ConfigurationBuilder();
			builder.AddUserSecrets<Program>();

			Configuration = builder.Build();
			var accessToken = Configuration["deezerApiKey"];

			var runtime = new DeezerRuntime(accessToken);

			Task.Factory.StartNew(() =>
			{
				try
				{
					var playlist = "Loved tracks";
					DumpPlaylistInfo(runtime, playlist, $"{playlist}_{DateTime.Now:yyyy-M-dd--HH-mm-ss}", true).Wait();
					// DumpPlaylistInfo(runtime, playlist, $"{playlist}_{DateTime.Now:yyyy-M-dd--HH-mm-ss}", true).Wait();
				}
				catch (Exception e)
				{
					Console.WriteLine($"Error: {e.Message}");
				}
			}).Wait();
		}

		static async Task DumpPlaylistInfo(DeezerRuntime runtime, string playlistName, string fileName, bool useJson = false)
		{
			if (runtime != null)
			{
				Console.WriteLine("Loading user...");
				User me = await runtime.GetCurrentUser();
				if (me == null)
				{
					throw new Exception("Can't load user information.");
				}

				// we need to LoadAsync explicitly each time to load the whole data
				// unless somebody comes up with a better solution
				await me.LoadAsync();

				Console.WriteLine("User loaded.");
				Console.WriteLine($"\nLooking for playlist {playlistName}...");

				var playlist = me.Playlists.FirstOrDefault(p => string.Equals(playlistName, p.Title));
				if (playlist == null)
				{
					throw new Exception("Can't find playlist.");
				}

				Console.WriteLine($"Loading {playlist.TracksCount} track(s) from playlist {playlistName}...");
				var progressUpdateMark = playlist.TracksCount / 10;

				await playlist.LoadAsync();
				for (int i = 0; i < playlist.Tracks.Count; i++)
				{
					var track = playlist.Tracks[i];
					await track.LoadAsync();
					if (i > 10 && i % progressUpdateMark == 0)
					{
						Console.WriteLine($"\t{i} loaded.");
					}
				}

				Console.WriteLine("Playlist tracks loaded sucessfully!");
				Console.WriteLine($"Writing results into {Path.Combine(AppContext.BaseDirectory, fileName)}");

				if (useJson)
				{
					using (var file = File.Open($"{fileName}.json", FileMode.CreateNew))
					{
						var options = new JsonSerializerOptions();
						options.MaxDepth = 2;
						options.WriteIndented = true;
						options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
						await JsonSerializer.SerializeAsync(file, playlist.Tracks);
					}
				}
				else
				{
					var probableTrackDescriptionLength = 30;
					var dump = new StringBuilder(playlist.Tracks.Count * probableTrackDescriptionLength);
					dump.AppendLine($"Playlist: {playlist.Title}, track count {playlist.Tracks.Count}:");
					foreach (var track in playlist.Tracks)
					{
						dump.AppendLine($"'{track.Artist.Name}' - '{track.Album.Title}' ({track.Album.ReleaseDate:yyyy}) - '{track.Title}'");
					}

					File.WriteAllText($"{fileName}.txt", dump.ToString());
				}

				Console.WriteLine("Results written succesfully!");
			}
		}
	}
}