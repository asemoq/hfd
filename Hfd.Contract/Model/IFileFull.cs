using Hfd.Common.Contract.Model;

namespace Hfd.Contract.Model;

/// <summary>
///     Сообщение.
/// </summary>
public interface IFileFull : IBaseInfo, IFileInfo
{
}

public interface IFileInfo
{
    long MessageId { get; }

    string FileId { get; }

    string UniqueId { get; }

    long? Size { get; }

    string? Data { get; }
}
