using DeezerJsonApiWrapperNet.Runtime;
using System;

namespace DemoApp
{
	class Program
	{
		static void Main(string[] args)
		{
			var accessToken = string.Empty;
			DeezerRuntime rt = new DeezerRuntime(accessToken);
			rt.InitCurrentUser().Wait();
			var me = rt.CurrentUser;
			
			// we need to LoadAsync explicitly each time to load the whole data unless somebody comes up with a better solution
			me.LoadAsync().Wait();

			var playlists = me.Playlists;
		}
	}
}
