namespace DeezerJsonApiWrapperNet.ApiObjects.Enums
{
	public enum RecordType
	{
		Album,
		Ep,
		Single,
		/// <summary>
		/// This value is used to gracefully process not (yet) loaded data. Add to all custom enums
		/// </summary>
		UnableToLoadContent
	}
}
