using Hospital.Application.Common;
using Hospital.Application.Common.Interfaces;
using Hospital.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Application.MedicalCenters.Commands;

public class CreateMedicalCenterCommand : IRequest<MedicalCenter>
{
    //https://github.com/mgdagpin/Arc/blob/master/Src/Arc.Application/Employees/Commands/CreateEmployeeCommand.cs
    public int Number { get; set; }
    public bool SaveChanges { get; }
    public CreateMedicalCenterCommand(int number, bool saveChanges)
    {
        Number = number;
        SaveChanges = saveChanges;
    }
    public class CreateMedicalCenterCommandHandler : BaseHandler, IRequestHandler<CreateMedicalCenterCommand, MedicalCenter>
    {
        public CreateMedicalCenterCommandHandler(IHospitalDbContext dbContext) : base(dbContext) { }
        public async Task<MedicalCenter> Handle(CreateMedicalCenterCommand request, CancellationToken cancellationToken)
        {
            var medicalCenter = await DbContext.MedicalCenters.FirstOrDefaultAsync(x => x.Number == request.Number, cancellationToken);
            if (medicalCenter is null)
            {
                medicalCenter = new MedicalCenter
                {
                    Number = request.Number
                };
                await DbContext.MedicalCenters.AddAsync(medicalCenter, cancellationToken);
                if (request.SaveChanges) await DbContext.SaveDbChangesAsync(cancellationToken);
            }
            return medicalCenter;
        }
    }
}
