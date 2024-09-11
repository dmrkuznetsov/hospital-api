using Hospital.Application.Common;
using Hospital.Application.Common.Enums;
using Hospital.Application.Common.Extenstions;
using Hospital.Application.Common.Interfaces;
using Hospital.Application.Doctors.Enums;
using Hospital.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Application.Doctors.Queries;

public class GetDoctorsSortedPagedQuery : IRequest<IEnumerable<Doctor>>
{
    public DoctorSortingField SortingField { get; }
    public OrderDirection OrderDirection { get; }
    public int ItemsPerPage { get; }
    public int Page { get; }

    public GetDoctorsSortedPagedQuery(DoctorSortingField field, OrderDirection direction, int itemsPerPage, int page)
    {
        SortingField = field;
        OrderDirection = direction;
        ItemsPerPage = itemsPerPage;
        Page = page;
    }

    public class GetDoctorsSortedPagedQueryHandler : BaseHandler, IRequestHandler<GetDoctorsSortedPagedQuery, IEnumerable<Doctor>>
    {
        public GetDoctorsSortedPagedQueryHandler(IHospitalDbContext dbContext) : base(dbContext) { }
        public async Task<IEnumerable<Doctor>> Handle(GetDoctorsSortedPagedQuery request, CancellationToken cancellationToken)
        {
            var doctorsQuery = DbContext.Doctors
                       .Include(x => x.MedicalCenter)
                       .Include(x => x.DoctorsOffice)
                       .Include(x => x.MedicalSpecialty);
            IOrderedQueryable<Doctor> orderedDoctorsQuery = null;
            switch (request.SortingField)
            {
                case DoctorSortingField.FullName:
                    orderedDoctorsQuery = doctorsQuery.OrderByWithDirection(x => x.FullName, request.OrderDirection);
                    break;
                case DoctorSortingField.MedicalSpecialty:
                    orderedDoctorsQuery = doctorsQuery.OrderByWithDirection(x => x.MedicalSpecialty.Name, request.OrderDirection);
                    break;
                case DoctorSortingField.DoctorsOffice:
                    orderedDoctorsQuery = doctorsQuery.OrderByWithDirection(x => x.DoctorsOffice.Number, request.OrderDirection);
                    break;
                case DoctorSortingField.MedicalCenter:
                    orderedDoctorsQuery = doctorsQuery.OrderByWithDirection(x => x.MedicalCenter.Number, request.OrderDirection);
                    break;
                default:
                    return new Doctor[] { };
            }
            return await orderedDoctorsQuery
                       .Skip(request.ItemsPerPage * request.Page)
                       .Take(request.ItemsPerPage)
                       .ToArrayAsync(cancellationToken);
        }
    }
}
