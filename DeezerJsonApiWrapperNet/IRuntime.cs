using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeezerJsonApiWrapperNet
{
	public interface IRuntime
	{
		Task<string> ExecuteHttpGetAsync(string method, string[] queryParameters = null);
		Task<string> ExecuteHttpDeleteAsync(string method, string[] queryParameters = null);
		Task<string> ExecuteHttpPostAsync(string method, Dictionary<string, string> content, string[] queryParameters = null);

		Task<T> DeserializeEntityFromData<T>(string jsonContent) where T : JsonDeserialized, IDefaultSelfProvider<T>, new();
		Task<T> DeserializeEntityFromSection<T>(string jsonContent, string jsonTopLevelKey) where T : JsonDeserialized, IDefaultSelfProvider<T>, new();
		Task<PagedList<T>> DeserializeEntitiesFromData<T>(string jsonContent) where T : JsonDeserialized;
		Task<PagedList<T>> DeserializeEntitiesFromSection<T>(string jsonContent, string jsonTopLevelKey) where T : JsonDeserialized;
		Task<PagedList<T>> DeserializeEntitiesFromSectionData<T>(string jsonContent, string jsonTopLevelKey) where T : JsonDeserialized;
		Task<T> DeserializeEntity<T>(string jsonContent) where T : JsonDeserialized;
	}
}