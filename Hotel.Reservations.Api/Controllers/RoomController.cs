using Hotel.Common.CrossCutting.Dtos;
using Hotel.Reservations.Api.Services;
using Hotel.Reservations.CrossCutting.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Reservations.Api.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomController : Controller
    {
        private readonly RoomService _roomService;

        public RoomController(RoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<IEnumerable<RoomDto>> Get()
        {
            return await _roomService.Get();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var room = await _roomService.GetById(id);
            if (room == null)
                return NotFound();

            return Ok(room);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoomDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _roomService.Create(dto);
            if (result.Status != CrudOperationResultStatus.Success)
                return BadRequest(result.Status);

            return Ok(result.Result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RoomDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _roomService.Update(dto);
            if (result.Status == CrudOperationResultStatus.RecordNotFound)
                return NotFound();
            if (result.Status != CrudOperationResultStatus.Success)
                return BadRequest();

            return Ok(result.Result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _roomService.Delete(id);
            if (result.Status == CrudOperationResultStatus.RecordNotFound)
                return NotFound();
            if (result.Status != CrudOperationResultStatus.Success)
                return BadRequest();

            return Ok(result.Status);
        }
    }
}
