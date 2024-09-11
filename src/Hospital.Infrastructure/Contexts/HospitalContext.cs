using Hospital.Application.Common.Interfaces;
using Hospital.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.Contexts
{
    public class HospitalContext : DbContext, IHospitalDbContext
    {
        public DbSet<MedicalCenter> MedicalCenters { get; set; }
        public DbSet<MedicalSpecialty> MedicalSpecialities { get; set; }
        public DbSet<DoctorsOffice> DoctorsOffices { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public HospitalContext(DbContextOptions<HospitalContext> options) : base(options)
        {

        }
        public async Task SaveDbChangesAsync(CancellationToken cancellationToken = default)
        {
            await SaveChangesAsync(cancellationToken);
        }
    }
}
