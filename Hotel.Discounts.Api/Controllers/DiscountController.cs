using Hotel.Common.CrossCutting.Dtos;
using Hotel.Discounts.Api.Services;
using Hotel.Discounts.CrossCutting.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Discounts.Api.Controllers
{
    [Route("api/discounts")]
    [ApiController]
    public class DiscountController : Controller
    {
        private readonly DiscountService _discountService;

        public DiscountController(DiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        public async Task<IEnumerable<DiscountDto>> GetAll()
        {
            return await _discountService.Get();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var discount = await _discountService.GetById(id);
            if (discount == null)
                return NotFound();

            return Ok(discount);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DiscountDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _discountService.Create(dto);
            if (result.Status != CrudOperationResultStatus.Success)
                return BadRequest();

            return Ok(result.Result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] DiscountDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _discountService.Update(dto);
            if (result.Status != CrudOperationResultStatus.Success)
                return BadRequest();

            return Ok(result.Result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _discountService.Delete(id);
            if (result.Status == CrudOperationResultStatus.RecordNotFound)
                return NotFound();

            return Ok(result.Status);
        }
    }
}

