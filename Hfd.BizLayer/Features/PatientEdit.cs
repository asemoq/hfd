using System.ComponentModel;
using Hfd.Common.Models;

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
    public sealed class Handler(IDbStorage storage, IUserService service, IBizContext context) : IRequestHandler<Request, IPatientFull>
    {
        /// <inheritdoc />
        public async Task<IPatientFull> Handle(Request request, CancellationToken cancellationToken)
        {
            if (request.User != null)
            {
                var user = await service.FindUser(request.User.PhoneNumber);
                if (user == null)
                {
                    request.User.Language = context.User.Language;
                    request.User.DefaultRole = UserRole.Patient;
                    request.User.Roles = [request.User.DefaultRole];
                    user = await service.CreateUser(request.User);
                }

                request.UserId = user.Id;
            }

            return await storage.GetService<IPatientService>().Edit(request.Id, request);
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
