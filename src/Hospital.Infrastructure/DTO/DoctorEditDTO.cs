using Hospital.API.Models.Entities;

namespace Hospital.API.DataAccess.DTO
{
    public class DoctorEditDTO
    {
        public string FullName { get; set; }
        public Guid DoctorsOfficeId { get; set; }
        public Guid MedicalSpecialtyId { get; set; }
        public Guid MedicalCenterId { get; set; }
        public DoctorEditDTO(string fullName, Guid doctorsOfficeId, Guid medicalSpecialtyId, Guid medicalCenterId)
        {
            FullName = fullName;
            DoctorsOfficeId = doctorsOfficeId;
            MedicalSpecialtyId = medicalSpecialtyId;
            MedicalCenterId = medicalCenterId;
        }
    }
}
