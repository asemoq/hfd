namespace Hfd.BizLayer.Internals;

public interface IProviderOptions
{
    /// <summary>
    /// Адрес
    /// </summary>
    Uri? HostUri { get; }

    /// <summary>
    /// Адрес провайдера
    /// </summary>
    Uri? TgUri { get; }
}

/// <summary>
/// Реализация по умолчанию
/// </summary>
public class ProviderOptions : IProviderOptions
{
    /// <inheritdoc/>
    public Uri? HostUri { get; set; }

    /// <inheritdoc/>
    public Uri? TgUri { get; set; }
}
