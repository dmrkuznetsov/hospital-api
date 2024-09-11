using Hospital.Application.Common;
using Hospital.Application.Common.Interfaces;
using Hospital.Application.MedicalCenters.Commands;
using Hospital.Domain.Entities;
using Hospital.Domain.Enums;
using MediatR;

namespace Hospital.Application.Patients.Commands;

public class CreatePatientCommand : IRequest<Patient>
{
    public string Surname { get; }
    public string Name { get; }
    public string Patronymic { get; }
    public string Address { get; }
    public DateOnly BirthDate { get; }
    public Gender Gender { get; }
    public int MedicalCenterNumber { get; set; }
    public CreatePatientCommand(string surname, string name, string patronymic, string address, DateOnly birthData, Gender gender, int medicalCenterNumber)
    {
        Surname = surname;
        Name = name;
        Patronymic = patronymic;
        Address = address;
        BirthDate = birthData;
        Gender = gender;
        MedicalCenterNumber = medicalCenterNumber;
    }

    public class CreatePatientCommandHandler : BaseHandler, IRequestHandler<CreatePatientCommand, Patient>
    {
        private readonly IMediator _mediator;

        public CreatePatientCommandHandler(IHospitalDbContext dbContext, IMediator mediator) : base(dbContext) 
        {
            _mediator = mediator;
        }
        public async Task<Patient> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            var medicalCenter = await _mediator.Send(new CreateMedicalCenterCommand(request.MedicalCenterNumber, false), cancellationToken);
            var patient = new Patient
            {
                Surname = request.Surname,
                Name = request.Name,
                Patronymic = request.Patronymic,
                Address = request.Address,
                BirthDate = request.BirthDate,
                Gender = request.Gender,
                MedicalCenter = medicalCenter
            };
            await DbContext.Patients.AddAsync(patient, cancellationToken);
            await DbContext.SaveDbChangesAsync(cancellationToken);
            return patient;
        }
    }
}
