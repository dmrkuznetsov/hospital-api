using Hospital.Domain.Enums;

namespace Hospital.API.DTOs.Patient;

public class PatientWithDependenciesIdDTO
{
    public Guid Id { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Patronymic { get; set; }
    public string Address { get; set; }
    public DateOnly BirthDate { get; set; }
    public Gender Gender { get; set; }
    public Guid MedicalCenterId { get; set; }
}
