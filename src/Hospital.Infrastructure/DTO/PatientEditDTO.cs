using Hospital.API.Models.Entities;
using Hospital.API.Models.Enums;

namespace Hospital.API.DataAccess.DTO
{
    public class PatientEditDTO
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Address { get; set; }
        public DateOnly BirthDate { get; set; }
        public Gender Gender { get; set; }
        public Guid MedicalCenterId { get; set; }
        public PatientEditDTO(string surname, string name, string patronymic, string address, DateOnly birthDate, Gender gender, Guid medicalCenterId)
        {
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            Address = address;
            BirthDate = birthDate;
            Gender = gender;
            MedicalCenterId = medicalCenterId;
        }
    }
}
