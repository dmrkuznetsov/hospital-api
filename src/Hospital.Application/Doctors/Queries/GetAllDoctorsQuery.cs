using Hospital.Application.Common;
using Hospital.Application.Common.Interfaces;
using Hospital.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Application.Doctors.Queries;

public class GetAllDoctorsQuery : IRequest<IEnumerable<Doctor>>
{

    public class GetAllDoctorsQueryHandler : BaseHandler, IRequestHandler<GetAllDoctorsQuery, IEnumerable<Doctor>>
    {
        public GetAllDoctorsQueryHandler(IHospitalDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Doctor>> Handle(GetAllDoctorsQuery request, CancellationToken cancellationToken)
        {
            return await DbContext.Doctors
                       .Include(x => x.MedicalCenter)
                       .Include(x => x.DoctorsOffice)
                       .Include(x => x.MedicalSpecialty)
                       .ToArrayAsync(cancellationToken);
        }
    }
}
