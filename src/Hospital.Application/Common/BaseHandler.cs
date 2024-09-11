using Hospital.Application.Common.Interfaces;

namespace Hospital.Application.Common;

public abstract class BaseHandler
{
    protected IHospitalDbContext DbContext { get; }
    public BaseHandler(IHospitalDbContext dbContext)
    {
        DbContext = dbContext;
    }
}
