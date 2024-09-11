using Hospital.Application.Common;
using Hospital.Application.Common.Interfaces;
using Hospital.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Application.Patients.Queries;
public class GetPatientByIdQuery : IRequest<Patient>
{
    public Guid Id;
    public GetPatientByIdQuery(Guid id)
    {
        Id = id;
    }
    public class GetPatientByIdQueryHandler : BaseHandler, IRequestHandler<GetPatientByIdQuery, Patient>
    {
        public GetPatientByIdQueryHandler(IHospitalDbContext dbContext) : base(dbContext) { }
        public async Task<Patient> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
        {
            return await DbContext.Patients
                .Include(x => x.MedicalCenter)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        }
    }
}
