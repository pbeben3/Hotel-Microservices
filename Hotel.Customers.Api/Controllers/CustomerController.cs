using Hotel.Common.CrossCutting.Dtos;
using Hotel.Customers.Api.Services;
using Hotel.Customers.CrossCutting.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Customers.Api.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly CustomerService _customerService;

        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IEnumerable<CustomerDto>> Read() => await _customerService.Get();

        [HttpGet("{id}")]
        public async Task<IActionResult> ReadById(Guid id)
        {
            var customerDto = await _customerService.GetById(id);
            if (customerDto == null)
                return NotFound();

            return Ok(customerDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var operationResult = await _customerService.Create(dto);

            return Ok(operationResult.Result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CustomerDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var operationResult = await _customerService.Update(dto);

            if (operationResult.Status == CrudOperationResultStatus.RecordNotFound)
                return NotFound();
            if (operationResult.Status != CrudOperationResultStatus.Success)
                return BadRequest();
            return Ok(operationResult.Result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var operationResult = await _customerService.Delete(id);

            if (operationResult.Status == CrudOperationResultStatus.RecordNotFound)
                return NotFound();
            if (operationResult.Status != CrudOperationResultStatus.Success)
                return BadRequest();

            return Ok(operationResult.Status);
        }
    }
}
