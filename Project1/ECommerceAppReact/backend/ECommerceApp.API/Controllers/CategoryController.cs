using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Application.Services;
using ECommerceApp.Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving categories", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound(new { message = "Category not found" });

            return Ok(category);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving category", error = ex.Message });
        }
    }

    [HttpPost]
    // [Authorize(Roles = "Admin")] // TODO: Re-enable after implementing real JWT
    public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
    {
        try
        {
            var category = await _categoryService.CreateCategoryAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error creating category", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    // [Authorize(Roles = "Admin")] // TODO: Re-enable after implementing real JWT
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDto dto)
    {
        try
        {
            var result = await _categoryService.UpdateCategoryAsync(id, dto);
            if (!result)
                return NotFound(new { message = "Category not found" });

            return Ok(new { message = "Category updated successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error updating category", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    // [Authorize(Roles = "Admin")] // TODO: Re-enable after implementing real JWT
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result)
                return NotFound(new { message = "Category not found" });

            return Ok(new { message = "Category deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error deleting category", error = ex.Message });
        }
    }
}
