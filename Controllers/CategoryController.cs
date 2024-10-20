using examWithXML.Entities;
using examWithXML.Services.CategoryService;
using Microsoft.AspNetCore.Mvc;

namespace examWithXML.Controllers;

[Route("api/categories")]
[ApiController]

public class CategoryController : ControllerBase
{
    private readonly ICategoryService categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        this.categoryService = categoryService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IResult> GetCategories()
    {
        return Results.Ok(await categoryService.GetCategoriesAsync());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetCategoryByIdAsync(int id)
    {
        if (id <= 0) return Results.BadRequest();
        var category = await categoryService.GetCategoryByIdAsync(id);
        if (category == null) return Results.NotFound();
        return Results.Ok(category);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> CreateCategoryAsync([FromBody] Category category)
    {
        if (category == null) return Results.BadRequest("Failed to create category.");
        await categoryService.CreateCategoryAsync(category);
        return Results.Created($"/api/categories/{category.Id}", "Category created successfully.");
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> UpdateCategoryAsync([FromBody] Category category)
    {
        if (category == null) return Results.BadRequest("Failed to update category.");
        if (await categoryService.GetCategoryByIdAsync(category.Id) == null) return Results.NotFound();
        await categoryService.UpdateCategoryAsync(category);
        return Results.Ok("Category updated successfully.");
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteCategoryAsync(int id)
    {
        if (await categoryService.DeleteCategoryAsync(id) == false) return Results.NotFound("Category with this ID doesn't exist");
        return Results.Ok("Category deleted successfully.");
    }
    
    [HttpGet("/withProductCount")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IResult> GetCategoryWithProdAsync()
    {
        return Results.Ok(await categoryService.GetCategoryWithProdAsync());
    }
}