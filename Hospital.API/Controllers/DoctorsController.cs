using Hospital.API.DataAccess.Contexts;
using Hospital.API.DataAccess.DTO;
using Hospital.API.DataAccess.Enums;
using Hospital.API.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DoctorsController : ControllerBase
    {
        private ApiMainContext _context;
        public DoctorsController(ApiMainContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        [HttpPost]
        public async Task<IActionResult> PostDoctor(DoctorDTO doctorDto)
        {
            var doctorsOffice = await _context.DoctorsOffices.Where(x => x.Number == doctorDto.DoctorsOfficeNumber).FirstOrDefaultAsync();
            if (doctorsOffice is null)
            {
                doctorsOffice = new DoctorsOffice() { Number = doctorDto.DoctorsOfficeNumber };
            }
            var medicalSpecialty = await _context.MedicalSpecialities
                .Where(x=>String.Equals(x.Name, doctorDto.MedicalSpecialtyName, StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync();
            if (medicalSpecialty is null)
            {
                medicalSpecialty = new MedicalSpecialty() { Name = doctorDto.MedicalSpecialtyName };
            }
            var medicalCenter = await _context.MedicalCenters.Where(x => x.Number == doctorDto.MedicalCenterNumber).FirstOrDefaultAsync();
            if (medicalCenter is null)
            {
                medicalCenter = new MedicalCenter() { Number = doctorDto.MedicalCenterNumber };
            }
            var doctor = new Doctor()
            {
                FullName = doctorDto.FullName,
                DoctorsOffice = doctorsOffice,
                MedicalSpecialty = medicalSpecialty,
                MedicalCenter = medicalCenter
            };
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDoctor), new { id = doctor.Id }, doctor);
        }

        [HttpGet(template: "{id}", Name = "GetDoctorById")]
        public async Task<ActionResult<DoctorEditDTO>> GetDoctor(Guid id)
        {
            var doctor = await _context.Doctors
                .Include(x => x.DoctorsOffice)
                .Include(x => x.MedicalSpecialty)
                .Include(x => x.MedicalCenter).FirstOrDefaultAsync(x => x.Id == id);
            if (doctor is null)
            {
                return NotFound();
            }
            return new DoctorEditDTO(doctor.FullName, doctor.DoctorsOffice.Id, doctor.MedicalSpecialty.Id, doctor.MedicalCenter.Id);
        }

        [HttpGet(Name = "GetAllDoctors")]
        public async Task<IEnumerable<DoctorDTO>> GetDoctors()
        {
            var doctors = await _context.Doctors
                .Include(x => x.DoctorsOffice)
                .Include(x => x.MedicalSpecialty)
                .Include(x => x.MedicalCenter).ToArrayAsync();
            return doctors.Select(x => new DoctorDTO(x.FullName, x.DoctorsOffice.Number, x.MedicalSpecialty.Name, x.MedicalCenter.Number));
        }

        [HttpGet(template: "sorted", Name = "GetAllDoctorsSorted")]
        public async Task<IEnumerable<DoctorDTO>> GetDoctors([FromQuery] SortingField field, [FromQuery] OrderDirection direction, [FromQuery] int itemsPerPage, [FromQuery] int page)
        {
            var doctorsQuery = _context.Doctors
                   .Include(x => x.DoctorsOffice)
                   .Include(x => x.MedicalSpecialty)
                   .Include(x => x.MedicalCenter);
            bool ascending = direction == OrderDirection.Ascending;
            Doctor[] doctors = Array.Empty<Doctor>();
            switch (field)
            {
                case SortingField.FullName:
                    doctors = ascending ?
                        await doctorsQuery.OrderBy(x => x.FullName).Skip(itemsPerPage * page).Take(itemsPerPage).ToArrayAsync() :
                        await doctorsQuery.OrderByDescending(x => x.FullName).Skip(itemsPerPage * page).Take(itemsPerPage).ToArrayAsync();
                    break;
                case SortingField.DoctorsOffice:
                    doctors = ascending ?
                        await doctorsQuery.OrderBy(x => x.DoctorsOffice.Number).Skip(itemsPerPage * page).Take(itemsPerPage).ToArrayAsync() :
                        await doctorsQuery.OrderByDescending(x => x.DoctorsOffice.Number).Skip(itemsPerPage * page).Take(itemsPerPage).ToArrayAsync();
                    break;
                case SortingField.MedicalSpecialty:
                    doctors = ascending ? 
                        await doctorsQuery.OrderBy(x => x.MedicalSpecialty.Name).Skip(itemsPerPage * page).Take(itemsPerPage).ToArrayAsync() :
                        await doctorsQuery.OrderByDescending(x => x.MedicalSpecialty.Name).Skip(itemsPerPage * page).Take(itemsPerPage).ToArrayAsync();
                    break;
                case SortingField.MedicalCenter:
                    doctors = ascending ? 
                        await doctorsQuery.OrderBy(x => x.MedicalCenter.Number).Skip(itemsPerPage * page).Take(itemsPerPage).ToArrayAsync() :
                        await doctorsQuery.OrderByDescending(x => x.MedicalCenter.Number).Skip(itemsPerPage * page).Take(itemsPerPage).ToArrayAsync();
                    break;
                default:
                    return Array.Empty<DoctorDTO>();
            }
            return doctors.Select(x => new DoctorDTO(x.FullName, x.DoctorsOffice.Number, x.MedicalSpecialty.Name, x.MedicalCenter.Number));
        }

        [HttpPut(template: "{id}", Name = "PutDoctorById" )]
        public async Task<IActionResult> PutDoctor(Guid id, DoctorDTO doctorDto)
        {
            var doctor = await _context.Doctors
               .Include(x => x.DoctorsOffice)
               .Include(x => x.MedicalSpecialty)
               .Include(x => x.MedicalCenter).FirstOrDefaultAsync(x => x.Id == id);
            if (doctor is null)
            {
                return NotFound();
            }
            var doctorsOffice = await _context.DoctorsOffices.Where(x => x.Number == doctorDto.DoctorsOfficeNumber).FirstOrDefaultAsync();
            if (doctorsOffice is null)
            {
                doctorsOffice = new DoctorsOffice() { Number = doctorDto.DoctorsOfficeNumber };
            }
            var medicalSpecialty = await _context.MedicalSpecialities
                .Where(x => String.Equals(x.Name, doctorDto.MedicalSpecialtyName, StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync();
            if (medicalSpecialty is null)
            {
                medicalSpecialty = new MedicalSpecialty() { Name = doctorDto.MedicalSpecialtyName };
            }
            var medicalCenter = await _context.MedicalCenters.Where(x => x.Number == doctorDto.MedicalCenterNumber).FirstOrDefaultAsync();
            if (medicalCenter is null)
            {
                medicalCenter = new MedicalCenter() { Number = doctorDto.MedicalCenterNumber };
            }
            doctor.FullName = doctorDto.FullName;
            doctor.DoctorsOffice = doctorsOffice;
            doctor.MedicalSpecialty = medicalSpecialty;
            doctor.MedicalCenter = medicalCenter;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete(template:"{id}", Name = "DeleteDoctorById")]
        public async Task<IActionResult> DeleteDoctor(Guid id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
