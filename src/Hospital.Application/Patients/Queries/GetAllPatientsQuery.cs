using Hospital.Application.Common;
using Hospital.Application.Common.Interfaces;
using Hospital.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Application.Patients.Queries;

public class GetAllPatientsQuery : IRequest<IEnumerable<Patient>>
{
    public class GetAllPatientsQueryHandler : BaseHandler, IRequestHandler<GetAllPatientsQuery, IEnumerable<Patient>>
    {
        public GetAllPatientsQueryHandler(IHospitalDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Patient>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
        {
            return await DbContext.Patients
                .Include(x => x.MedicalCenter)
                .ToArrayAsync();
        }
    }
}
