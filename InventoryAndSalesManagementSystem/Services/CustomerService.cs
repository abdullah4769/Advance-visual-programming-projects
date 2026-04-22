using System;
using System.Collections.Generic;
using System.Linq;
using InventoryAndSalesManagementSystem.Models;

namespace InventoryAndSalesManagementSystem.Services
{
    public class CustomerService
    {
        private readonly FileStorageService<Customer> _storage;
        private List<Customer> _customers;

        public CustomerService()
        {
            _storage = new FileStorageService<Customer>("customers.json");
            _customers = _storage.LoadData();
        }

        public void AddCustomer(Customer customer)
        {
            customer.CustomerId = _customers.Count > 0 ? _customers.Max(c => c.CustomerId) + 1 : 1;
            _customers.Add(customer);
            _storage.SaveData(_customers);
        }

        public List<Customer> GetAllCustomers()
        {
            return _customers;
        }

        public void UpdateCustomer(Customer updatedCustomer)
        {
            var customer = _customers.FirstOrDefault(c => c.CustomerId == updatedCustomer.CustomerId);
            if (customer != null)
            {
                customer.Name = updatedCustomer.Name;
                customer.Phone = updatedCustomer.Phone;
                customer.Email = updatedCustomer.Email;
                _storage.SaveData(_customers);
            }
            else
            {
                throw new Exception("Customer not found.");
            }
        }

        public void DeleteCustomer(int customerId)
        {
            var customer = _customers.FirstOrDefault(c => c.CustomerId == customerId);
            if (customer != null)
            {
                _customers.Remove(customer);
                _storage.SaveData(_customers);
            }
            else
            {
                throw new Exception("Customer not found.");
            }
        }

        public List<Customer> SearchCustomer(string searchTerm)
        {
            return _customers.Where(c => 
                c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                c.Phone.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                (c.Email != null && c.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }
    }
}
