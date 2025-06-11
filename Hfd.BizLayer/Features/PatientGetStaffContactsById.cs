namespace Hfd.BizLayer.Features;

/// <summary>
///     Get patient staff contacts by id.
/// </summary>
public static class PatientGetStaffContactsById
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : IRequest<StaffContact>
    {
        public long PatientId { get; set; }
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage) : IRequestHandler<Request, StaffContact>
    {
        /// <inheritdoc />
        public async Task<StaffContact> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = storage.GetService<IPatientService>();
            var patient = await service.GetById(request.PatientId, true);
            var contacts = new StaffContact() { Nurse = patient.Nurse?.PhoneNumber! };
            return contacts;
        }

        /// <inheritdoc />
        public sealed class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                this.RuleFor(x => x.PatientId).GreaterThan(0).WithMessage(nameof(HfdError.HFDPatientIdIsNotCorrect));
            }
        }
    }

    public class StaffContact
    {
        /// <summary>
        ///     Nurse contact.
        /// </summary>
        public string Nurse { get; set; }

        /// <summary>
        ///     Psychologist contact.
        /// </summary>
        public string Psychologist { get; set; }

        /// <summary>
        ///     SocialWorker contact.
        /// </summary>
        public string SocialWorker { get; set; }
    }
}
