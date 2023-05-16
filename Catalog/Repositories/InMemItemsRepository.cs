using Catalog.Entities;
using System.Linq;

namespace Catalog.Repositories;

public class InMemItemsRepository : IItemsRepository
{
    private readonly List<Item> items = new()
    {
        new Item {Id = Guid.NewGuid(), Name = "Potion", Price = 9, CreatedDate = DateTimeOffset.UtcNow},
        new Item {Id = Guid.NewGuid(), Name = "Iron Sword", Price = 20, CreatedDate = DateTimeOffset.UtcNow},
        new Item {Id = Guid.NewGuid(), Name = "Bronze Shield", Price = 18, CreatedDate = DateTimeOffset.UtcNow}
    };

    public async Task<IEnumerable<Item>> GetItemsAsync()
    {
        return await Task.FromResult(items); // Just return a completed task with items in it
    }

    public async Task<Item?> GetItemAsync(Guid id)
    {
        //var item = items.Where(item => item.Id == id).SingleOrDefault();
        var item = items.SingleOrDefault(item => item.Id == id); // Same as above 
        return await Task.FromResult(item); // Just return a completed task with item
    }

    public async Task CreateItemAsync(Item item)
    {
        items.Add(item);
        
        await Task.CompletedTask; // Just return a completed task
        
    }

    public async Task UpdateItemAsync(Item item)
    {
        var index = items.FindIndex(existingItem => existingItem.Id == item.Id);
        items[index] = item;
        await Task.CompletedTask; // Just return a completed task
    }

    public async Task DeleteItemAsync(Guid id)
    {
        var index = items.FindIndex(existingItem => existingItem.Id == id);
        items.RemoveAt(index);
        await Task.CompletedTask; // Just return a completed task
    }
}