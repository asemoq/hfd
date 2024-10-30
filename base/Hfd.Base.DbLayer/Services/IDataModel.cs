namespace Hfd.Base.DbLayer.Services;

/// <summary>
///     Маркерный интерфейс модели данных.
/// </summary>
public interface IDataModel : IDisposable
{
}

/// <summary>
///     Фабрика модели данных.
/// </summary>
/// <typeparam name="TDataModel">Тип модели данных.</typeparam>
public interface IDataModelFactory<TDataModel>
	where TDataModel : class, IDataModel
{
	/// <summary>
	///     Создает модель данных.
	/// </summary>
	/// <returns>Модель данных.</returns>
	TDataModel Create();
}