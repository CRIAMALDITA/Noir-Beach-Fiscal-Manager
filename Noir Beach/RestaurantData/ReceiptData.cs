using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantData
{
    internal class ReceiptData
    {
        public CompanyData User { get; set; } = CompanyData.MainUser();
        public int OperationNum { get; set; } = 0;
        public string ReceiptCode { get; set; } = string.Empty;
        public DateTime DateOfIssue { get; set; } = DateTime.Now;
        public float SubTotal;
        public float Discount;
        public float refund;
        public float Total;
    }
}
