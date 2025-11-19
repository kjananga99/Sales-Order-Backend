using Microsoft.AspNetCore.Mvc;
using SalesOrderAPI.Application.DTOs;
using SalesOrderAPI.Application.Interfaces;

namespace SalesOrderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesOrdersController : ControllerBase
    {
        private readonly ISalesOrderService _salesOrderService;

        public SalesOrdersController(ISalesOrderService salesOrderService)
        {
            _salesOrderService = salesOrderService;
        }

        // GET: api/salesorders
        [HttpGet]
        public async Task<IActionResult> GetAllSalesOrders()
        {
            var orders = await _salesOrderService.GetAllSalesOrdersAsync();
            return Ok(orders);
        }

        // GET: api/salesorders/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSalesOrderById(int id)
        {
            var order = await _salesOrderService.GetSalesOrderByIdAsync(id);
            if (order == null)
                return NotFound(new { message = "Sales order not found" });

            return Ok(order);
        }

        // POST: api/salesorders
        [HttpPost]
        public async Task<IActionResult> CreateSalesOrder([FromBody] CreateSalesOrderDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = await _salesOrderService.CreateSalesOrderAsync(createDto);
            return CreatedAtAction(nameof(GetSalesOrderById), new { id = order.Id }, order);
        }

        // PUT: api/salesorders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSalesOrder(int id, [FromBody] CreateSalesOrderDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = await _salesOrderService.UpdateSalesOrderAsync(id, updateDto);
            if (order == null)
                return NotFound(new { message = "Sales order not found" });

            return Ok(order);
        }

        // DELETE: api/salesorders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalesOrder(int id)
        {
            var result = await _salesOrderService.DeleteSalesOrderAsync(id);
            if (!result)
                return NotFound(new { message = "Sales order not found" });

            return NoContent();
        }
    }
}