using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeezerJsonApiWrapperNet
{
	public class SerializationHelper
	{
		private readonly IRuntime _currentRuntime;
		private static readonly ILog Logger = LogManager.GetLogger(typeof(SerializationHelper));

		public SerializationHelper(IRuntime currentRuntime)
		{
			_currentRuntime = currentRuntime;
		}

		public Task<T> DeserializeEntityFromJsonNodeData<T>(string jsonContent) where T : JsonDeserialized, IDefaultSelfProvider<T>, new()
		{
			return DeserializeEntityFromJsonNode<T>(jsonContent, "data");
		}

		public Task<T> DeserializeEntityFromJsonNode<T>(string jsonContent, string nodeKey) where T : JsonDeserialized, IDefaultSelfProvider<T>, new()
		{
			if (TryGetParsedJsonContent(jsonContent, nodeKey, out string parsingResult, out _))
			{
				return DeserializeEntity<T>(parsingResult);
			}

			if (Logger.IsDebugEnabled)
			{
				Logger.Debug($"Nothing found in {jsonContent}\nby key '{nodeKey}'.\n\tReturning default value for {typeof(T)}.");
			}

			var t = new T();
			return Task.FromResult(t.CreateDefault());
		}

		public Task<PagedList<T>> DeserializeMultipleEntitiesFromJsonNodeData<T>(string jsonContent) where T : JsonDeserialized
		{
			if (TryGetParsedJsonContent(jsonContent, "data", out string parsingResult, out string nextUri))
			{
				var array = JArray.Parse(parsingResult);
				return Task.Run(() => DeserializeMultipleEntitiesFromJsonArray<T>(array, nextUri));
			}

			if (Logger.IsDebugEnabled)
			{
				Logger.Debug($"No objects can be parsed from {jsonContent}.\n\tReturning empty list of {typeof(T)}.");
			}

			return Task.FromResult(new PagedList<T>(string.Empty));
		}

		public Task<PagedList<T>> DeserializeMultipleEntitiesFromJsonNode<T>(string jsonContent, string nodeKey) where T : JsonDeserialized
		{
			if (TryGetParsedJsonContent(jsonContent, nodeKey, out string parsingResult, out string nextUri))
			{
				var array = JArray.Parse(parsingResult);
				return Task.Run(() => DeserializeMultipleEntitiesFromJsonArray<T>(array, nextUri));
			}

			if (Logger.IsDebugEnabled)
			{
				Logger.Debug($"No objects can be parsed from {jsonContent}\nby key '{nodeKey}'.\n\tReturning empty list of {typeof(T)}.");
			}

			return Task.FromResult(new PagedList<T>(string.Empty));
		}

		public Task<PagedList<T>> DeserializeMultipleEntitiesFromJsonNodeData<T>(string jsonContent, string nodeKey) where T : JsonDeserialized
		{
			if (TryGetParsedJsonContent(jsonContent, nodeKey, out string jsonResult, out string nextUri) &&
				TryGetParsedJsonContent(jsonResult, "data", out string nodeResult, out _))
			{
				var array = JArray.Parse(nodeResult);
				return Task.Run(() => DeserializeMultipleEntitiesFromJsonArray<T>(array, nextUri));
			}

			if (Logger.IsDebugEnabled)
			{
				Logger.Debug($"No objects can be parsed from {jsonContent}\nby key '{nodeKey}'.\n\tReturning empty list of {typeof(T)}.");
			}

			return Task.FromResult(new PagedList<T>(string.Empty));
		}

		private bool TryGetParsedJsonContent(string jsonContent, string nodeKey, out string content, out string nextUri)
		{
			content = String.Empty;
			nextUri = string.Empty;

			if (String.IsNullOrWhiteSpace(jsonContent) || String.IsNullOrWhiteSpace(nodeKey))
			{
				throw new ArgumentException($"{nameof(jsonContent)} or {nameof(nodeKey)} is empty!");
			}

			var jsonResult = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonContent);

			if (jsonResult.ContainsKey("error"))
			{
				if (Logger.IsDebugEnabled)
				{
					Logger.Debug(jsonResult["error"].ToString());
				}

				return false;
			}

			if (!jsonResult.ContainsKey(nodeKey))
			{
				return false;
			}

			if (jsonResult.ContainsKey("next"))
			{
				nextUri = jsonResult["next"].ToString();
			}

			content = jsonResult[nodeKey].ToString();
			return true;
		}

		private async Task<PagedList<T>> DeserializeMultipleEntitiesFromJsonArray<T>(JArray jsonArrayContent, string nextUri) where T : JsonDeserialized
		{
			var entities = new PagedList<T>(nextUri);
			foreach (var entityJson in jsonArrayContent)
			{
				entities.Add(await DeserializeEntity<T>(entityJson.ToString()));
			}

			return entities;
		}

		public Task<T> DeserializeEntity<T>(string jsonContent) where T : JsonDeserialized
		{
			return Task.Run(() =>
			{
				var entity = JsonConvert.DeserializeObject<T>(jsonContent);
				entity.Init(jsonContent, _currentRuntime);
				return entity;
			});
		}
	}
}
