using Hospital.Application.Common;
using Hospital.Application.Common.Interfaces;
using MediatR;

namespace Hospital.Application.Patients.Commands;

public class DeletePatientCommand : IRequest<bool>
{
    public Guid Id { get; }

    public DeletePatientCommand(Guid id)
    {
        Id = id;
    }

    public class DeletePatientCommandHandler : BaseHandler, IRequestHandler<DeletePatientCommand, bool>
    {
        public DeletePatientCommandHandler(IHospitalDbContext dbContext) : base(dbContext) { } 

        public async Task<bool> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = await DbContext.Patients.FindAsync(request.Id, cancellationToken);
            if (patient is null) return false;
            DbContext.Patients.Remove(patient);
            await DbContext.SaveDbChangesAsync(cancellationToken);
            return true;
        }
    }
}
