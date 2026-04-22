using System;
using System.Collections.Generic;
using System.Linq;
using InventoryAndSalesManagementSystem.Models;

namespace InventoryAndSalesManagementSystem.Services
{
    public class InventoryService
    {
        private readonly FileStorageService<Product> _storage;
        private List<Product> _products;

        public InventoryService()
        {
            _storage = new FileStorageService<Product>("products.json");
            _products = _storage.LoadData();
        }

        public void AddProduct(Product product)
        {
            if (_products.Any(p => p.Code.Equals(product.Code, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception("Product code already exists.");
            }
            product.ProductId = _products.Count > 0 ? _products.Max(p => p.ProductId) + 1 : 1;
            _products.Add(product);
            _storage.SaveData(_products);
        }

        public List<Product> GetAllProducts()
        {
            return _products;
        }

        public void UpdateProduct(Product updatedProduct)
        {
            var product = _products.FirstOrDefault(p => p.ProductId == updatedProduct.ProductId);
            if (product != null)
            {
                if (product.Code != updatedProduct.Code && _products.Any(p => p.Code.Equals(updatedProduct.Code, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new Exception("New product code already exists.");
                }

                product.Name = updatedProduct.Name;
                product.Code = updatedProduct.Code;
                product.Category = updatedProduct.Category;
                product.Price = updatedProduct.Price;
                product.StockQuantity = updatedProduct.StockQuantity;
                _storage.SaveData(_products);
            }
            else
            {
                throw new Exception("Product not found.");
            }
        }

        public void DeleteProduct(int productId)
        {
            var product = _products.FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                _products.Remove(product);
                _storage.SaveData(_products);
            }
            else
            {
                throw new Exception("Product not found.");
            }
        }

        public List<Product> SearchProduct(string searchTerm)
        {
            return _products.Where(p => 
                p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                p.Code.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                p.Category.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }

        public Product GetProductById(int productId)
        {
            return _products.FirstOrDefault(p => p.ProductId == productId);
        }

        public void ReduceStock(int productId, int quantity)
        {
            var product = GetProductById(productId);
            if (product != null)
            {
                if (product.StockQuantity < quantity)
                {
                    throw new Exception($"Not enough stock for product {product.Name}. Available: {product.StockQuantity}");
                }
                product.StockQuantity -= quantity;
                _storage.SaveData(_products);
            }
            else
            {
                throw new Exception("Product not found.");
            }
        }

        public List<Product> GetLowStockProducts(int threshold = 10)
        {
            return _products.Where(p => p.StockQuantity < threshold).ToList();
        }
    }
}
