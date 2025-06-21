using System;
using System.Threading.Tasks;
using Hotel.Common.CrossCutting.Dtos;
using Hotel.Discounts.Api.Services;
using Hotel.Discounts.CrossCutting.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Discounts.Api.Controllers
{
    [Route("api/customer-discounts")]
    [ApiController]
    public class CustomerDiscountController : Controller
    {
        private readonly CustomerDiscountService _service;

        public CustomerDiscountController(CustomerDiscountService service)
        {
            _service = service;
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomerId(Guid customerId)
        {
            var list = await _service.GetByCustomerId(customerId);
            return Ok(list);
        }

        [HttpGet("customer/{customerId}/active")]
        public async Task<IActionResult> GetActiveForCustomer(Guid customerId)
        {
            var dto = await _service.GetActiveForCustomer(customerId);
            if (dto == null)
                return NotFound();
            return Ok(dto);
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignDiscountToCustomer([FromBody] AssignDiscountRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.AssignDiscountToCustomer(request.CustomerId, request.DiscountId);
            if (result.Status == CrudOperationResultStatus.RecordNotFound)
                return NotFound();
            if (result.Status != CrudOperationResultStatus.Success)
                return BadRequest();

            return Ok(result.Result);
        }

        [HttpPost("{id}/activate")]
        public async Task<IActionResult> ActivateDiscount(Guid id)
        {
            var result = await _service.SetActive(id);
            if (result.Status == CrudOperationResultStatus.RecordNotFound)
                return NotFound();
            if (result.Status != CrudOperationResultStatus.Success)
                return BadRequest();

            return Ok(result.Result);
        }

        [HttpPost("{id}/deactivate")]
        public async Task<IActionResult> DeactivateDiscount(Guid id)
        {
            var result = await _service.SetInactive(id);
            if (result.Status == CrudOperationResultStatus.RecordNotFound)
                return NotFound();
            if (result.Status != CrudOperationResultStatus.Success)
                return BadRequest();

            return Ok(result.Result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssignment(Guid id)
        {
            var result = await _service.DeleteAssignment(id);
            if (result.Status == CrudOperationResultStatus.RecordNotFound)
                return NotFound();
            if (result.Status != CrudOperationResultStatus.Success)
                return BadRequest();

            return Ok();
        }
    }

    public class AssignDiscountRequest
    {
        public Guid CustomerId { get; set; }
        public Guid DiscountId { get; set; }
    }
}
