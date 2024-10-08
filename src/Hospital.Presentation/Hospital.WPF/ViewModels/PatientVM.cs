﻿using Hospital.Domain.Enums;
using Hospital.Web.DTOs.Patient;
using Hospital.WPF.ViewModels.Abstract;

namespace Hospital.WPF.ViewModels;

public class PatientVM : ObservableObject
{
    private PatientInfoDTO _patientInfo;
    public Guid Id { get => _patientInfo.Id; }
    public string Surname 
    {
        get => _patientInfo.Surname;
        set
        {
            _patientInfo.Surname = value;
            RaisePropertyChanged();
        }
    }
    public string Name 
    {
        get => _patientInfo.Name;
        set
        {
            _patientInfo.Name = value;
            RaisePropertyChanged();
        }
    }
    public string Patronymic 
    {
        get => _patientInfo.Patronymic;
        set
        {
            _patientInfo.Patronymic = value;
            RaisePropertyChanged();
        }
    }
    public string Address 
    {
        get => _patientInfo.Address;
        set
        {
            _patientInfo.Address = value;
            RaisePropertyChanged();
        }
    }
    public DateOnly BirthDate 
    {
        get => _patientInfo.BirthDate;
        set
        {
            _patientInfo.BirthDate = value;
            RaisePropertyChanged();
        }
    }
    public Gender Gender
    {
        get=>_patientInfo.Gender;
        set
        {
            _patientInfo.Gender = value;
            RaisePropertyChanged();
        }
    }
    public int MedicalCenterNumber 
    {
        get => _patientInfo.MedicalCenterNumber;
        set
        {
            _patientInfo.MedicalCenterNumber = value;
            RaisePropertyChanged();
        }
    }
    public PatientInfoDTO Data => _patientInfo;

    public PatientVM(PatientInfoDTO patientInfo)
    {
        _patientInfo = patientInfo;
    }

    public PatientVM()
    {
        _patientInfo = new PatientInfoDTO 
        {
            Surname = "Иванов",
            Name = "Иван",
            Patronymic = "Иванович",
            Address = "г.Неизвестный, ул.Гдетотам 23",
            BirthDate = new DateOnly(1994, 12, 20),
            Gender = Gender.Male,
            MedicalCenterNumber = 123,
        };
    }
    public void UpdateData(PatientInfoDTO pInfo)
    {
        _patientInfo = pInfo;
    }
}
