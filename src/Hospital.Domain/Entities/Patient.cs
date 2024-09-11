using Hospital.Domain.Entities.Abstract;
using Hospital.Domain.Enums;

namespace Hospital.Domain.Entities;

public class Patient : BaseEntity
{
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Patronymic { get; set; }
    public string Address { get; set; }
    public DateOnly BirthDate { get; set; }
    public Gender Gender { get; set; }
    public MedicalCenter MedicalCenter { get; set; }
}
