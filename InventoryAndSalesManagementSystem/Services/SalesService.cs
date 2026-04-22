using System;
using System.Collections.Generic;
using System.Linq;
using InventoryAndSalesManagementSystem.Models;

namespace InventoryAndSalesManagementSystem.Services
{
    public class SalesService
    {
        private readonly FileStorageService<SaleRecord> _storage;
        private readonly InventoryService _inventoryService;
        private List<SaleRecord> _sales;

        public SalesService(InventoryService inventoryService)
        {
            _storage = new FileStorageService<SaleRecord>("sales.json");
            _sales = _storage.LoadData();
            _inventoryService = inventoryService;
        }

        public void CreateSale(SaleRecord sale)
        {
            // Validate stock availability first
            foreach (var item in sale.SaleItems)
            {
                var product = _inventoryService.GetProductById(item.ProductId);
                if (product == null)
                {
                    throw new Exception($"Product with ID {item.ProductId} not found.");
                }
                if (product.StockQuantity < item.Quantity)
                {
                    throw new Exception($"Not enough stock for {product.Name}. Requested: {item.Quantity}, Available: {product.StockQuantity}");
                }
            }

            // Reduce stock
            foreach (var item in sale.SaleItems)
            {
                _inventoryService.ReduceStock(item.ProductId, item.Quantity);
            }

            sale.SaleId = _sales.Count > 0 ? _sales.Max(s => s.SaleId) + 1 : 1;
            
            int itemIdCounter = 1;
            if (_sales.Count > 0)
            {
                var allItems = _sales.SelectMany(s => s.SaleItems).ToList();
                if (allItems.Count > 0)
                {
                    itemIdCounter = allItems.Max(i => i.SaleItemId) + 1;
                }
            }

            foreach(var item in sale.SaleItems)
            {
                item.SaleItemId = itemIdCounter++;
                item.SaleId = sale.SaleId;
            }

            sale.SaleDate = DateTime.Now;
            _sales.Add(sale);
            _storage.SaveData(_sales);
        }

        public List<SaleRecord> GetAllSales()
        {
            return _sales;
        }

        public decimal GetDailySalesTotal(DateTime date)
        {
            return _sales.Where(s => s.SaleDate.Date == date.Date).Sum(s => s.TotalAmount);
        }

        public Product GetMostSoldProduct()
        {
            var productSales = _sales.SelectMany(s => s.SaleItems)
                                     .GroupBy(i => i.ProductId)
                                     .Select(g => new { ProductId = g.Key, TotalSold = g.Sum(i => i.Quantity) })
                                     .OrderByDescending(x => x.TotalSold)
                                     .FirstOrDefault();

            if (productSales != null)
            {
                return _inventoryService.GetProductById(productSales.ProductId);
            }
            return null;
        }
    }
}
