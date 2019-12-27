using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeezerJsonApiWrapperNet.ApiObjects
{
	public class Chart : JsonDeserialized
	{
		public List<Track> Tracks { get; private set; }
		public List<Album> Albums { get; private set; }
		public List<Artist> Artists { get; private set; }
		public List<Playlist> Playlists { get; private set; }

		public override void Init(string jsonContent, IRuntime runtime)
		{
			base.Init(jsonContent, runtime);
			UriPrefix = $"{ApiConsts.Chart.Self}/";
		}

		public override async Task LoadAsync()
		{
			JsonContent = await Runtime.ExecuteHttpGetAsync(UriPrefix);

			Tracks = await Runtime.DeserializeEntitiesFromData<Track>(
				await SendGetAsync(ApiConsts.Chart.Tracks));
			Albums = await Runtime.DeserializeEntitiesFromData<Album>(
				await SendGetAsync(ApiConsts.Chart.Albums));
			Artists = await Runtime.DeserializeEntitiesFromData<Artist>(
				await SendGetAsync(ApiConsts.Chart.Artists));
			Playlists =
				await Runtime.DeserializeEntitiesFromData<Playlist>(
					await SendGetAsync(ApiConsts.Chart.Playlists));
		}
	}
}