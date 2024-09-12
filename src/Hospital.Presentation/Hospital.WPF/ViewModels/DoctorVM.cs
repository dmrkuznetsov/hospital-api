using Hospital.Web.DTOs.Doctor;
using Hospital.WPF.ViewModels.Abstract;

namespace Hospital.WPF.ViewModels;

public class DoctorVM : ObservableObject
{
    private DoctorInfoDTO _doctorInfo;
    public Guid Id => _doctorInfo.Id;
    public string FullName 
    {
        get => _doctorInfo.FullName;
        set
        {
            _doctorInfo.FullName = value;
            RaisePropertyChanged();
        }
    }
    public int DoctorsOfficeNumber
    {
        get => _doctorInfo.DoctorsOfficeNumber;
        set
        {
            _doctorInfo.DoctorsOfficeNumber = value;
            RaisePropertyChanged();
        }
    }
    public string MedicalSpecialtyName
    {
        get => _doctorInfo.MedicalSpecialtyName;
        set
        {
            _doctorInfo.MedicalSpecialtyName = value;
            RaisePropertyChanged();
        }
    }
    public int MedicalCenterNumber
    {
        get => _doctorInfo.MedicalCenterNumber;
        set
        {
            _doctorInfo.MedicalCenterNumber = value;
            RaisePropertyChanged();
        }
    }
    public DoctorInfoDTO Data => _doctorInfo;

    public DoctorVM(DoctorInfoDTO doctorInfo)
    {
        _doctorInfo = doctorInfo;
    }
    public DoctorVM()
    {
        _doctorInfo = new DoctorInfoDTO
        {
            FullName = "Иванов Иван Иванович",
            DoctorsOfficeNumber = 203,
            MedicalCenterNumber = 1024,
            MedicalSpecialtyName = "офтальмолог"
        };
    }
    public void UpdateData(DoctorInfoDTO dInfo)
    {
        _doctorInfo = dInfo;
    }
}
