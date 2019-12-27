using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DeezerJsonApiWrapperNet.ApiObjects
{
	[DebuggerDisplay("{Text}")]
	public class Comment : JsonDeserialized
	{
		[JsonProperty(ApiConsts.Comment.Id)]
		public long Id { get; set; }

		[JsonProperty(ApiConsts.Comment.Text)]
		public string Text { get; set; }

		[JsonProperty(ApiConsts.Comment.Date)]
		public string Date { get; set; }

		[JsonProperty(ApiConsts.Comment.OwnerEntity)]
		public JsonDeserialized OwnerEntity { get; set; }

		[JsonProperty(ApiConsts.Comment.Author)]
		public User Author { get; set; }

		public override void Init(string jsonContent, IRuntime runtime)
		{
			base.Init(jsonContent, runtime);
			UriPrefix = $"{ApiConsts.Comment.Self}/{Id}/";
		}

		public override async Task LoadAsync()
		{
			JsonContent = await Runtime.ExecuteHttpGetAsync(UriPrefix);

			Author = await Runtime.DeserializeEntityFromData<User>(await SendGetAsync(ApiConsts.Comment.OwnerEntity));
		}
	}
}
