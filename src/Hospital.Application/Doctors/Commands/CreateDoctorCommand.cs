using Hospital.Application.Common;
using Hospital.Application.Common.Interfaces;
using Hospital.Application.DoctorsOffices.Commands;
using Hospital.Application.MedicalCenters.Commands;
using Hospital.Application.MedicalSpecialties.Commands;
using Hospital.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Application.Doctors.Commands;

public class CreateDoctorCommand : IRequest<Doctor>
{
    public string FullName { get;  }
    public int DoctorsOfficeNumber { get;}
    public string MedicalSpecialtyName { get; }
    public int MedicalCenterNumber { get; }
    public CreateDoctorCommand(string fullName, int doctorsOfficeNumber, string medicalSpecialtyName, int medicalCenterNumber)
    {
        FullName = fullName;
        DoctorsOfficeNumber = doctorsOfficeNumber;
        MedicalSpecialtyName = medicalSpecialtyName;
        MedicalCenterNumber = medicalCenterNumber;
    }
    public class CreateDoctorCommandHandler : BaseHandler, IRequestHandler<CreateDoctorCommand, Doctor>
    {
        private readonly IMediator _mediator;

        public CreateDoctorCommandHandler(IHospitalDbContext dbContext, IMediator mediator) : base(dbContext)
        {
            _mediator = mediator;
        }
        public async Task<Doctor> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
        {
            var doctor = await DbContext.Doctors.FirstOrDefaultAsync(x => x.FullName.Equals(request.FullName, StringComparison.OrdinalIgnoreCase), cancellationToken);
            if (doctor is null)
            {
                var doctorsOffice = await _mediator.Send(new CreateDoctorsOfficeCommand(request.DoctorsOfficeNumber, false), cancellationToken);
                var medicalCenter = await _mediator.Send(new CreateMedicalCenterCommand(request.MedicalCenterNumber, false), cancellationToken);
                var medicalSpecialty = await _mediator.Send(new CreateMedicalSpecialtyCommand(request.MedicalSpecialtyName, false), cancellationToken);
                doctor = new Doctor
                {
                    FullName = request.FullName,
                    DoctorsOffice = doctorsOffice,
                    MedicalCenter = medicalCenter,
                    MedicalSpecialty = medicalSpecialty
                };
                await DbContext.Doctors.AddAsync(doctor, cancellationToken);
                await DbContext.SaveDbChangesAsync(cancellationToken);
            }
            return doctor;
        }
    }
}
