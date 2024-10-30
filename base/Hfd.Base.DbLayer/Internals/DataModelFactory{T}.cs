using Hfd.Base.Contract.Services;
using Hfd.Base.DbLayer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hfd.Base.DbLayer.Internals;

public abstract class DataModelFactory<TDataModel> : IDataModelFactory<TDataModel>
	where TDataModel : AuditableDataModel
{
	/// <summary>
	///     Инициализирует экземпляр <see cref="DataModelFactory{TDataModel}" />.
	/// </summary>
	/// <param name="userAccessor">Текущий пользователь.</param>
	public DataModelFactory(IUserAccessor userAccessor)
	{
		UserAccessor = userAccessor;
	}

	/// <summary>
	///     Строка соединения.
	/// </summary>
	public string ConnectionString
		=> UserAccessor.Services.GetRequiredService<IConfiguration>().GetValue<string>("dataModel:connectionString");

	/// <summary>
	///     Контехт.
	/// </summary>
	public IUserAccessor UserAccessor { get; }

	/// <inheritdoc />
	public abstract TDataModel Create();
}