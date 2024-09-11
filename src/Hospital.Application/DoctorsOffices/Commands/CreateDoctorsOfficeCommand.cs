using Hospital.Application.Common;
using Hospital.Application.Common.Interfaces;
using Hospital.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Application.DoctorsOffices.Commands;

public class CreateDoctorsOfficeCommand : IRequest<DoctorsOffice>
{
    public int Number { get; }
    public bool SaveChanges { get; }
    public CreateDoctorsOfficeCommand(int number, bool saveChanges)
    {
        Number = number; 
        SaveChanges = saveChanges;
    }

    public class CreateDoctorsOfficeCommandHander : BaseHandler, IRequestHandler<CreateDoctorsOfficeCommand, DoctorsOffice>
    {
        public CreateDoctorsOfficeCommandHander(IHospitalDbContext dbContext) : base(dbContext) { }
        public async Task<DoctorsOffice> Handle(CreateDoctorsOfficeCommand request, CancellationToken cancellationToken)
        {
            var doctorsOffice = await DbContext.DoctorsOffices.FirstOrDefaultAsync(x => x.Number == request.Number);
            if (doctorsOffice is null)
            {
                doctorsOffice = new DoctorsOffice
                {
                    Number = request.Number
                };
                await DbContext.DoctorsOffices.AddAsync(doctorsOffice, cancellationToken);
                if (request.SaveChanges) await DbContext.SaveDbChangesAsync(cancellationToken);
            }
            return doctorsOffice;
        }
    }
}
