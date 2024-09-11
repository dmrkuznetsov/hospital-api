using Hospital.Application.Common;
using Hospital.Application.Common.Interfaces;
using Hospital.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Application.MedicalSpecialties.Commands;

public class CreateMedicalSpecialtyCommand : IRequest<MedicalSpecialty>
{
    public string SpecialtyName { get; }
    public bool SaveChanges { get; }
    public CreateMedicalSpecialtyCommand(string specialty, bool saveChanges)
    {
        SpecialtyName = specialty;
        SaveChanges = saveChanges;
    }
    public class CreateMedicalSpecialtyCommandHandler : BaseHandler, IRequestHandler<CreateMedicalSpecialtyCommand, MedicalSpecialty>
    {

        public CreateMedicalSpecialtyCommandHandler(IHospitalDbContext dbContext) : base(dbContext) { }
        public async Task<MedicalSpecialty> Handle(CreateMedicalSpecialtyCommand request, CancellationToken cancellationToken)
        {
            var medicalSpecialty = await DbContext.MedicalSpecialities.FirstOrDefaultAsync(x=>x.Name.Equals(request.SpecialtyName, StringComparison.OrdinalIgnoreCase), cancellationToken);
            if (medicalSpecialty is null)
            {
                medicalSpecialty = new MedicalSpecialty
                {
                    Name = request.SpecialtyName
                };
                await DbContext.MedicalSpecialities.AddAsync(medicalSpecialty, cancellationToken);
                if (request.SaveChanges) await DbContext.SaveDbChangesAsync(cancellationToken);
            }
            return medicalSpecialty;
        }
    }
}
