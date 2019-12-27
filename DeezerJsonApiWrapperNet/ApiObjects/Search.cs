using System;
using System.Threading.Tasks;

namespace DeezerJsonApiWrapperNet.ApiObjects
{
	public class Search : JsonDeserialized
	{						   
		public Search()
		{
			UriPrefix = "/search/";
		}

		public async Task FindTrack(long id = -1, string title = null, string shortTitle = null, int duration = -1, int rank = -1, Uri preview = null)
		{

		}

		public override async Task LoadAsync()
		{
			throw new NotImplementedException();
		}

		//public async Task FindArtist

		/*
		search / album 	Search albums 	A list of object of type album
search / artist 	Search artists 	A list of object of type artist
search / history 	Get user search history 	A list of object of type track
A list of object of type album
A list of object of type artist
A list of object of type playlist
A list of object of type podcast
A list of object of type radio
search / playlist 	Search playlists 	A list of object of type playlist
search / radio 	Search radio 	A list of object of type radio
search / track 	Search tracks 	A list of object of type track
search / user
		 */

		//public async Task<Artist> FindArtist(string name, Order order = Order.RANKING)
		//{

		//}
	}
}
