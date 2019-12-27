using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DeezerJsonApiWrapperNet.ApiObjects
{
	[DebuggerDisplay("{Title}")]
	public class Folder : JsonDeserialized
	{
		[JsonProperty(ApiConsts.Folder.Id)]
		public long Id { get; set; }

		[JsonProperty(ApiConsts.Folder.Title)]
		public string Title { get; set; }

		[JsonProperty(ApiConsts.Folder.Creator)]
		public User Creator { get; set; }

		public override void Init(string jsonContent, IRuntime runtime)
		{
			base.Init(jsonContent, runtime);
			UriPrefix = $"{ApiConsts.Folder.Self}/{Id}/";
		}

		public override async Task LoadAsync()
		{
			JsonContent = await Runtime.ExecuteHttpGetAsync(UriPrefix);
		}
	}
}
