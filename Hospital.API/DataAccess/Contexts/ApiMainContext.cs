using Hospital.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hospital.API.DataAccess.Contexts
{
    public class ApiMainContext : DbContext
    {
        public DbSet<MedicalCenter> MedicalCenters { get; set; }
        public DbSet<MedicalSpecialty> MedicalSpecialities { get; set; }
        public DbSet<DoctorsOffice> DoctorsOffices { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public ApiMainContext(DbContextOptions<ApiMainContext> options) : base(options)
        {

        }
    }
}
