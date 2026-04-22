using System;
using System.Collections.Generic;
using InventoryAndSalesManagementSystem.Models;
using InventoryAndSalesManagementSystem.Services;

namespace InventoryAndSalesManagementSystem
{
    class Program
    {
        static InventoryService inventoryService;
        static SupplierService supplierService;
        static CustomerService customerService;
        static SalesService salesService;

        static void Main(string[] args)
        {
            try
            {
                inventoryService = new InventoryService();
                supplierService = new SupplierService();
                customerService = new CustomerService();
                salesService = new SalesService(inventoryService);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing services: {ex.Message}");
                return;
            }

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=== Inventory and Sales Management System ===");
                Console.WriteLine("1. Product Management");
                Console.WriteLine("2. Supplier Management");
                Console.WriteLine("3. Customer Management");
                Console.WriteLine("4. Create Sale");
                Console.WriteLine("5. View Sales History");
                Console.WriteLine("6. Reports");
                Console.WriteLine("7. Exit");
                Console.Write("Enter your choice: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ProductManagementMenu();
                        break;
                    case "2":
                        SupplierManagementMenu();
                        break;
                    case "3":
                        CustomerManagementMenu();
                        break;
                    case "4":
                        CreateSale();
                        break;
                    case "5":
                        ViewSalesHistory();
                        break;
                    case "6":
                        ReportsMenu();
                        break;
                    case "7":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Press Enter to continue.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        #region Product Management
        static void ProductManagementMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("--- Product Management ---");
                Console.WriteLine("1. Add Product");
                Console.WriteLine("2. View Products");
                Console.WriteLine("3. Update Product");
                Console.WriteLine("4. Delete Product");
                Console.WriteLine("5. Search Product");
                Console.WriteLine("6. Back");
                Console.Write("Enter choice: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AddProduct(); break;
                    case "2": ViewProducts(); break;
                    case "3": UpdateProduct(); break;
                    case "4": DeleteProduct(); break;
                    case "5": SearchProduct(); break;
                    case "6": back = true; break;
                    default: Console.WriteLine("Invalid choice."); Console.ReadLine(); break;
                }
            }
        }

        static void AddProduct()
        {
            Console.Clear();
            Console.WriteLine("Add New Product");
            try
            {
                var product = new Product();
                Console.Write("Name: "); product.Name = Console.ReadLine();
                Console.Write("Code: "); product.Code = Console.ReadLine();
                Console.Write("Category: "); product.Category = Console.ReadLine();
                Console.Write("Price: "); product.Price = decimal.Parse(Console.ReadLine());
                Console.Write("Stock Quantity: "); product.StockQuantity = int.Parse(Console.ReadLine());

                if (string.IsNullOrWhiteSpace(product.Name) || string.IsNullOrWhiteSpace(product.Code) || product.Price <= 0 || product.StockQuantity < 0)
                {
                    Console.WriteLine("Invalid input. Name and Code cannot be empty. Price must be > 0. Stock cannot be negative.");
                }
                else
                {
                    inventoryService.AddProduct(product);
                    Console.WriteLine("Product added successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadLine();
        }

        static void ViewProducts()
        {
            Console.Clear();
            Console.WriteLine("All Products");
            var products = inventoryService.GetAllProducts();
            foreach (var p in products)
            {
                Console.WriteLine($"ID: {p.ProductId} | Code: {p.Code} | Name: {p.Name} | Cat: {p.Category} | Price: {p.Price} | Stock: {p.StockQuantity}");
            }
            Console.ReadLine();
        }

        static void UpdateProduct()
        {
            Console.Clear();
            Console.Write("Enter Product ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var product = inventoryService.GetProductById(id);
                if (product != null)
                {
                    try
                    {
                        var updated = new Product { ProductId = id };
                        Console.Write($"Name ({product.Name}): "); var name = Console.ReadLine(); updated.Name = string.IsNullOrWhiteSpace(name) ? product.Name : name;
                        Console.Write($"Code ({product.Code}): "); var code = Console.ReadLine(); updated.Code = string.IsNullOrWhiteSpace(code) ? product.Code : code;
                        Console.Write($"Category ({product.Category}): "); var cat = Console.ReadLine(); updated.Category = string.IsNullOrWhiteSpace(cat) ? product.Category : cat;
                        
                        Console.Write($"Price ({product.Price}): "); var priceInput = Console.ReadLine();
                        updated.Price = string.IsNullOrWhiteSpace(priceInput) ? product.Price : decimal.Parse(priceInput);
                        
                        Console.Write($"Stock Quantity ({product.StockQuantity}): "); var stockInput = Console.ReadLine();
                        updated.StockQuantity = string.IsNullOrWhiteSpace(stockInput) ? product.StockQuantity : int.Parse(stockInput);

                        inventoryService.UpdateProduct(updated);
                        Console.WriteLine("Product updated successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Product not found.");
                }
            }
            Console.ReadLine();
        }

        static void DeleteProduct()
        {
            Console.Clear();
            Console.Write("Enter Product ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    inventoryService.DeleteProduct(id);
                    Console.WriteLine("Product deleted.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            Console.ReadLine();
        }

        static void SearchProduct()
        {
            Console.Clear();
            Console.Write("Enter search term (Name, Code, Category): ");
            var term = Console.ReadLine();
            var results = inventoryService.SearchProduct(term);
            if (results.Count == 0)
            {
                Console.WriteLine("No products found.");
            }
            else
            {
                foreach (var p in results)
                {
                    Console.WriteLine($"ID: {p.ProductId} | Code: {p.Code} | Name: {p.Name} | Cat: {p.Category} | Price: {p.Price} | Stock: {p.StockQuantity}");
                }
            }
            Console.ReadLine();
        }
        #endregion

        #region Supplier Management
        static void SupplierManagementMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("--- Supplier Management ---");
                Console.WriteLine("1. Add Supplier");
                Console.WriteLine("2. View Suppliers");
                Console.WriteLine("3. Update Supplier");
                Console.WriteLine("4. Delete Supplier");
                Console.WriteLine("5. Search Supplier");
                Console.WriteLine("6. Back");
                Console.Write("Enter choice: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AddSupplier(); break;
                    case "2": ViewSuppliers(); break;
                    case "3": UpdateSupplier(); break;
                    case "4": DeleteSupplier(); break;
                    case "5": SearchSupplier(); break;
                    case "6": back = true; break;
                    default: Console.WriteLine("Invalid choice."); Console.ReadLine(); break;
                }
            }
        }

        static void AddSupplier()
        {
            Console.Clear();
            Console.WriteLine("Add New Supplier");
            var supplier = new Supplier();
            Console.Write("Name: "); supplier.Name = Console.ReadLine();
            Console.Write("Phone: "); supplier.Phone = Console.ReadLine();
            Console.Write("Email: "); supplier.Email = Console.ReadLine();
            Console.Write("Address: "); supplier.Address = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(supplier.Name))
            {
                Console.WriteLine("Name cannot be empty.");
            }
            else
            {
                supplierService.AddSupplier(supplier);
                Console.WriteLine("Supplier added.");
            }
            Console.ReadLine();
        }

        static void ViewSuppliers()
        {
            Console.Clear();
            Console.WriteLine("All Suppliers");
            foreach (var s in supplierService.GetAllSuppliers())
            {
                Console.WriteLine($"ID: {s.SupplierId} | Name: {s.Name} | Phone: {s.Phone} | Email: {s.Email}");
            }
            Console.ReadLine();
        }
        
        static void UpdateSupplier()
        {
            Console.Clear();
            Console.Write("Enter Supplier ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    var updated = new Supplier { SupplierId = id };
                    Console.Write("Name: "); updated.Name = Console.ReadLine();
                    Console.Write("Phone: "); updated.Phone = Console.ReadLine();
                    Console.Write("Email: "); updated.Email = Console.ReadLine();
                    Console.Write("Address: "); updated.Address = Console.ReadLine();
                    supplierService.UpdateSupplier(updated);
                    Console.WriteLine("Supplier updated.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            Console.ReadLine();
        }

        static void DeleteSupplier()
        {
            Console.Clear();
            Console.Write("Enter Supplier ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    supplierService.DeleteSupplier(id);
                    Console.WriteLine("Supplier deleted.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            Console.ReadLine();
        }

        static void SearchSupplier()
        {
            Console.Clear();
            Console.Write("Enter search term: ");
            var results = supplierService.SearchSupplier(Console.ReadLine());
            foreach (var s in results) Console.WriteLine($"ID: {s.SupplierId} | Name: {s.Name}");
            Console.ReadLine();
        }
        #endregion

        #region Customer Management
        static void CustomerManagementMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("--- Customer Management ---");
                Console.WriteLine("1. Add Customer");
                Console.WriteLine("2. View Customers");
                Console.WriteLine("3. Update Customer");
                Console.WriteLine("4. Delete Customer");
                Console.WriteLine("5. Search Customer");
                Console.WriteLine("6. Back");
                Console.Write("Enter choice: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AddCustomer(); break;
                    case "2": ViewCustomers(); break;
                    case "3": UpdateCustomer(); break;
                    case "4": DeleteCustomer(); break;
                    case "5": SearchCustomer(); break;
                    case "6": back = true; break;
                    default: Console.WriteLine("Invalid choice."); Console.ReadLine(); break;
                }
            }
        }
        
        static void AddCustomer()
        {
            Console.Clear();
            Console.WriteLine("Add New Customer");
            var customer = new Customer();
            Console.Write("Name: "); customer.Name = Console.ReadLine();
            Console.Write("Phone: "); customer.Phone = Console.ReadLine();
            Console.Write("Email: "); customer.Email = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(customer.Name))
                Console.WriteLine("Name cannot be empty.");
            else
            {
                customerService.AddCustomer(customer);
                Console.WriteLine("Customer added.");
            }
            Console.ReadLine();
        }

        static void ViewCustomers()
        {
            Console.Clear();
            Console.WriteLine("All Customers");
            foreach (var c in customerService.GetAllCustomers()) Console.WriteLine($"ID: {c.CustomerId} | Name: {c.Name} | Phone: {c.Phone}");
            Console.ReadLine();
        }

        static void UpdateCustomer()
        {
            Console.Clear();
            Console.Write("Enter Customer ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    var updated = new Customer { CustomerId = id };
                    Console.Write("Name: "); updated.Name = Console.ReadLine();
                    Console.Write("Phone: "); updated.Phone = Console.ReadLine();
                    Console.Write("Email: "); updated.Email = Console.ReadLine();
                    customerService.UpdateCustomer(updated);
                    Console.WriteLine("Customer updated.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            Console.ReadLine();
        }

        static void DeleteCustomer()
        {
            Console.Clear();
            Console.Write("Enter Customer ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    customerService.DeleteCustomer(id);
                    Console.WriteLine("Customer deleted.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            Console.ReadLine();
        }

        static void SearchCustomer()
        {
            Console.Clear();
            Console.Write("Enter search term: ");
            var results = customerService.SearchCustomer(Console.ReadLine());
            foreach (var c in results) Console.WriteLine($"ID: {c.CustomerId} | Name: {c.Name}");
            Console.ReadLine();
        }
        #endregion

        #region Sales Management
        static void CreateSale()
        {
            Console.Clear();
            Console.WriteLine("--- Create Sale ---");
            
            var sale = new SaleRecord();
            Console.Write("Enter Customer ID (or 0 for guest): ");
            if (int.TryParse(Console.ReadLine(), out int custId))
            {
                sale.CustomerId = custId;
            }

            Console.Write("Enter Payment Type (Cash/Card): ");
            sale.PaymentType = Console.ReadLine();

            bool addingItems = true;
            while (addingItems)
            {
                Console.Write("Enter Product ID to add: ");
                if (int.TryParse(Console.ReadLine(), out int pId))
                {
                    var product = inventoryService.GetProductById(pId);
                    if (product != null)
                    {
                        Console.Write($"Enter Quantity for {product.Name} (Available: {product.StockQuantity}): ");
                        if (int.TryParse(Console.ReadLine(), out int qty) && qty > 0)
                        {
                            var item = new SaleItem
                            {
                                ProductId = product.ProductId,
                                Quantity = qty,
                                UnitPrice = product.Price
                            };
                            sale.SaleItems.Add(item);
                            Console.WriteLine("Item added.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid quantity.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Product not found.");
                    }
                }

                Console.Write("Add another item? (y/n): ");
                if (Console.ReadLine()?.ToLower() != "y")
                {
                    addingItems = false;
                }
            }

            if (sale.SaleItems.Count > 0)
            {
                decimal total = 0;
                foreach(var item in sale.SaleItems) total += item.Quantity * item.UnitPrice;
                sale.TotalAmount = total;
                Console.WriteLine($"Total Amount: {sale.TotalAmount}");
                
                try
                {
                    salesService.CreateSale(sale);
                    Console.WriteLine("Sale completed successfully! Stock updated.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Sale failed: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("No items in sale. Cancelled.");
            }

            Console.ReadLine();
        }

        static void ViewSalesHistory()
        {
            Console.Clear();
            Console.WriteLine("--- Sales History ---");
            foreach (var s in salesService.GetAllSales())
            {
                Console.WriteLine($"Sale ID: {s.SaleId} | Date: {s.SaleDate} | Total: {s.TotalAmount} | Payment: {s.PaymentType}");
                foreach(var item in s.SaleItems)
                {
                    var p = inventoryService.GetProductById(item.ProductId);
                    Console.WriteLine($"  - Product: {p?.Name ?? "Unknown"} | Qty: {item.Quantity} | Unit Price: {item.UnitPrice}");
                }
            }
            Console.ReadLine();
        }
        #endregion

        #region Reports
        static void ReportsMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("--- Reports ---");
                Console.WriteLine("1. Total Products & Available Stock");
                Console.WriteLine("2. Low-Stock Products");
                Console.WriteLine("3. Daily Sales Report");
                Console.WriteLine("4. Most Sold Product");
                Console.WriteLine("5. Total Customers & Suppliers");
                Console.WriteLine("6. Back");
                Console.Write("Enter choice: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        var products = inventoryService.GetAllProducts();
                        Console.WriteLine($"Total Products Types: {products.Count}");
                        int totalStock = 0;
                        foreach (var p in products) totalStock += p.StockQuantity;
                        Console.WriteLine($"Total Available Stock (all items): {totalStock}");
                        Console.ReadLine();
                        break;
                    case "2":
                        var lowStock = inventoryService.GetLowStockProducts(10); // threshold 10
                        Console.WriteLine("Low Stock Products:");
                        foreach (var p in lowStock) Console.WriteLine($"- {p.Name} (Stock: {p.StockQuantity})");
                        Console.ReadLine();
                        break;
                    case "3":
                        Console.Write("Enter date (yyyy-mm-dd) or leave blank for today: ");
                        var dateInput = Console.ReadLine();
                        DateTime d = DateTime.Now;
                        if (!string.IsNullOrWhiteSpace(dateInput)) DateTime.TryParse(dateInput, out d);
                        var total = salesService.GetDailySalesTotal(d);
                        Console.WriteLine($"Total Sales for {d.ToShortDateString()}: {total}");
                        Console.ReadLine();
                        break;
                    case "4":
                        var mostSold = salesService.GetMostSoldProduct();
                        if (mostSold != null) Console.WriteLine($"Most Sold Product: {mostSold.Name} (Code: {mostSold.Code})");
                        else Console.WriteLine("No sales data available.");
                        Console.ReadLine();
                        break;
                    case "5":
                        Console.WriteLine($"Total Customers: {customerService.GetAllCustomers().Count}");
                        Console.WriteLine($"Total Suppliers: {supplierService.GetAllSuppliers().Count}");
                        Console.ReadLine();
                        break;
                    case "6":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        Console.ReadLine();
                        break;
                }
            }
        }
        #endregion
    }
}
