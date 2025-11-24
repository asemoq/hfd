using System.ComponentModel;

namespace Hfd.BizLayer.Features;

/// <summary>
///     Edit patient.
/// </summary>
public static class PatientEdit
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : IPatientInfo, IRequest<IPatientFull>
    {
        /// <summary>
        ///     Идентификатор.
        /// </summary>
        public long Id { get; set; }

        /// <inheritdoc />
        public string UserId { get; set; }

        /// <inheritdoc />
        public long? PlotId { get; set; }

        /// <inheritdoc />
        public string Scale { get; set; }

        /// <inheritdoc />
        public string Imt { get; set; }

        /// <inheritdoc />
        public int Klas { get; set; }

        /// <inheritdoc />
        [DefaultValue(false)]
        public bool Dead { get; set; }

        /// <inheritdoc />
        public string Gender { get; set; }

        /// <inheritdoc />
        public string Birthday { get; set; }

        /// <inheritdoc />
        public string Region { get; set; }

        /// <inheritdoc />
        public string Address { get; set; }

        /// <inheritdoc />
        public string Diagnosis { get; set; }

        /// <inheritdoc />
        public string Diseases { get; set; }

        /// <inheritdoc />
        public string Height { get; set; }

        /// <inheritdoc />
        public string Weight { get; set; }

        /// <inheritdoc />
        public string FamilyFullName { get; set; }

        /// <inheritdoc />
        public string FamilyPhoneNumber { get; set; }
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage) : IRequestHandler<Request, IPatientFull>
    {
        /// <inheritdoc />
        public async Task<IPatientFull> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = storage.GetService<IPatientService>();
            return await service.Edit(request.Id, request);
        }

        /// <inheritdoc />
        public sealed class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                this.RuleFor(x => x.Id).GreaterThan(0).WithMessage(nameof(HfdError.PatientIdIsNotCorrect));
                this.RuleFor(x => x.UserId).NotEmpty().WithMessage(nameof(HfdError.PatientUserIdIsNotCorrect));
            }
        }
    }
}
