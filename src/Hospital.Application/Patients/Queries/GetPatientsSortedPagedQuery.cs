using Hospital.Application.Common;
using Hospital.Application.Common.Enums;
using Hospital.Application.Common.Extenstions;
using Hospital.Application.Common.Interfaces;
using Hospital.Application.Patients.Enums;
using Hospital.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Application.Patients.Queries;

public class GetPatientsSortedPagedQuery : IRequest<IEnumerable<Patient>>
{
    public PatientSortingField SortingField { get; }
    public OrderDirection OrderDirection { get; }
    public int ItemsPerPage { get; }
    public int Page { get; }

    public GetPatientsSortedPagedQuery(PatientSortingField field, OrderDirection direction, int itemsPerPage, int page)
    {
        SortingField = field;
        OrderDirection = direction;
        ItemsPerPage = itemsPerPage;
        Page = page;
    }

    public class GetPatientsSortedPagedQueryHandler : BaseHandler, IRequestHandler<GetPatientsSortedPagedQuery, IEnumerable<Patient>>
    {
        public GetPatientsSortedPagedQueryHandler(IHospitalDbContext dbContext) : base(dbContext) { }
        public async Task<IEnumerable<Patient>> Handle(GetPatientsSortedPagedQuery request, CancellationToken cancellationToken)
        {
            var patientsQuery = DbContext.Patients
                       .Include(x => x.MedicalCenter);
            IOrderedQueryable<Patient> orderedPatientsQuery = null;
            switch (request.SortingField)
            {
                case PatientSortingField.Surname:
                    orderedPatientsQuery = patientsQuery.OrderByWithDirection(x => x.Surname, request.OrderDirection);
                    break;
                case PatientSortingField.Name:
                    orderedPatientsQuery = patientsQuery.OrderByWithDirection(x => x.Name, request.OrderDirection);
                    break;
                case PatientSortingField.Patronymic:
                    orderedPatientsQuery = patientsQuery.OrderByWithDirection(x => x.Patronymic, request.OrderDirection);
                    break;
                case PatientSortingField.Address:
                    orderedPatientsQuery = patientsQuery.OrderByWithDirection(x => x.Address, request.OrderDirection);
                    break;
                case PatientSortingField.BirthDate:
                    orderedPatientsQuery = patientsQuery.OrderByWithDirection(x => x.BirthDate, request.OrderDirection);
                    break;
                case PatientSortingField.Gender:
                    orderedPatientsQuery = patientsQuery.OrderByWithDirection(x => x.Gender, request.OrderDirection);
                    break;
                case PatientSortingField.MedicalCenter:
                    orderedPatientsQuery = patientsQuery.OrderByWithDirection(x => x.MedicalCenter.Number, request.OrderDirection);
                    break;

                default:
                    return new Patient[] { };
            }
            return await orderedPatientsQuery
                       .Skip(request.ItemsPerPage * request.Page)
                       .Take(request.ItemsPerPage)
                       .ToArrayAsync(cancellationToken);
        }
    }
}
