using Hospital.API.Models.Entities;
using Hospital.API.Models.Enums;

namespace Hospital.API.DataAccess.DTO
{
    public class PatientDTO
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Address { get; set; }
        public DateOnly BirthDate { get; set; }
        public Gender Gender { get; set; }
        public int MedicalCenterNumber { get; set; }
        public PatientDTO(string surname, string name, string patronymic, string address, DateOnly birthDate, Gender gender, int medicalCenterNumber)
        {
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            Address = address;
            BirthDate = birthDate;
            Gender = gender;
            MedicalCenterNumber = medicalCenterNumber;
        }
    }
}
