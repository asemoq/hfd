using Hfd.Common.Contract.Model;
using Hfd.Contract.Enums;

namespace Hfd.Contract.Model;

/// <summary>
/// </summary>
public interface ITrainingFull : IBaseInfo, ITrainingInfo
{
}

public interface ITrainingInfo
{
	/// <summary>
	/// </summary>
	string Name { get; }

	/// <summary>
	/// </summary>
	string Text { get; }

	/// <summary>
	/// </summary>
	string FileId { get; }

	/// <summary>
	/// </summary>
	FileType FileType { get; }
}