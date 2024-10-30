using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hfd.Base.Contract.Services;

public interface IBizContext : IUserAccessor
{
}

public static class BizContextExtensions
{
	public static IMapper GetMapper(this IBizContext context)
	{
		return context.Services.GetRequiredService<IMapper>();
	}

	public static IMediator GetMediator(this IBizContext context)
	{
		return context.Services.GetRequiredService<IMediator>();
	}

	public static T GetService<T>(this IBizContext context)
	{
		return context.Services.GetRequiredService<T>();
	}
}