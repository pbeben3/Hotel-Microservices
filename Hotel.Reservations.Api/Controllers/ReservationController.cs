using Hotel.Common.CrossCutting.Dtos;
using Hotel.Reservations.Api.Services;
using Hotel.Reservations.CrossCutting.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Reservations.Api.Controllers
{
    [Route("api/reservations")]
    [ApiController]
    public class ReservationController : Controller
    {
        private readonly ReservationService _reservationService;

        public ReservationController(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        public async Task<IEnumerable<ReservationDto>> GetAll()
        {
            return await _reservationService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var reservation = await _reservationService.GetById(id);
            if (reservation == null)
                return NotFound();

            return Ok(reservation);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IEnumerable<ReservationDto>> GetByCustomerId(Guid customerId)
        {
            return await _reservationService.GetByCustomerId(customerId);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReservationRequest dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _reservationService.Create(dto);
            if (result.Status != CrudOperationResultStatus.Success)
                return BadRequest(result.Status);

            return Ok(result.Result);
        }

        [HttpPost("{id}/confirm")]
        public async Task<IActionResult> Confirm(Guid id)
        {
            var result = await _reservationService.ConfirmReservation(id);
            if (result.Status == CrudOperationResultStatus.RecordNotFound)
                return NotFound();
            if (result.Status != CrudOperationResultStatus.Success)
                return BadRequest();

            return Ok(result.Result);
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var result = await _reservationService.CancelReservation(id);
            if (result.Status == CrudOperationResultStatus.RecordNotFound)
                return NotFound();
            if (result.Status != CrudOperationResultStatus.Success)
                return BadRequest();

            return Ok(result.Result);
        }
    }

    public class CreateReservationRequest
    {
        public Guid CustomerId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
