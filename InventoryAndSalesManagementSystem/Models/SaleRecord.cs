using System;
using System.Collections.Generic;

namespace InventoryAndSalesManagementSystem.Models
{
    public class SaleRecord
    {
        public int SaleId { get; set; }
        public int CustomerId { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentType { get; set; }
        public List<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
    }
}
