using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hfd.Base.Contract.Dto;
using Hfd.Base.Contract.Exceptions;
using Hfd.Base.Contract.Internals;
using Hfd.Base.Contract.Model;
using Hfd.Base.Contract.Services;
using Hfd.Base.DbLayer;
using Hfd.DbLayer.Internals;
using Microsoft.EntityFrameworkCore;

namespace Hfd.DbLayer.Services;

public class UserLoginDbService : HfdDbService, IUserLoginDbService
{
	/// <summary>
	///     Constructor of user service class.
	/// </summary>
	/// <param name="storage"></param>
	/// <param name="log"></param>
	public UserLoginDbService(HfdDbStorage storage, ILogger<UserLoginDbService> log)
		: base(storage, log)
	{
	}

	public async Task<IPageList<IUserFull>> GetAll(UserFilter filter)
	{
		return await Execute(async model =>
		{
			Expression<Func<UserEntity, bool>> query = x => true;

			if (filter.Blocked != null) query = query.AndExpression(x => x.Blocked == filter.Blocked);

			if (filter.Usertype != null) query = query.AndExpression(x => x.Usertype == filter.Usertype);

			if (filter.Language != null) query = query.AndExpression(x => x.Language == filter.Language);

			var total = await model.Users.Where(query).CountAsync();
			var skip = Math.Min(total, filter.Skip);
			var take = Math.Min(total - filter.Skip, filter.Take == 0 ? total : filter.Take);

			return new PageList<IUserFull>
			{
				Total = total,
				First = skip,
				Items = take > 0
					? await model.Users.Where(query).Skip(skip).Take(take).AsNoTrackingWithIdentityResolution()
						.ToListAsync<IUserFull>()
					: new List<IUserFull>()
			};
		});
	}

	/// <inheritdoc />
	public async Task<IUserFull> Create(LoginRole type, UserInfo user, string password)
	{
		return await Execute(async model =>
		{
			var login = user.Username.ToLowerInvariant();
			var existingUser = await model.Users.SingleOrDefaultAsync(u => u.Username == login);
			if (existingUser != null)
				throw new HfdException(nameof(HfdBaseError.User_Already_Exists));

			var entity = new UserEntity
			{
				Usertype = type,
				Username = user.Username,
				UniqueId = user.UniqueId,
				FullName = user.FullName,
				Password = password,
				Language = user.Language,
				Blocked = false
			};
			await model.Users.AddAsync(entity);
			await model.SaveChangesAsync();

			return entity;
		});
	}

	/// <inheritdoc />
	public async Task<IUserFull> GetOne(long id)
	{
		return await Execute(async model
			=> await model.Users.SingleOrDefaultAsync(u => u.Id == id));
	}

	/// <inheritdoc />
	public async Task<IUserFull> GetOne(string username)
	{
		var login = username.ToLowerInvariant();
		return await Execute(async model
			=> await model.Users.SingleOrDefaultAsync(u => u.Username == login));
	}

	public async Task<IUserFull> GetOneByPhoneNumber(string phoneNumber)
	{
		return await Execute(async model
			=> await model.Users.SingleOrDefaultAsync(u =>
				u.PatientEntity.PhoneNumber == phoneNumber || u.NurseEntity.PhoneNumber == phoneNumber ||
				u.PsychologistEntity.PhoneNumber == phoneNumber || u.SocialWorkerEntity.PhoneNumber == phoneNumber));
	}

	/// <inheritdoc />
	public async Task<IUserFull> Update(long id, UserInfo user)
	{
		return await Execute(async model =>
		{
			var entity = await model.Users.SingleOrDefaultAsync(u => u.Id == id) ??
						throw new NotFoundException(nameof(HfdBaseError.User_Not_Found));

			var login = user.Username.ToLowerInvariant();
			var existingUser = await model.Users.SingleOrDefaultAsync(u => u.Username == login);
			if (existingUser != null && existingUser != entity)
				throw new HfdException(nameof(HfdBaseError.User_Already_Exists));

			entity.Username = login;
			entity.UniqueId = user.UniqueId;
			entity.FullName = user.FullName;
			entity.Language = user.Language;
			entity.Usertype = user.Usertype;
			if (!string.IsNullOrEmpty(user.Password)) entity.Password = user.Password.Hash();
			await model.SaveChangesAsync();
			return entity;
		});
	}

	/// <summary>
	///     Изменить пользователя.
	/// </summary>
	/// <param name="id">Идентификатор пользователя.</param>
	/// <param name="password">Образ пароля для хранения в базе.</param>
	/// <returns>Асинхронный результат.</returns>
	public async Task ChangePassword(long id, string password)
	{
		await Execute(async model =>
		{
			var entity
				= await model.Users.SingleOrDefaultAsync(u => u.Id == id)
				?? throw new NotFoundException(nameof(HfdBaseError.User_Not_Found));

			entity.Password = password;
			await model.SaveChangesAsync();
		});
	}

	/// <inheritdoc />
	public async Task<IUserFull> Block(long id, bool blocked)
	{
		return await Execute(async model =>
		{
			var entity
				= await model.Users.SingleOrDefaultAsync(u => u.Id == id)
				?? throw new NotFoundException(nameof(HfdBaseError.User_Not_Found));

			if (entity.Blocked == blocked) return entity;
			entity.Blocked = blocked;

			await model.SaveChangesAsync();

			return entity;
		});
	}

	/// <inheritdoc />
	public async Task Delete(long id)
	{
		await Execute(async model =>
		{
			var entity
				= await model.Users.SingleOrDefaultAsync(u => u.Id == id)
				?? throw new NotFoundException(nameof(HfdBaseError.User_Not_Found));

			if (entity.Username.Equals("admin", StringComparison.InvariantCultureIgnoreCase))
				throw new HfdException(nameof(HfdBaseError.User_Admin_Cannot_Be_Deleted));

			model.Remove(entity);
			await model.SaveChangesAsync();
		});
	}
}