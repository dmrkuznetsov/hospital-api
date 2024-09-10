using Hospital.Domain.Entities.Abstract;

namespace Hospital.API.Models.Entities;

public class MedicalSpecialty : BaseEntity
{
    public string Name { get; set; }
}
