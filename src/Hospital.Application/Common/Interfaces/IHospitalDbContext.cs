using Hospital.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Application.Common.Interfaces;

public interface IHospitalDbContext
{
    public DbSet<MedicalCenter> MedicalCenters { get; set; }
    public DbSet<MedicalSpecialty> MedicalSpecialities { get; set; }
    public DbSet<DoctorsOffice> DoctorsOffices { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    Task SaveDbChangesAsync(CancellationToken cancellationToken = default);
}
