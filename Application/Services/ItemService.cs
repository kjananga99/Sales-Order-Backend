using SalesOrderAPI.Application.DTOs;
using SalesOrderAPI.Application.Interfaces;
using SalesOrderAPI.Domain.Entities;

namespace SalesOrderAPI.Application.Services
{
    public class ItemService : IItemService
    {
        private readonly IRepository<Item> _itemRepository;

        public ItemService(IRepository<Item> itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<IEnumerable<ItemDto>> GetAllItemsAsync()
        {
            var items = await _itemRepository.GetAllAsync();
            return items.Select(i => new ItemDto
            {
                Id = i.Id,
                Code = i.Code,
                Description = i.Description,
                Price = i.Price
            });
        }

        public async Task<ItemDto?> GetItemByIdAsync(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null) return null;

            return new ItemDto
            {
                Id = item.Id,
                Code = item.Code,
                Description = item.Description,
                Price = item.Price
            };
        }
    }
}