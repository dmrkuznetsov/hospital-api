namespace Hospital.Web.DTOs.Doctor;

public class DoctorWithDependenciesIdDTO
{
    public Guid Id { get; set; }
    public Guid DoctorsOfficeId { get; set; }
    public Guid MedicalSpecialtyId { get; set; }
    public Guid MedicalCenterId { get; set; }
}
