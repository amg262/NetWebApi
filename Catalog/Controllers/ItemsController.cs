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
    private readonly IItemsRepository _repository;

    // Dependency Injection
    public ItemsController(IItemsRepository repository, ILogger<ItemsController> loggerStubObject)
    {
        this._repository = repository;
    }

    // GET /items
    [HttpGet]
    public async Task<IEnumerable<ItemDto>> GetItemsAsync()
    {
        var items = (await _repository.GetItemsAsync()).Select(item =>
            item.AsDto()); // Do the first task and then the second task
        return items; // 200
    }

    // GET /items/{id}
    // [HttpGet("{id:guid}")] - this will make sure the id is a guid but for testing we wont use it here
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
    {
        var item = await _repository.GetItemAsync(id);

        if (item is null)
        {
            return NotFound(); // 404
        }

        return item.AsDto(); // 200
    }

    // POST /items
    [HttpPost]
    public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
    {
        Item item = new()
        {
            Id = Guid.NewGuid(),
            Name = itemDto.Name,
            Price = itemDto.Price,
            CreatedDate = DateTimeOffset.UtcNow
        };

        await _repository.CreateItemAsync(item);

        // We need to pass the action name to CreatedAtAction() in this way
        // If we use CreatedAtRoute() we dont need to pass the action name or use this
        // But CreateAtRoute wasn't working for me
        var action = nameof(GetItemAsync);

        // Can use CreatedAtAction() or CreatedAtRoute()
        //return CreatedAtRoute(action, new {id = item.Id}, item.AsDto());
        return CreatedAtAction(action, new {id = item.Id}, item.AsDto()); // 201
    }

    // PUT /items/{id}
    // [HttpPut("{id:guid}")] - this will make sure the id is a guid but for testing we wont use it here
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
    {
        var existingItem = await _repository.GetItemAsync(id);

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

        await _repository.UpdateItemAsync(updatedItem);

        // It is convention to return NoContent() when updating
        return NoContent(); // 204
    }

    // DELETE /items/{id}
    // [HttpDelete("{id:guid}")] - this will make sure the id is a guid but for testing we wont use it here
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteItem(Guid id)
    {
        var existingItem = await _repository.GetItemAsync(id); // Get the item first

        if (existingItem is null)
        {
            return NotFound(); // 404
        }

        await _repository.DeleteItemAsync(id); // Delete the item

        // It is convention to return NoContent() when deleting
        return NoContent(); // 204
    }
}