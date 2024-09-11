using Hospital.Application.Common;
using Hospital.Application.Common.Interfaces;
using Hospital.Application.MedicalCenters.Commands;
using Hospital.Domain.Entities;
using Hospital.Domain.Enums;
using MediatR;

namespace Hospital.Application.Patients.Commands;

public class UpdatePatientCommand : IRequest<Patient>
{
    public Guid Id { get; }
    public string Surname { get; }
    public string Name { get; }
    public string Patronymic { get; }
    public string Address { get; }
    public DateOnly BirthDate { get; }
    public Gender Gender { get; }
    public int MedicalCenterNumber { get; set; }

    public UpdatePatientCommand(Guid id, string surname, string name, string patronymic, string address, DateOnly birthDate, Gender gender, int medicalCenterNumber)
    {
        Id = id;
        Surname = surname;
        Name = name;
        Patronymic = patronymic;
        Address = address;
        BirthDate = birthDate;
        Gender = gender;
        MedicalCenterNumber = medicalCenterNumber;
    }

    public class UpdatePatientCommandHandler : BaseHandler, IRequestHandler<UpdatePatientCommand, Patient>
    {
        private IMediator _mediator;

        public UpdatePatientCommandHandler(IMediator mediator, IHospitalDbContext dbContext) : base(dbContext)
        {
            _mediator = mediator;
        }

        public async Task<Patient> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = await DbContext.Patients.FindAsync(request.Id);
            if (patient is null) return null;
            var medicalCenter = await _mediator.Send(new CreateMedicalCenterCommand(request.MedicalCenterNumber, false), cancellationToken);
            patient.Surname = request.Surname;
            patient.Name = request.Name;
            patient.Patronymic = request.Patronymic;
            patient.Address = request.Address;
            patient.BirthDate = request.BirthDate;
            patient.Gender = request.Gender;
            patient.MedicalCenter = medicalCenter;
            await DbContext.SaveDbChangesAsync(cancellationToken);
            return patient;
        }
    }
}
