using Catalog.Dtos;
using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Route("items")]
// [Route("api/v{version:apiVersion}/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IItemsRepository repository;

    // Dependency Injection
    public ItemsController(IItemsRepository repository)
    {
        this.repository = repository;
    }

    // GET /items
    [HttpGet]
    public IEnumerable<ItemDto> GetItems()
    {
        var items = repository.GetItems().Select(item => item.AsDto());
        return items; // 200
    }

    // GET /items/{id}
    // [HttpGet("{id:guid}")] - this will make sure the id is a guid but for testing we wont use it here
    [HttpGet("{id}")]
    public ActionResult<ItemDto> GetItem(Guid id)
    {
        var item = repository.GetItem(id);

        if (item is null)
        {
            return NotFound(); // 404
        }

        return item.AsDto(); // 200
    }

    // POST /items
    [HttpPost]
    public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto)
    {
        Item item = new()
        {
            Id = Guid.NewGuid(),
            Name = itemDto.Name,
            Price = itemDto.Price,
            CreatedDate = DateTimeOffset.UtcNow
        };

        repository.CreateItem(item);

        // Can use CreatedAtAction() or CreatedAtRoute()
        //return CreatedAtRoute(nameof(GetItem), new {id = item.Id}, item.AsDto());
        return CreatedAtAction(nameof(GetItem), new {id = item.Id}, item.AsDto()); // 201
    }

    // PUT /items/{id}
    // [HttpPut("{id:guid}")] - this will make sure the id is a guid but for testing we wont use it here
    [HttpPut("{id}")]
    public ActionResult UpdateItem(Guid id, UpdateItemDto itemDto)
    {
        var existingItem = repository.GetItem(id);

        if (existingItem is null)
        {
            return NotFound(); // 404
        }

        // With Expression : Taking existingItem and updating it with the new values
        // This is addition to records that allows me to use immutable type but still modify it
        Item updatedItem = existingItem with
        {
            Name = itemDto.Name,
            Price = itemDto.Price
        };

        repository.UpdateItem(updatedItem);

        // It is convention to return NoContent() when updating
        return NoContent(); // 204
    }

    // DELETE /items/{id}
    // [HttpDelete("{id:guid}")] - this will make sure the id is a guid but for testing we wont use it here
    [HttpDelete("{id}")]
    public ActionResult DeleteItem(Guid id)
    {
        var existingItem = repository.GetItem(id);

        if (existingItem is null)
        {
            return NotFound(); // 404
        }

        repository.DeleteItem(id);

        // It is convention to return NoContent() when deleting
        return NoContent(); // 204
    }
}