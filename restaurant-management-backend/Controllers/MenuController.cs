using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using restaurant_management_backend.Dtos.Menu;
using restaurant_management_backend.Interfaces;

namespace restaurant_management_backend.Controllers
{
    [Route("api/menu")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuRepository _menuRepository;

        public MenuController(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        // --- Category Endpoints ---
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var response = await _menuRepository.GetCategories();
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("categories")]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public async Task<IActionResult> AddCategory(UpsertCategoryRequestDto dto)
        {
            var response = await _menuRepository.AddCategory(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("categories/{id}")]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public async Task<IActionResult> UpdateCategory(Guid id, UpsertCategoryRequestDto dto)
        {
            var response = await _menuRepository.UpdateCategory(id, dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("categories/{id}")]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var response = await _menuRepository.DeleteCategory(id);
            return StatusCode(response.StatusCode, response);
        }

        // --- MenuItem Endpoints ---
        [HttpGet("items")]
        public async Task<IActionResult> GetMenuItems()
        {
            var response = await _menuRepository.GetMenuItems();
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("items")]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public async Task<IActionResult> AddMenuItem(UpsertMenuItemRequestDto dto)
        {
            var response = await _menuRepository.AddMenuItem(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("items/{id}")]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public async Task<IActionResult> UpdateMenuItem(Guid id, UpsertMenuItemRequestDto dto)
        {
            var response = await _menuRepository.UpdateMenuItem(id, dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("items/{id}")]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public async Task<IActionResult> DeleteMenuItem(Guid id)
        {
            var response = await _menuRepository.DeleteMenuItem(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
