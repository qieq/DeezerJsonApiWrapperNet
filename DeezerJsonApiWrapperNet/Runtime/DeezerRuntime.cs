using DeezerJsonApiWrapperNet.ApiObjects;
using LazyCache;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeezerJsonApiWrapperNet.Runtime
{
	public class DeezerRuntime : IRuntime
	{
		private readonly CachingService _cache = new CachingService();

		private readonly string ApiEndpoint = "https://api.deezer.com/";

		private readonly HttpClient _httpClient;
		private readonly string _accessToken;
		private readonly SerializationHelper _serializer;

		private Me _me;

		public DeezerRuntime(string accessToken)
		{
			_httpClient = new HttpClient();
			_accessToken = accessToken;
			_serializer = new SerializationHelper(this);
		}

		public async Task<User> GetCurrentUser()
		{
			if (_me == null)
			{
				_me = await GetEntity<Me>(ApiConsts.Me.Self).ConfigureAwait(false);
			}

			return _me;
		}

		public async Task<string> ExecuteHttpGetAsync(string method, string[] queryParameters = null)
		{
			var requestUri = GenerateRequestUri(method, queryParameters);
			return await GetResponse(requestUri).ConfigureAwait(false);
		}

		public async Task<string> ExecuteHttpGetAsIsAsync(string requestUri)
		{
			return await GetResponse(requestUri).ConfigureAwait(false);
		}

		private async Task<string> GetResponse(string requestUri)
		{
			return await _cache.GetOrAdd(requestUri, () => GetValidResponse(requestUri).ConfigureAwait(false));
		}

		private async Task<string> GetValidResponse(string requestUri)
		{
			var httpResponse = await _httpClient.GetAsync(requestUri).ConfigureAwait(false);
			var jsonResponse = await ValidateResponse(httpResponse).ConfigureAwait(false);
			return jsonResponse;
		}

		public async Task<string> ExecuteHttpDeleteAsync(string method, string[] queryParameters = null)
		{
			var requestUri = GenerateRequestUri(method, queryParameters);

			var response = await _httpClient.DeleteAsync(requestUri);
			return await ValidateResponse(response);
		}

		public async Task<string> ExecuteHttpPostAsync(string method, Dictionary<string, string> content, string[] queryParameters = null)
		{
			var requestUri = GenerateRequestUri(method, queryParameters);

			var encodedContent = new FormUrlEncodedContent(content ?? new Dictionary<string, string>());
			var response = await _httpClient.PostAsync(requestUri, encodedContent);
			return await ValidateResponse(response);
		}

		private static async Task<string> ValidateResponse(HttpResponseMessage httpResponse)
		{
			if (!httpResponse.IsSuccessStatusCode)
			{
				throw new DeezerRuntimeException(httpResponse.ReasonPhrase);
			}

			return await httpResponse.Content.ReadAsStringAsync();
		}

		private string GenerateRequestUri(string method, string[] queryParameters)
		{
			var requestUri = new StringBuilder();
			requestUri.AppendFormat("{0}{1}?", ApiEndpoint, method);

			if (queryParameters != null)
			{
				foreach (var qp in queryParameters)
				{
					requestUri.AppendFormat("{0}&", qp);
				}
			}

			if (!string.IsNullOrEmpty(_accessToken)) requestUri.AppendFormat("access_token={0}", _accessToken);

			return requestUri.ToString();
		}

		private async Task<T> GetEntity<T>(string entityUrlKey, long id = -1) where T : JsonDeserialized
		{
			var method = new StringBuilder(entityUrlKey.Length + 20); // additional 20 is ~length of id
			method.AppendFormat("/{0}/", entityUrlKey);
			if (id != -1) method.Append(id);

			string responseContent = await ExecuteHttpGetAsync(method.ToString()).ConfigureAwait(false);
			T entity = JsonConvert.DeserializeObject<T>(responseContent);
			entity.JsonContent = responseContent;
			entity.Init(responseContent, this);

			return entity;
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					if (_httpClient != null) _httpClient.Dispose();
				}

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~DeezerRuntime() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}

		#endregion

		public Task<T> DeserializeEntityFromData<T>(string jsonContent) where T : JsonDeserialized, IDefaultSelfProvider<T>, new()
		{
			return _serializer.DeserializeEntityFromJsonNodeData<T>(jsonContent);
		}

		public Task<T> DeserializeEntityFromSection<T>(string jsonContent, string jsonTopLevelKey) where T : JsonDeserialized, IDefaultSelfProvider<T>, new()
		{
			return _serializer.DeserializeEntityFromJsonNode<T>(jsonContent, jsonTopLevelKey);
		}

		public Task<PagedList<T>> DeserializeEntitiesFromData<T>(string jsonContent) where T : JsonDeserialized
		{
			return _serializer.DeserializeMultipleEntitiesFromJsonNodeData<T>(jsonContent);
		}

		public Task<PagedList<T>> DeserializeEntitiesFromSection<T>(string jsonContent, string jsonTopLevelKey) where T : JsonDeserialized
		{
			return _serializer.DeserializeMultipleEntitiesFromJsonNode<T>(jsonContent, jsonTopLevelKey);
		}

		public Task<PagedList<T>> DeserializeEntitiesFromSectionData<T>(string jsonContent, string jsonTopLevelKey) where T : JsonDeserialized
		{
			return _serializer.DeserializeMultipleEntitiesFromJsonNodeData<T>(jsonContent, jsonTopLevelKey);
		}

		public Task<T> DeserializeEntity<T>(string jsonContent) where T : JsonDeserialized
		{
			return _serializer.DeserializeEntity<T>(jsonContent);
		}
	}
}
