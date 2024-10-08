﻿using Hospital.Domain.Entities.Abstract;

namespace Hospital.Domain.Entities;

public class Doctor : BaseEntity
{
    public string FullName { get; set; }
    public DoctorsOffice DoctorsOffice { get; set; }
    public MedicalSpecialty MedicalSpecialty { get; set; }
    public MedicalCenter MedicalCenter { get; set; }
}
