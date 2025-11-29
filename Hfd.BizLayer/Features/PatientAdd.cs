using System.ComponentModel;
using Hfd.Common.Contract.Exceptions;
using Hfd.Common.Models;

namespace Hfd.BizLayer.Features;

/// <summary>
///     Add patient.
/// </summary>
public static class PatientAdd
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : IPatientInfo, IRequest<IPatientFull>
    {
        /// <inheritdoc />
        public string UserId { get; set; }

        /// <inheritdoc />
        public long? PlotId { get; set; }

        /// <inheritdoc />
        public string Scale { get; set; }

        /// <inheritdoc />
        public int Klas { get; set; }

        /// <inheritdoc />
        [DefaultValue(false)]
        public bool Dead { get; set; }

        /// <inheritdoc />
        public Gender Gender { get; set; }

        /// <inheritdoc />
        public DateTime Birthday { get; set; }

        /// <inheritdoc />
        public string Region { get; set; }

        /// <inheritdoc />
        public string Address { get; set; }

        /// <inheritdoc />
        public string Diagnosis { get; set; }

        /// <inheritdoc />
        public string Diseases { get; set; }

        /// <inheritdoc />
        public double Height { get; set; }

        /// <inheritdoc />
        public double Weight { get; set; }

        /// <inheritdoc />
        public string FamilyFullName { get; set; }

        /// <inheritdoc />
        public string FamilyPhoneNumber { get; set; }

        public UserInfo? User { get; set; }
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage, IUserService service) : IRequestHandler<Request, IPatientFull>
    {
        /// <inheritdoc />
        public async Task<IPatientFull> Handle(Request request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.UserId))
            {
                request.UserId = await service.CreateOrFind(request.User, UserRole.Patient) ?? throw new UserNotFoundException();
            }

            return await storage.GetService<IPatientService>().Add(request);
        }

        /// <inheritdoc />
        public sealed class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                this.RuleFor(x => x.UserId)
                    .NotEmpty()
                    .When(x => x.User == null || string.IsNullOrEmpty(x.User.PhoneNumber))
                    .WithError(ErrorType.NotCorrect, HfdError.PatientUserId);
            }
        }
    }
}
