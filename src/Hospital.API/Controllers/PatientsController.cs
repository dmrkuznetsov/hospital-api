using Microsoft.AspNetCore.Mvc;
using MediatR;
using Hospital.API.DTOs.Patient;
using Hospital.Application.Patients.Commands;
using Hospital.Application.Patients.Queries;
using Hospital.Application.Patients.Enums;
using Hospital.Application.Common.Enums;

namespace Hospital.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientsController : ControllerBase
    {
        private IMediator _mediator;
        public PatientsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(Name = "PostPatient")]
        public async Task<ActionResult<PatientInfoDTO>> PostPatient(PatientCreationDTO patientDto)
        {
            var patient = await _mediator.Send(new CreatePatientCommand(patientDto.Surname, patientDto.Name, patientDto.Patronymic, patientDto.Address, patientDto.BirthDate, patientDto.Gender, patientDto.MedicalCenterNumber));
            if (patient is null) return Conflict();
            return new PatientInfoDTO { Id = patient.Id, Surname = patient.Surname, Name = patient.Name, Patronymic = patient.Patronymic, Address = patient.Address, BirthDate = patient.BirthDate, Gender = patient.Gender, MedicalCenterNumber = patient.MedicalCenter.Number };
        }

        [HttpGet(template: "{id}", Name = "GetPatientById")]
        public async Task<ActionResult<PatientWithDependenciesIdDTO>> GetPatient(Guid id)
        {
            var patient = await _mediator.Send(new GetPatientByIdQuery(id));
            if (patient is null || patient.Id != id) return NotFound();
            return new PatientWithDependenciesIdDTO
            {
                Id = patient.Id,
                Surname = patient.Surname,
                Name = patient.Name,
                Patronymic = patient.Patronymic,
                Address = patient.Address,
                BirthDate = patient.BirthDate,
                Gender = patient.Gender,
                MedicalCenterId = patient.MedicalCenter.Id
            };
        }

        [HttpGet(Name = "GetAllPatients")]
        public async Task<ActionResult<IEnumerable<PatientInfoDTO>>> GetPatients()
        {
            var patients = await _mediator.Send(new GetAllPatientsQuery());
            if (patients is null || !patients.Any()) return NotFound();
            return patients.Select(x => new PatientInfoDTO
            {
                Id = x.Id,
                Surname = x.Surname,
                Name = x.Name,
                Patronymic = x.Patronymic,
                Address = x.Address,
                BirthDate = x.BirthDate,
                Gender = x.Gender,
                MedicalCenterNumber = x.MedicalCenter.Number
            }).ToArray();
        }

        [HttpGet(template: "sorted", Name = "GetAllPatientsSorted")]
        public async Task<ActionResult<IEnumerable<PatientInfoDTO>>> GetDoctors([FromQuery] PatientSortingField field, [FromQuery] OrderDirection direction, [FromQuery] int itemsPerPage, [FromQuery] int page)
        {
            var patients = await _mediator.Send(new GetPatientsSortedPagedQuery(field, direction, itemsPerPage, page));
            if (patients is null || !patients.Any()) return NotFound();
            return patients.Select(x => new PatientInfoDTO
            {
                Id = x.Id,
                Surname = x.Surname,
                Name = x.Name,
                Patronymic = x.Patronymic,
                Address = x.Address,
                BirthDate = x.BirthDate,
                Gender = x.Gender,
                MedicalCenterNumber = x.MedicalCenter.Number
            }).ToArray();
        }

        [HttpPut(Name = "PutPatientById")]
        public async Task<IActionResult> PutPatient(PatientInfoDTO patientDto)
        {
            var patient = await _mediator.Send(new UpdatePatientCommand(patientDto.Id, patientDto.Surname, patientDto.Name, patientDto.Patronymic, patientDto.Address, patientDto.BirthDate, patientDto.Gender, patientDto.MedicalCenterNumber));
            if (patient is null) return NotFound();
            return Ok();
        }

        [HttpDelete(template: "{id}", Name = "DeletePatientById")]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            var res = await _mediator.Send(new DeletePatientCommand(id));
            if(!res) return NotFound();
            return NoContent();
        }
    }
}
