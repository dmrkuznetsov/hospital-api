namespace Hospital.API.Models.Entities
{
    public class Doсtor : BaseEntity
    {
        public string FullName { get; set; }
        public DoctorsOffice DoctorsOffice { get; set; }
        public MedicalSpecialty MedicalSpecialty { get; set; }
        public MedicalCenter MedicalCenter { get; set; }
    }
}
