using Hospital.Application.Common.Enums;
using Hospital.Application.Doctors.Commands;
using Hospital.Application.Doctors.Enums;
using Hospital.Application.Doctors.Queries;
using Hospital.Web.DTOs.Doctor;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DoctorsController : ControllerBase
    {
        private IMediator _mediator;
        public DoctorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<DoctorInfoDTO>> PostDoctor(DoctorCreationDTO doctorDto)
        {
            var doctor = await _mediator.Send(new CreateDoctorCommand(doctorDto.FullName, doctorDto.DoctorsOfficeNumber, doctorDto.MedicalSpecialtyName, doctorDto.MedicalCenterNumber));
            if (doctor is null) return Conflict();
            return new DoctorInfoDTO { FullName = doctor.FullName, Id = doctor.Id };
        }

        [HttpGet(template: "{id}", Name = "GetDoctorById")]
        public async Task<ActionResult<DoctorWithDependenciesIdDTO>> GetDoctor(Guid id)
        {
            var doctor = await _mediator.Send(new GetDoctorByIdQuery(id));
            if (doctor is null || doctor.Id != id) return NotFound();
            return new DoctorWithDependenciesIdDTO
            {
                Id = doctor.Id,
                DoctorsOfficeId = doctor.DoctorsOffice.Id,
                MedicalCenterId = doctor.MedicalCenter.Id,
                MedicalSpecialtyId = doctor.MedicalSpecialty.Id
            };
        }

        [HttpGet(Name = "GetAllDoctors")]
        public async Task<ActionResult<IEnumerable<DoctorInfoDTO>>> GetDoctors()
        {
            var doctors = await _mediator.Send(new GetAllDoctorsQuery());
            if (doctors is null || !doctors.Any()) return NotFound();
            return doctors.Select(x => new DoctorInfoDTO
            {
                Id = x.Id,
                FullName = x.FullName,
                DoctorsOfficeNumber = x.DoctorsOffice.Number,
                MedicalCenterNumber = x.MedicalCenter.Number,
                MedicalSpecialtyName = x.MedicalSpecialty.Name,
            }).ToArray();
        }

        [HttpGet(template: "sorted", Name = "GetAllDoctorsSorted")]
        public async Task<ActionResult<IEnumerable<DoctorInfoDTO>>> GetDoctors([FromQuery] DoctorSortingField field, [FromQuery] OrderDirection direction, [FromQuery] int itemsPerPage, [FromQuery] int page)
        {
            var doctors = await _mediator.Send(new GetDoctorsSortedPagedQuery(field, direction, itemsPerPage, page));
            if (doctors is null || !doctors.Any()) return NotFound();
            return doctors.Select(x => new DoctorInfoDTO
            {
                Id = x.Id,
                FullName = x.FullName,
                DoctorsOfficeNumber = x.DoctorsOffice.Number,
                MedicalCenterNumber = x.MedicalCenter.Number,
                MedicalSpecialtyName = x.MedicalSpecialty.Name,
            }).ToArray();
        }

        [HttpPut(Name = "PutDoctorById")]
        public async Task<ActionResult> PutDoctor(DoctorInfoDTO doctorDto)
        {
            var doctor = await _mediator.Send(new UpdateDoctorCommand(doctorDto.Id, doctorDto.FullName, doctorDto.DoctorsOfficeNumber, doctorDto.MedicalSpecialtyName, doctorDto.MedicalCenterNumber));
            if (doctor is null) return NotFound();
            return Ok();
        }

        [HttpDelete(template: "{id}", Name = "DeleteDoctorById")]
        public async Task<IActionResult> DeleteDoctor(Guid id)
        {
            var res = await _mediator.Send(new DeleteDoctorCommand(id));
            if (!res) return NotFound();
            return NoContent();
        }
    }
}
