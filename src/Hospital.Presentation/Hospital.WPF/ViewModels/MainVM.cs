using Hospital.Web.DTOs.Doctor;
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
    public bool PatientsAnyChanges 
    {
        get => _modifiedPatients.Any();
    }
    public bool DoctorsAnyChanges 
    {
        get => _modifiedDoctors.Any();
    }

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
    private List<DoctorVM> _modifiedDoctors= new List<DoctorVM>();
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
        if (!_modifiedPatients.Any()) return;
        try
        {
            ActiveAction = true;
            foreach (var p in _modifiedPatients.ToArray())
            {
                await DataAccessHelper.PutCall($"{DataAccessConstants.BaseUri}{DataAccessConstants.PatientsUri}", p.Data);
                _modifiedPatients.Remove(p);
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
    public bool CanSaveChangedPatients(object _) => PatientsAnyChanges && !ActiveAction; 
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
        if (!_modifiedDoctors.Any()) return;
        try
        {
            ActiveAction = true;
            foreach (var d in _modifiedDoctors.ToArray())
            {
                await DataAccessHelper.PutCall($"{DataAccessConstants.BaseUri}{DataAccessConstants.DoctorsUri}", d.Data);
                _modifiedDoctors.Remove(d);
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
    public bool CanSaveChangedDoctors(object _) => DoctorsAnyChanges && !ActiveAction;
    #endregion

    #endregion

    #region Конструктор
    public MainVM()
    {
        GetPatientsCommand = new RelayCommand(OnGetPatients, CanGetPatients);
        GetDoctortsCommand = new RelayCommand(OnGetDoctors, CanGetDoctors);
        SaveChangedPatientsCommand = new RelayCommand(OnSaveChangedPatients, CanSaveChangedPatients);
        SaveChangedDoctorsCommand = new RelayCommand(OnSaveChangedDoctors, CanSaveChangedDoctors);
    }
    #endregion
}
