using Hospital.Application.Common;
using Hospital.Application.Common.Interfaces;
using Hospital.Application.DoctorsOffices.Commands;
using Hospital.Application.MedicalCenters.Commands;
using Hospital.Application.MedicalSpecialties.Commands;
using Hospital.Domain.Entities;
using MediatR;

namespace Hospital.Application.Doctors.Commands;

public class UpdateDoctorCommand : IRequest<Doctor>
{
    public Guid Id { get; }
    public string FullName { get; }
    public int DoctorsOfficeNumber { get; }
    public string MedicalSpecialtyName { get; }
    public int MedicalCenterNumber { get; }

    public UpdateDoctorCommand(Guid id, string fullName, int doctorsOfficeNumber, string medicalSpecialtyName, int medicalCenterNumber)
    {
        Id = id;
        FullName = fullName;
        DoctorsOfficeNumber = doctorsOfficeNumber;
        MedicalSpecialtyName = medicalSpecialtyName;
        MedicalCenterNumber = medicalCenterNumber;
    }

    public class UpdateDoctorCommandHandler : BaseHandler, IRequestHandler<UpdateDoctorCommand, Doctor>
    {
        private IMediator _mediator;

        public UpdateDoctorCommandHandler(IMediator mediator, IHospitalDbContext dbContext) : base(dbContext)
        {
            _mediator = mediator;
        }

        public async Task<Doctor> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
        {
            var doctor = await DbContext.Doctors.FindAsync(request.Id, cancellationToken);
            if (doctor is null) return null;
            var doctorsOffice = await _mediator.Send(new CreateDoctorsOfficeCommand(request.DoctorsOfficeNumber, false), cancellationToken);
            var medicalCenter = await _mediator.Send(new CreateMedicalCenterCommand(request.MedicalCenterNumber, false), cancellationToken);
            var medicalSpecialty = await _mediator.Send(new CreateMedicalSpecialtyCommand(request.MedicalSpecialtyName, false), cancellationToken);
            doctor.FullName = request.FullName;
            doctor.DoctorsOffice = doctorsOffice;
            doctor.MedicalSpecialty = medicalSpecialty;
            doctor.MedicalCenter = medicalCenter;
            await DbContext.SaveDbChangesAsync(cancellationToken);
            return doctor;
        }
    }
}
