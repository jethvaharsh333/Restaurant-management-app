using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using restaurant_management_backend.Dtos.Table;
using restaurant_management_backend.Interfaces;
using restaurant_management_backend.Models.OrderAndOperations;

namespace restaurant_management_backend.Controllers
{
    [Route("api/tables")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly ITableRepository _repository;

        public TableController(ITableRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN,MANAGER,WAITER")]
        public async Task<IActionResult> GetTableLayout()
        {
            var response = await _repository.GetAllTablesAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ADMIN,MANAGER,WAITER")]
        public async Task<IActionResult> GetTableById(Guid id)
        {
            var response = await _repository.GetTableByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public async Task<IActionResult> CreateTable(UpsertTableRequestDto dto)
        {
            var response = await _repository.CreateTableAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public async Task<IActionResult> UpdateTable(Guid id, UpsertTableRequestDto dto)
        {
            var response = await _repository.UpdateTableAsync(id, dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public async Task<IActionResult> DeleteTable(Guid id)
        {
            var response = await _repository.DeleteTableAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "ADMIN,MANAGER,WAITER")]
        public async Task<IActionResult> UpdateTableStatus(Guid id, [FromBody] UpdateTableStatusDto dto)
        {
            var response = await _repository.UpdateTableStatusAsync(id, dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("reservations")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateReservation(CreateReservationDto dto)
        {
            var response = await _repository.CreateReservationAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("reservations/{id}/status")]
        [Authorize(Roles = "ADMIN,MANAGER,WAITER")]
        public async Task<IActionResult> UpdateReservationStatus(Guid id, [FromBody] UpdateReservationStatusDto dto)
        {
            var response = await _repository.UpdateReservationStatusAsync(id, dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("reservations")]
        [Authorize(Roles = "ADMIN,MANAGER,WAITER")]
        public async Task<IActionResult> GetReservations([FromQuery] DateTime date)
        {
            var response = await _repository.GetReservationsByDateAsync(date);
            return Ok(response);
        }

    }
}
