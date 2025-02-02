using System.Linq;
using System.Reflection;
using Hfd.Base.Contract.Helpers;
using Hfd.Base.Contract.Services;
using Hfd.Contract.Dto;
using Hfd.Contract.Enums;
using Hfd.Contract.Model;
using Hfd.Contract.Services;

namespace Hfd.BizLayer.Features;

public static class SurveyGetBySummary
{
	/// <summary>
	///     Request.
	/// </summary>
	public sealed class Request : IRequest<SummaryModel>
	{
		/// <summary>
		///     Id пациента.
		/// </summary>
		public long PatientId { get; set; }

		/// <summary>
		///     Месяц и год.
		/// </summary>
		public DateOnly Date { get; set; }
	}

	/// <inheritdoc />
	public sealed class Handler : IRequestHandler<Request, SummaryModel>
	{
		public readonly IBizContext _context;

		public Handler(IBizContext context)
		{
			_context = context;
		}

		/// <inheritdoc />
		public async Task<SummaryModel> Handle(Request rq, CancellationToken token)
		{
			var service = _context.Services.GetRequiredService<IDbStorage>().Get<ISurveyService>();

			var surveys = await service.GetForPeriod(rq.PatientId, rq.Date.Month, rq.Date.Year);

			var summary = new SummaryModel();
			var properties = new List<PropertyInfo>();
			foreach (var type in typeof(ISurveyFull).GetInterfaces())
			{
				properties.AddRange(type.GetProperties());
			}

			foreach (var row in properties.Select(property => DynamicRow.CreateInstance(property.Name)))
			{
				summary.Rows.Add(row);
			}

			var startDate = new DateOnly(rq.Date.Year, rq.Date.Month, 1);
			var currentDate = startDate;

			while (currentDate.Month == startDate.Month && currentDate.Year == startDate.Year)
			{
				foreach (DayTime dayTime in Enum.GetValues(typeof(DayTime)))
				{
					var columnName =
						$"{dayTime.ToString().ToLower()}_{currentDate.Day}_{currentDate.Month}_{currentDate.Year}";
					var column = new ColumnModel()
					{
						Name = columnName,
						Header = dayTime.ToString(),
						GroupHeader = $"{currentDate.Day} {currentDate.DayOfWeek.ToString()}"
					};
					summary.Columns.Add(column);
				}

				currentDate = currentDate.AddDays(1);
			}


			var grouped = surveys.GroupBy(x => x.DateTime);
			foreach (var group in grouped)
			{
				foreach (var item in group)
				{
					var columnName =
						$"{item.DayTime.ToString().ToLower()}_{item.DateTime.Day}_{item.DateTime.Month}_{item.DateTime.Year}";
					foreach (var property in properties)
					{
						var row = summary.Rows.SingleOrDefault(x => x.Name == property.Name.ToLowerFirstChar());
						var propertyValue = property.GetValue(item);
						if (row != null) row[columnName] = propertyValue;
					}
				}
			}

			return summary;
		}


		/// <inheritdoc />
		public sealed class Validator : AbstractValidator<Request>
		{
			public Validator()
			{
				RuleFor(x => x.PatientId).GreaterThan(0)
					.WithMessage(nameof(HfdError.HFD_Patient_Id_Is_Not_Correct));
			}
		}
	}
}