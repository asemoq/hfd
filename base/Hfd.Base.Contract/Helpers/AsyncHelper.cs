using System.Globalization;

// https://github.com/aspnet/AspNetIdentity/blob/master/src/Microsoft.AspNet.Identity.Core/AsyncHelper.cs.
//
// Copyright (c) Microsoft Corporation, Inc. All rights reserved.
// Licensed under the MIT License, Version 2.0. See License.txt in the project root for license information.
//
namespace Hfd.Base.Contract.Helpers;

/// <summary>
///     Методы помощники.
/// </summary>
public static class AsyncHelper
{
	private static readonly TaskFactory TaskFactory = new(
		CancellationToken.None,
		TaskCreationOptions.None,
		TaskContinuationOptions.None,
		TaskScheduler.Default);

	public static TResult RunSync<TResult>(Func<Task<TResult>> func)
	{
		var cultureUi = CultureInfo.CurrentUICulture;
		var culture = CultureInfo.CurrentCulture;
		return TaskFactory
			.StartNew(() =>
			{
				Thread.CurrentThread.CurrentCulture = culture;
				Thread.CurrentThread.CurrentUICulture = cultureUi;
				return func();
			})
			.Unwrap()
			.GetAwaiter()
			.GetResult();
	}

	public static void RunSync(Func<Task> func)
	{
		var cultureUi = CultureInfo.CurrentUICulture;
		var culture = CultureInfo.CurrentCulture;
		TaskFactory
			.StartNew(() =>
			{
				Thread.CurrentThread.CurrentCulture = culture;
				Thread.CurrentThread.CurrentUICulture = cultureUi;
				return func();
			})
			.Unwrap()
			.GetAwaiter()
			.GetResult();
	}
}