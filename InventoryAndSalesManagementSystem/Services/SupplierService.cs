using System;
using System.Collections.Generic;
using System.Linq;
using InventoryAndSalesManagementSystem.Models;

namespace InventoryAndSalesManagementSystem.Services
{
    public class SupplierService
    {
        private readonly FileStorageService<Supplier> _storage;
        private List<Supplier> _suppliers;

        public SupplierService()
        {
            _storage = new FileStorageService<Supplier>("suppliers.json");
            _suppliers = _storage.LoadData();
        }

        public void AddSupplier(Supplier supplier)
        {
            supplier.SupplierId = _suppliers.Count > 0 ? _suppliers.Max(s => s.SupplierId) + 1 : 1;
            _suppliers.Add(supplier);
            _storage.SaveData(_suppliers);
        }

        public List<Supplier> GetAllSuppliers()
        {
            return _suppliers;
        }

        public void UpdateSupplier(Supplier updatedSupplier)
        {
            var supplier = _suppliers.FirstOrDefault(s => s.SupplierId == updatedSupplier.SupplierId);
            if (supplier != null)
            {
                supplier.Name = updatedSupplier.Name;
                supplier.Phone = updatedSupplier.Phone;
                supplier.Email = updatedSupplier.Email;
                supplier.Address = updatedSupplier.Address;
                _storage.SaveData(_suppliers);
            }
            else
            {
                throw new Exception("Supplier not found.");
            }
        }

        public void DeleteSupplier(int supplierId)
        {
            var supplier = _suppliers.FirstOrDefault(s => s.SupplierId == supplierId);
            if (supplier != null)
            {
                _suppliers.Remove(supplier);
                _storage.SaveData(_suppliers);
            }
            else
            {
                throw new Exception("Supplier not found.");
            }
        }

        public List<Supplier> SearchSupplier(string searchTerm)
        {
            return _suppliers.Where(s => 
                s.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                s.Phone.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                (s.Email != null && s.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }
    }
}
