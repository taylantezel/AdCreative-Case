using Integration.Common;
using Integration.Backend;
using Integration.Providers;

namespace Integration.Service;

public sealed class ItemIntegrationService
{
    //This is a dependency that is normally fulfilled externally.
    private ItemOperationBackend ItemIntegrationBackend { get; set; } = new();
    private readonly RedisDatabaseProvider _redisDatabaseProvider = new();

    // This is called externally and can be called multithreaded, in parallel.
    // More than one item with the same content should not be saved. However,
    // calling this with different contents at the same time is OK, and should
    // be allowed for performance reasons.
    public Result SaveItem(string itemContent)
    {
        var lockManager = new RedisLockManager(_redisDatabaseProvider.GetDatabase());
        var lockIdentifier = Guid.NewGuid().ToString();

        if (lockManager.AcquireLock(itemContent, lockIdentifier, TimeSpan.FromSeconds(30)))
        {
            // Check the backend to see if the content is already saved.
            if (ItemIntegrationBackend.FindItemsWithContent(itemContent).Count != 0)
            {
                return new Result(false, $"Duplicate item received with content {itemContent}.");
            }

            var item = ItemIntegrationBackend.SaveItem(itemContent);

            lockManager.ReleaseLock(itemContent, lockIdentifier);

            return new Result(true, $"Item with content {itemContent} saved with id {item.Id}");
        }

        return new Result(false, $"Item was not saved. Distributed lock time out.");
    }

    public List<Item> GetAllItems()
    {
        return ItemIntegrationBackend.GetAllItems();
    }
}