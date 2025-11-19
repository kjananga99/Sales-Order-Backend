using Microsoft.EntityFrameworkCore;
using SalesOrderAPI.Application.DTOs;
using SalesOrderAPI.Application.Interfaces;
using SalesOrderAPI.Domain.Entities;
using SalesOrderAPI.Infrastructure.Data;

namespace SalesOrderAPI.Application.Services
{
    public class SalesOrderService : ISalesOrderService
    {
        private readonly ApplicationDbContext _context;

        public SalesOrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SalesOrderDto>> GetAllSalesOrdersAsync()
        {
            var orders = await _context.SalesOrders
                .Include(so => so.Items)
                .OrderByDescending(so => so.CreatedAt)
                .ToListAsync();

            return orders.Select(MapToDto);
        }

        public async Task<SalesOrderDto?> GetSalesOrderByIdAsync(int id)
        {
            var order = await _context.SalesOrders
                .Include(so => so.Items)
                .FirstOrDefaultAsync(so => so.Id == id);

            return order == null ? null : MapToDto(order);
        }

        public async Task<SalesOrderDto> CreateSalesOrderAsync(CreateSalesOrderDto createDto)
        {
            var salesOrder = new SalesOrder
            {
                CustomerId = createDto.CustomerId,
                CustomerName = createDto.CustomerName,
                Address1 = createDto.Address1,
                Address2 = createDto.Address2,
                Address3 = createDto.Address3,
                Suburb = createDto.Suburb,
                State = createDto.State,
                PostCode = createDto.PostCode,
                InvoiceNo = createDto.InvoiceNo,
                InvoiceDate = createDto.InvoiceDate,
                ReferenceNo = createDto.ReferenceNo,
                Items = new List<SalesOrderItem>()
            };

            // Calculate totals
            decimal totalExcl = 0;
            decimal totalTax = 0;

            foreach (var itemDto in createDto.Items)
            {
                var exclAmount = itemDto.Quantity * itemDto.Price;
                var taxAmount = exclAmount * itemDto.TaxRate / 100;
                var inclAmount = exclAmount + taxAmount;

                var orderItem = new SalesOrderItem
                {
                    ItemCode = itemDto.ItemCode,
                    Description = itemDto.Description,
                    Note = itemDto.Note,
                    Quantity = itemDto.Quantity,
                    Price = itemDto.Price,
                    TaxRate = itemDto.TaxRate,
                    ExclAmount = exclAmount,
                    TaxAmount = taxAmount,
                    InclAmount = inclAmount
                };

                salesOrder.Items.Add(orderItem);
                totalExcl += exclAmount;
                totalTax += taxAmount;
            }

            salesOrder.TotalExcl = totalExcl;
            salesOrder.TotalTax = totalTax;
            salesOrder.TotalIncl = totalExcl + totalTax;

            _context.SalesOrders.Add(salesOrder);
            await _context.SaveChangesAsync();

            return MapToDto(salesOrder);
        }

        public async Task<SalesOrderDto?> UpdateSalesOrderAsync(int id, CreateSalesOrderDto updateDto)
        {
            var existingOrder = await _context.SalesOrders
                .Include(so => so.Items)
                .FirstOrDefaultAsync(so => so.Id == id);

            if (existingOrder == null) return null;

            // Update header
            existingOrder.CustomerId = updateDto.CustomerId;
            existingOrder.CustomerName = updateDto.CustomerName;
            existingOrder.Address1 = updateDto.Address1;
            existingOrder.Address2 = updateDto.Address2;
            existingOrder.Address3 = updateDto.Address3;
            existingOrder.Suburb = updateDto.Suburb;
            existingOrder.State = updateDto.State;
            existingOrder.PostCode = updateDto.PostCode;
            existingOrder.InvoiceNo = updateDto.InvoiceNo;
            existingOrder.InvoiceDate = updateDto.InvoiceDate;
            existingOrder.ReferenceNo = updateDto.ReferenceNo;
            existingOrder.UpdatedAt = DateTime.Now;

            // Remove old items
            _context.SalesOrderItems.RemoveRange(existingOrder.Items);

            // Add new items
            existingOrder.Items = new List<SalesOrderItem>();
            decimal totalExcl = 0;
            decimal totalTax = 0;

            foreach (var itemDto in updateDto.Items)
            {
                var exclAmount = itemDto.Quantity * itemDto.Price;
                var taxAmount = exclAmount * itemDto.TaxRate / 100;
                var inclAmount = exclAmount + taxAmount;

                var orderItem = new SalesOrderItem
                {
                    ItemCode = itemDto.ItemCode,
                    Description = itemDto.Description,
                    Note = itemDto.Note,
                    Quantity = itemDto.Quantity,
                    Price = itemDto.Price,
                    TaxRate = itemDto.TaxRate,
                    ExclAmount = exclAmount,
                    TaxAmount = taxAmount,
                    InclAmount = inclAmount
                };

                existingOrder.Items.Add(orderItem);
                totalExcl += exclAmount;
                totalTax += taxAmount;
            }

            existingOrder.TotalExcl = totalExcl;
            existingOrder.TotalTax = totalTax;
            existingOrder.TotalIncl = totalExcl + totalTax;

            await _context.SaveChangesAsync();

            return MapToDto(existingOrder);
        }

        public async Task<bool> DeleteSalesOrderAsync(int id)
        {
            var order = await _context.SalesOrders.FindAsync(id);
            if (order == null) return false;

            _context.SalesOrders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        private static SalesOrderDto MapToDto(SalesOrder order)
        {
            return new SalesOrderDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                CustomerName = order.CustomerName,
                Address1 = order.Address1,
                Address2 = order.Address2,
                Address3 = order.Address3,
                Suburb = order.Suburb,
                State = order.State,
                PostCode = order.PostCode,
                InvoiceNo = order.InvoiceNo,
                InvoiceDate = order.InvoiceDate,
                ReferenceNo = order.ReferenceNo,
                TotalExcl = order.TotalExcl,
                TotalTax = order.TotalTax,
                TotalIncl = order.TotalIncl,
                Items = order.Items.Select(i => new SalesOrderItemDto
                {
                    Id = i.Id,
                    ItemCode = i.ItemCode,
                    Description = i.Description,
                    Note = i.Note,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    TaxRate = i.TaxRate,
                    ExclAmount = i.ExclAmount,
                    TaxAmount = i.TaxAmount,
                    InclAmount = i.InclAmount
                }).ToList()
            };
        }
    }
}