﻿using Hospital.Web.DTOs.Doctor;
using Hospital.Web.DTOs.Patient;
using Hospital.WPF.DataAccess;
using Hospital.WPF.ViewModels.Abstract;
using Hospital.WPF.ViewModels.Commands;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Hospital.WPF.ViewModels;

public class MainVM : ObservableObject
{

    #region Поля-свойства
    public ObservableCollection<DoctorVM> Doctors { get; } = new ObservableCollection<DoctorVM>();
    public ObservableCollection<PatientVM> Patients { get; } = new ObservableCollection<PatientVM>();
    private bool _activeAction = false;
    public bool PatientsAnyChanges => _modifiedPatients.Any();
    public bool DoctorsAnyChanges => _modifiedDoctors.Any();
    public bool AnyDoctorsToAdd => _doctorsToAdd.Any();
    public bool AnyPatientsToAdd => _patientsToAdd.Any();

    public bool ActiveAction
    {
        get => _activeAction;
        set
        {
            _activeAction = value;
            RaisePropertyChanged();
            RaisePropertyChanged(nameof(NoActiveAction));
        }
    }
    public bool NoActiveAction
    {
        get => !_activeAction;
    }
    private List<PatientVM> _modifiedPatients = new List<PatientVM>();
    private List<DoctorVM> _modifiedDoctors = new List<DoctorVM>();
    private List<PatientVM> _patientsToAdd = new List<PatientVM>();
    private List<DoctorVM> _doctorsToAdd = new List<DoctorVM>();
    #endregion

    #region Команды

    #region Получить всех пациентов

    public ICommand GetPatientsCommand { get; }
    public async void OnGetPatients(object _)
    {
        try
        {
            ActiveAction = true;
            Patients.Clear();
            _modifiedPatients.Clear();
            var patients = await DataAccessHelper.GetCall<PatientInfoDTO[]>($"{DataAccessConstants.BaseUri}{DataAccessConstants.PatientsUri}");
            if(patients is null)
            {
                MessageBox.Show("No patients received", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            foreach (var p in patients)
            {
                var pvm = new PatientVM(p);
                Patients.Add(pvm);
                pvm.PropertyChanged += (sender, args) =>
                {
                    var patient = sender as PatientVM;
                    _modifiedPatients.Add(patient);
                    RaisePropertyChanged(nameof(PatientsAnyChanges));
                };
            }
        }
        catch(Exception ex) 
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            ActiveAction = false;
        }
    }
    public bool CanGetPatients(object _) => !ActiveAction;
    #endregion

    #region Сохранить изменения в таблице пациентов 
    public ICommand SaveChangedPatientsCommand { get; }

    public async void OnSaveChangedPatients(object _)
    {
        if (!_modifiedPatients.Any() && !_patientsToAdd.Any()) return;
        try
        {
            ActiveAction = true;
            foreach (var p in _modifiedPatients.ToArray())
            {
                await DataAccessHelper.PutCall($"{DataAccessConstants.BaseUri}{DataAccessConstants.PatientsUri}", p.Data);
                _modifiedPatients.Remove(p);
            }
            foreach (var p in _patientsToAdd.ToArray())
            {
                var updatedData = await DataAccessHelper.PostCall<PatientInfoDTO, PatientInfoDTO>($"{DataAccessConstants.BaseUri}{DataAccessConstants.PatientsUri}", p.Data);
                p.UpdateData(updatedData);
                _patientsToAdd.Remove(p);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            ActiveAction = false;
        }
    }
    public bool CanSaveChangedPatients(object _) => (PatientsAnyChanges || AnyPatientsToAdd) && !ActiveAction;
    #endregion

    #region Добавить пациента
    public ICommand AddPatientCommand { get; }
    public void OnAddPatient(object _)
    {
        var patient = new PatientVM();
        Patients.Add(patient);
        _patientsToAdd.Add(patient);
        RaisePropertyChanged(nameof(AnyPatientsToAdd));
    }
    public bool CanAddPatient(object _) => !ActiveAction;
    #endregion

    #region Получить всех врачей

    public ICommand GetDoctortsCommand { get; }
    public async void OnGetDoctors(object _)
    {
        try
        {
            ActiveAction = true;
            Doctors.Clear();
            var doctors = await DataAccessHelper.GetCall<DoctorInfoDTO[]>($"{DataAccessConstants.BaseUri}{DataAccessConstants.DoctorsUri}");
            if (doctors is null)
            {
                MessageBox.Show("No doctors received", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            foreach (var d in doctors)
            {
                var dvm = new DoctorVM(d);
                Doctors.Add(dvm);
                dvm.PropertyChanged += (sender, args) =>
                {
                    var doctor = sender as DoctorVM;
                    _modifiedDoctors.Add(doctor);
                    RaisePropertyChanged(nameof(DoctorsAnyChanges));
                };
            }
        }
        catch(Exception ex) 
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            ActiveAction = false;
        }
    }
    public bool CanGetDoctors(object _) => !ActiveAction;
    #endregion

    #region Сохранить изменения в таблице врачей
    public ICommand SaveChangedDoctorsCommand { get; }
    public async void OnSaveChangedDoctors(object _)
    {
        if (!_modifiedDoctors.Any() && !_doctorsToAdd.Any()) return;
        try
        {
            ActiveAction = true;
            foreach (var d in _modifiedDoctors.ToArray())
            {
                await DataAccessHelper.PutCall($"{DataAccessConstants.BaseUri}{DataAccessConstants.DoctorsUri}", d.Data);
                _modifiedDoctors.Remove(d);
            }
            foreach (var d in _doctorsToAdd.ToArray())
            {
                var updatedData = await DataAccessHelper.PostCall<DoctorInfoDTO, DoctorInfoDTO>($"{DataAccessConstants.BaseUri}{DataAccessConstants.DoctorsUri}", d.Data);
                d.UpdateData(updatedData);
                _doctorsToAdd.Remove(d);
            }
        }
        catch(Exception ex) 
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            ActiveAction = false;
        }

    }
    public bool CanSaveChangedDoctors(object _) => (DoctorsAnyChanges || AnyDoctorsToAdd) && !ActiveAction;
    #endregion

    #region Добавить врача
    public ICommand AddDoctorCommand { get; }
    public void OnAddDoctor(object _)
    {
        var doctor = new DoctorVM();
        Doctors.Add(doctor);
        _doctorsToAdd.Add(doctor);
        RaisePropertyChanged(nameof(AnyDoctorsToAdd));
    }
    public bool CanAddDoctor(object _) => !ActiveAction;
    #endregion

    #endregion

    #region Конструктор
    public MainVM()
    {
        GetPatientsCommand = new RelayCommand(OnGetPatients, CanGetPatients);
        GetDoctortsCommand = new RelayCommand(OnGetDoctors, CanGetDoctors);
        SaveChangedPatientsCommand = new RelayCommand(OnSaveChangedPatients, CanSaveChangedPatients);
        SaveChangedDoctorsCommand = new RelayCommand(OnSaveChangedDoctors, CanSaveChangedDoctors);
        AddPatientCommand = new RelayCommand(OnAddPatient, CanAddPatient);
        AddDoctorCommand = new RelayCommand(OnAddDoctor, CanAddDoctor);
    }
    #endregion
}
