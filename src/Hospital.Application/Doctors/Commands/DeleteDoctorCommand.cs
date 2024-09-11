using Hospital.Application.Common;
using Hospital.Application.Common.Interfaces;
using MediatR;

namespace Hospital.Application.Doctors.Commands;

public class DeleteDoctorCommand : IRequest<bool>
{
    public Guid Id { get; }

    public DeleteDoctorCommand(Guid id)
    {
        Id = id;
    }

    public class DeleteDoctorCommandHandler : BaseHandler, IRequestHandler<DeleteDoctorCommand, bool>
    {
        public DeleteDoctorCommandHandler(IHospitalDbContext dbContext) : base(dbContext) { } 

        public async Task<bool> Handle(DeleteDoctorCommand request, CancellationToken cancellationToken)
        {
            var doctor = await DbContext.Doctors.FindAsync(request.Id, cancellationToken);
            if (doctor is null) return false;
            DbContext.Doctors.Remove(doctor);
            await DbContext.SaveDbChangesAsync(cancellationToken);
            return true;
        }
    }
}
