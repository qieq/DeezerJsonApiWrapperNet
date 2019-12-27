using System.Collections.Generic;

namespace DeezerJsonApiWrapperNet
{
	public class PagedList<T> : List<T>
	{
		public string NextUri { get; }

		public PagedList(string nextUri)
		{
			NextUri = nextUri;
		}
	}
}
