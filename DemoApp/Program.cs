using DeezerJsonApiWrapperNet.Runtime;
using Microsoft.Extensions.Configuration;

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
			
			DeezerRuntime rt = new DeezerRuntime(accessToken);
			rt.InitCurrentUser().Wait();
			var me = rt.CurrentUser;

			// we need to LoadAsync explicitly each time to load the whole data unless somebody comes up with a better solution
			me.LoadAsync().Wait();

			var playlists = me.Playlists;
		}
	}
}
