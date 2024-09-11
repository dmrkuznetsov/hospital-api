namespace Hospital.API.DTOs.Doctor;

public class DoctorInfoDTO
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public int DoctorsOfficeNumber { get; set; }
    public string MedicalSpecialtyName { get; set; }
    public int MedicalCenterNumber { get; set; }
}
