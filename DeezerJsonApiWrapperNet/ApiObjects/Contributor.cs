using System.Diagnostics;
using DeezerJsonApiWrapperNet.ApiObjects.Enums;

namespace DeezerJsonApiWrapperNet.ApiObjects
{
	[DebuggerDisplay("{Name}")]
	public class Contributor : Artist
	{
		public ContibutorRole Role { get; set; }
	}
}
