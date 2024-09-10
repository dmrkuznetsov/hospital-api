using Hospital.API.DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;
using Hospital.API.Models.Entities;
using Hospital.API.DataAccess.DTO;
using Microsoft.EntityFrameworkCore;
using Hospital.API.DataAccess.Enums;
using Hospital.API.Models.Enums;

namespace Hospital.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientsController : ControllerBase
    {
        private ApiMainContext _context;
        public PatientsController(ApiMainContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        [HttpPost(Name = "PostPatient")]
        public async Task<IActionResult> PostPatient(PatientDTO patientDto)
        {
            var medicalCenter = await _context.MedicalCenters.Where(x => x.Number == patientDto.MedicalCenterNumber).FirstOrDefaultAsync();
            if (medicalCenter is null)
            {
                medicalCenter = new MedicalCenter() { Number = patientDto.MedicalCenterNumber };
                _context.MedicalCenters.Add(medicalCenter);
            }
            var patient = new Patient()
            {
                Surname = patientDto.Surname,
                Name = patientDto.Name,
                Patronymic = patientDto.Patronymic,
                Address = patientDto.Address,
                BirthDate = patientDto.BirthDate,
                Gender = patientDto.Gender,
                MedicalCenter = medicalCenter
            };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
        }

        [HttpGet(template: "{id}", Name = "GetPatientById")]
        public async Task<ActionResult<PatientEditDTO>> GetPatient(Guid id)
        {
            var patient = await _context.Patients.Include(x=>x.MedicalCenter).FirstOrDefaultAsync(x=>x.Id == id);
            if (patient is null)
            {
                return NotFound();
            }
            return new PatientEditDTO(patient.Surname, patient.Name, patient.Patronymic, patient.Address, patient.BirthDate, patient.Gender, patient.MedicalCenter.Id);
        }

        [HttpGet(Name = "GetAllPatients")]
        public async Task<IEnumerable<PatientDTO>> GetPatients()
        {
            var patients = await _context.Patients.Include(x=>x.MedicalCenter).ToArrayAsync();
            return patients.Select(x => new PatientDTO(x.Surname, x.Name, x.Patronymic, x.Address, x.BirthDate, x.Gender, x.MedicalCenter.Number));
        }

        [HttpGet(template: "sorted", Name = "GetAllPatientsSorted")]
        public async Task<IEnumerable<PatientDTO>> GetDoctors([FromQuery] SortingField field, [FromQuery] OrderDirection direction, [FromQuery] int itemsPerPage, [FromQuery] int page)
        {
            var patientsQuery = _context.Patients
                   .Include(x => x.MedicalCenter);
            bool ascending = direction == OrderDirection.Ascending;
            Patient[] patients = Array.Empty<Patient>();
            switch (field)
            {
                case SortingField.Surname:
                    patients = ascending ?
                        await patientsQuery.OrderBy(x => x.Surname).Skip(itemsPerPage * page).Take(itemsPerPage).ToArrayAsync() :
                        await patientsQuery.OrderByDescending(x => x.Surname).Skip(itemsPerPage * page).Take(itemsPerPage).ToArrayAsync();
                    break;
                case SortingField.Name:
                    patients = ascending ?
                        await patientsQuery.OrderBy(x => x.Name).Skip(itemsPerPage * page).Take(itemsPerPage).ToArrayAsync() :
                        await patientsQuery.OrderByDescending(x => x.Name).Skip(itemsPerPage * page).Take(itemsPerPage).ToArrayAsync();
                    break;
                case SortingField.Patronimic:
                    patients = ascending ?
                        await patientsQuery.OrderBy(x => x.Patronymic).Skip(itemsPerPage * page).Take(itemsPerPage).ToArrayAsync() :
                        await patientsQuery.OrderByDescending(x => x.Patronymic).Skip(itemsPerPage * page).Take(itemsPerPage).ToArrayAsync();
                    break;
                case SortingField.Address:
                    patients = ascending ?
                        await patientsQuery.OrderBy(x => x.Address).Skip(itemsPerPage * page).Take(itemsPerPage).ToArrayAsync() :
                        await patientsQuery.OrderByDescending(x => x.Address).Skip(itemsPerPage * page).Take(itemsPerPage).ToArrayAsync();
                    break;
                case SortingField.BirthDate:
                    patients = ascending ?
                        await patientsQuery.OrderBy(x => x.BirthDate).Skip(itemsPerPage * page).Take(itemsPerPage).ToArrayAsync() :
                        await patientsQuery.OrderByDescending(x => x.BirthDate).Skip(itemsPerPage * page).Take(itemsPerPage).ToArrayAsync();
                    break;
                case SortingField.Gender:
                    patients = ascending ?
                        await patientsQuery.OrderBy(x => x.Gender).Skip(itemsPerPage * page).Take(itemsPerPage).ToArrayAsync() :
                        await patientsQuery.OrderByDescending(x => x.Gender).Skip(itemsPerPage * page).Take(itemsPerPage).ToArrayAsync();
                    break;
                case SortingField.MedicalCenter:
                    patients = ascending ?
                        await patientsQuery.OrderBy(x => x.MedicalCenter.Number).Skip(itemsPerPage * page).Take(itemsPerPage).ToArrayAsync() :
                        await patientsQuery.OrderByDescending(x => x.MedicalCenter.Number).Skip(itemsPerPage * page).Take(itemsPerPage).ToArrayAsync();
                    break;
                default:
                    return Array.Empty<PatientDTO>();
            }
            return patients.Select(x => new PatientDTO(x.Surname, x.Name, x.Patronymic, x.Address, x.BirthDate, x.Gender, x.MedicalCenter.Number));
        }

        [HttpPut(template: "{id}", Name = "PutPatientById")]
        public async Task<IActionResult> PutPatient(Guid id, PatientDTO patientDto)
        {
            var patient = await _context.Patients.Include(x => x.MedicalCenter).FirstOrDefaultAsync(x => x.Id == id);
            if (patient is null)
            {
                return NotFound();
            }
            patient.Surname = patientDto.Surname;
            patient.Name = patientDto.Name; 
            patient.Patronymic = patientDto.Patronymic;
            patient.Address = patientDto.Address;
            patient.BirthDate = patientDto.BirthDate;
            patient.Gender = patientDto.Gender;
            var medicalCenter = await _context.MedicalCenters.Where(x => x.Number == patientDto.MedicalCenterNumber).FirstOrDefaultAsync();
            if (medicalCenter is null)
            {
                medicalCenter = new MedicalCenter() { Number = patientDto.MedicalCenterNumber };
                _context.MedicalCenters.Add(medicalCenter);
            }
            patient.MedicalCenter = medicalCenter;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete(template: "{id}", Name = "DeletePatientById")]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
