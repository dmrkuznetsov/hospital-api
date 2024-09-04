using Hospital.API.Models.Entities;

namespace Hospital.API.DataAccess.DTO
{
    public class DoctorDTO
    {
        public string FullName { get; set; }
        public int DoctorsOfficeNumber { get; set; }
        public string MedicalSpecialtyName { get; set; }
        public int MedicalCenterNumber { get; set; }

        public DoctorDTO(string fullName, int doctorsOfficeNumber, string medicalSpecialtyName, int medicalCenterNumber)
        {
            FullName = fullName;
            DoctorsOfficeNumber = doctorsOfficeNumber;
            MedicalSpecialtyName = medicalSpecialtyName;
            MedicalCenterNumber = medicalCenterNumber;
        }
    }
}
