using Hfd.Base.Contract.Model;

namespace Hfd.Contract.Model;

/// <summary>
///     Сообщение.
/// </summary>
public interface IFileFull : IBaseInfo, IFileInfo
{
}

public interface IFileInfo
{
	/// <summary>
	///     Ссылка на сообщение.
	/// </summary>
	long MessageId { get; }

	/// <summary>
	///     Identifier for this file, which can be used to download or reuse the file
	/// </summary>
	string FileId { get; }

	/// <summary>
	///     Unique identifier for this file, which is supposed to be the same over time and for different bots.
	///     Can't be used to download or reuse the file.
	/// </summary>
	string UniqueId { get; }

	/// <summary>
	///     Optional. File size
	/// </summary>
	long? Size { get; }

	string Object { get; }
}