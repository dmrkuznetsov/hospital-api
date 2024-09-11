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
    #endregion

    #region Команды

    #region Получить всех пациентов

    public ICommand GetPatientsCommand { get; }
    public async void OnGetPatients(object _)
    {
        try
        {
            ActiveAction = true;
            var patients = await DataAccessHelper.GetCall<IEnumerable<PatientInfoDTO>>($"{DataAccessConstants.BaseUri}{DataAccessConstants.PatientsUri}");
            Patients.Clear();
            foreach (var p in patients)
            {
                Patients.Add(new PatientVM(p));
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

    #region Получить всех врачей

    public ICommand GetDoctortsCommand { get; }
    public async void OnGetDoctors(object _)
    {
        try
        {
            ActiveAction = true;
            var doctors = await DataAccessHelper.GetCall<IEnumerable<DoctorInfoDTO>>($"{DataAccessConstants.BaseUri}{DataAccessConstants.DoctorsUri}");
            Doctors.Clear();
            foreach (var d in doctors)
            {
                Doctors.Add(new DoctorVM(d));
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


    #endregion

    #region Конструктор
    public MainVM()
    {
        GetPatientsCommand = new RelayCommand(OnGetPatients, CanGetPatients);
        GetDoctortsCommand = new RelayCommand(OnGetDoctors, CanGetDoctors);
    }
    #endregion
}
