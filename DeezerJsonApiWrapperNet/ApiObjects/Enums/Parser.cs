using System;

namespace DeezerJsonApiWrapperNet.ApiObjects.Enums
{
	public static class Parser
	{
		/// <summary>
		/// Add this value to all custom enums to handle not loaded values gracefully
		/// </summary>
		public static string UnableToLoadContent = nameof(UnableToLoadContent);

		public static T Parse<T>(string value)
		{
			return (T)Enum.Parse(typeof(RecordType), value ?? UnableToLoadContent, true);
		}
	}
}
