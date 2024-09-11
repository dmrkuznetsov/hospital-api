using Hospital.Application.Common;
using Hospital.Application.Common.Interfaces;
using Hospital.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Application.Doctors.Queries;

public class GetDoctorByIdQuery : IRequest<Doctor>
{
    public Guid Id;  
    public GetDoctorByIdQuery(Guid id)
    {
        Id = id;
    }
    public class GetDoctorByIdQueryHandler : BaseHandler, IRequestHandler<GetDoctorByIdQuery, Doctor>
    {
        public GetDoctorByIdQueryHandler(IHospitalDbContext dbContext) : base(dbContext) { }
        public async Task<Doctor> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
        {
            return await DbContext.Doctors
                .Include(x=>x.MedicalCenter)
                .Include(x=>x.DoctorsOffice)
                .Include(x=>x.MedicalSpecialty)
                .FirstOrDefaultAsync(x=>x.Id == request.Id, cancellationToken);
        }
    }
}
