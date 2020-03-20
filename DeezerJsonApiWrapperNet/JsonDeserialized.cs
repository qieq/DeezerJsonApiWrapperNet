using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace DeezerJsonApiWrapperNet
{
	public abstract class JsonDeserialized : INotifyPropertyChanged
	{
		protected IRuntime Runtime { get; set; }
		public string JsonContent;

		private string _uriPrefix;
		protected string UriPrefix
		{
			get => _uriPrefix;
			set { if (string.IsNullOrEmpty(_uriPrefix)) _uriPrefix = value; }
		}

		public virtual void Init(string jsonContent, IRuntime runtime)
		{
			Runtime = runtime;
			JsonContent = jsonContent;
		}

		public abstract Task LoadAsync();

		protected async Task SendPostAsync(string uriPostfix, string varName, object varValue, string[] queryParameters = null)
		{
			ValidateRequest();
			await Runtime.ExecuteHttpPostAsync(UriPrefix + uriPostfix, new Dictionary<string, string> { { varName, varValue.ToString() } }, queryParameters);
		}

		protected async Task SendDeleteAsync(string uriPostfix, string varName, object varValue)
		{
			ValidateRequest();
			await Runtime.ExecuteHttpDeleteAsync(UriPrefix + uriPostfix, new[] { $"{varName}={varValue}" });
		}

		protected Task<string> SendGetAsync(string uriPostfix, string[] queryParameters = null)
		{
			ValidateRequest();
			return Runtime.ExecuteHttpGetAsync(UriPrefix + uriPostfix, queryParameters);
		}

		protected Task<string> SendGetAsIsAsync(string uri)
		{
			ValidateRequest();
			return Runtime.ExecuteHttpGetAsIsAsync(uri);
		}

		protected async Task<List<T>> LoadPagedContent<T>(string firstPageUriPostfix)
			where T : JsonDeserialized
		{
			PagedList<T> page;
			var content = new List<T>();

			page = await Runtime.DeserializeEntitiesFromData<T>(await SendGetAsync(firstPageUriPostfix));
			content.AddRange(page);

			var nextPageUri = page.NextUri;
			while (!string.IsNullOrEmpty(nextPageUri))
			{
				page = await Runtime.DeserializeEntitiesFromData<T>(await SendGetAsIsAsync(nextPageUri));
				content.AddRange(page);
				nextPageUri = page.NextUri;
			}

			return content;
		}

		private void ValidateRequest()
		{
			if (Runtime == null)
				throw new Exception(
					$"Cannot perform request without running {typeof(IRuntime).Name} instance.");
			if (string.IsNullOrEmpty(UriPrefix)) throw new Exception("No uri provided for request.");
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
