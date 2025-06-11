using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hfd.Common.ApiClient.Model;
using Hfd.Common.Contract.Enums;
using Hfd.Common.Contract.Exceptions;
using Hfd.Common.Contract.Internals;
using Hfd.Common.Contract.Model;
using Hfd.Common.Contract.Services;

namespace Hfd.DbLayer.Services;

public class UserLoginDbService(HfdDbStorage storage, ILogger<UserLoginDbService> log)
    : HfdDbService(storage, log), IUserLoginDbService
{
    public async Task<IPageList<IUserFull>> GetAll(UserFilter filter)
    {
        return await this.Execute(async model =>
        {
            Expression<Func<UserEntity, bool>> query = x => true;
            query = query.AndIf(filter.Blocked != null, x => x.Blocked == filter.Blocked);
            query = query.AndIf(filter.Usertype != null, x => x.Usertype == Enum.Parse<LoginRole>(filter.Usertype!));
            query = query.AndIf(filter.Language != null, x => x.Language == Enum.Parse<Language>(filter.Language!));

            var total = await model.Users.Where(query).CountAsync();
            var skip = Math.Min(total, filter.Skip);
            var take = Math.Min(total - filter.Skip, filter.Take == 0 ? total : filter.Take);

            return new PageList<IUserFull>
            {
                Total = total,
                First = skip,
                Items = take > 0
                    ? await model.Users.Where(query)
                        .Skip(skip)
                        .Take(take)
                        .AsNoTrackingWithIdentityResolution()
                        .ToListAsync<IUserFull>()
                    : new List<IUserFull>(),
            };
        });
    }

    /// <inheritdoc />
    public async Task<IUserFull> Create(LoginRole type, UserInfo user, string password)
    {
        return await this.Execute(async model =>
        {
            this.Storage.BeginTransaction();
            try
            {
                var login = user.Username.ToUpperInvariant();
                var existingUser = await model.Users.SingleOrDefaultAsync(u => u.Username == login);
                if (existingUser != null)
                {
                    throw new HfdException(nameof(HfdCoreError.UserAlreadyExists));
                }

                var entity = new UserEntity
                {
                    Usertype = type,
                    Username = user.Username.ToUpperInvariant(),
                    UniqueId = user.UniqueId,
                    FullName = user.FullName,
                    Password = password,
                    Language = Enum.Parse<Language>(user.Language),
                    Blocked = false,
                };
                await model.Users.AddAsync(entity);
                await model.SaveChangesAsync();
                this.Storage.CommitTransaction();
                return entity;
            }
            catch
            {
                this.Storage.RollbackTransaction();
                throw;
            }
        });
    }

    /// <inheritdoc />
    public async Task<IUserFull> GetOne(long id)
    {
        return await this.Execute(async model
            => await model.Users.SingleOrDefaultAsync(u => u.Id == id)
               ?? throw new NotFoundException(nameof(HfdCoreError.UserNotFound)));
    }

    /// <inheritdoc />
    public async Task<IUserFull> GetOne(string username)
    {
        var login = username.ToUpperInvariant();
        return await this.Execute(async model => await model.Users.SingleOrDefaultAsync(u => u.Username == login)
                                                 ?? throw new NotFoundException(nameof(HfdCoreError.UserNotFound)));
    }

    public async Task<IUserFull> GetOneByPhoneNumber(string phoneNumber)
    {
        return await this.Execute(async model => await model.Users.SingleOrDefaultAsync(u
                                                     => u.PatientEntity!.PhoneNumber == phoneNumber
                                                        || u.NurseEntity!.PhoneNumber == phoneNumber
                                                        || u.PsychologistEntity!.PhoneNumber == phoneNumber
                                                        || u.SocialWorkerEntity!.PhoneNumber == phoneNumber)
                                                 ?? throw new NotFoundException(nameof(HfdCoreError.UserNotFound)));
    }

    /// <inheritdoc />
    public async Task<IUserFull> Update(long id, UserInfo user)
    {
        return await this.Execute(async model =>
        {
            this.Storage.BeginTransaction();
            try
            {
                var entity = await model.Users.SingleOrDefaultAsync(u => u.Id == id)
                             ?? throw new NotFoundException(nameof(HfdCoreError.UserNotFound));

                var login = user.Username.ToUpperInvariant();
                var existingUser = await model.Users.SingleOrDefaultAsync(u => u.Username == login);
                if (existingUser != null && existingUser != entity)
                {
                    throw new HfdException(nameof(HfdCoreError.UserAlreadyExists));
                }

                entity.Username = login;
                entity.UniqueId = user.UniqueId;
                entity.FullName = user.FullName;
                entity.Language = Enum.Parse<Language>(user.Language);
                entity.Usertype = Enum.Parse<LoginRole>(user.Usertype);
                if (!string.IsNullOrEmpty(user.Password))
                {
                    entity.Password = user.Password.Hash();
                }

                await model.SaveChangesAsync();
                this.Storage.CommitTransaction();
                return entity;
            }
            catch
            {
                this.Storage.RollbackTransaction();
                throw;
            }
        });
    }

    /// <summary>
    ///     Изменить пользователя.
    /// </summary>
    /// <param name="password">Образ пароля для хранения в базе.</param>
    /// <returns>Асинхронный результат.</returns>
    public async Task ChangePassword(string password)
    {
        await this.Execute(async model =>
        {
            this.Storage.BeginTransaction();
            try
            {
                var entity = await model.Users.SingleOrDefaultAsync(u => u.Id == model.UserFull.Id)
                             ?? throw new NotFoundException(nameof(HfdCoreError.UserNotFound));

                entity.Password = password;
                await model.SaveChangesAsync();
                this.Storage.CommitTransaction();
                return entity;
            }
            catch
            {
                this.Storage.RollbackTransaction();
                throw;
            }
        });
    }

    /// <inheritdoc />
    public async Task<IUserFull> Block(long id, bool blocked)
    {
        return await this.Execute(async model =>
        {
            var entity = await model.Users.SingleOrDefaultAsync(u => u.Id == id)
                         ?? throw new NotFoundException(nameof(HfdCoreError.UserNotFound));

            if (entity.Blocked == blocked)
            {
                return entity;
            }

            entity.Blocked = blocked;

            await model.SaveChangesAsync();

            return entity;
        });
    }

    /// <inheritdoc />
    public async Task Delete(long id)
    {
        await this.Execute(async model =>
        {
            var entity = await model.Users.SingleOrDefaultAsync(u => u.Id == id)
                         ?? throw new NotFoundException(nameof(HfdCoreError.UserNotFound));

            if (entity.Username.Equals("admin", StringComparison.Ordinal))
            {
                throw new HfdException(nameof(HfdCoreError.UserAdminCannotBeDeleted));
            }

            model.Remove(entity);
            await model.SaveChangesAsync();
        });
    }
}
