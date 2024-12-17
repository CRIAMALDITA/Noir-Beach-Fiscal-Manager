using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantData
{
    public class ReceiptData
    {
        public ReceiptTop Top;
        public ReceiptClientData Client;
        public ReceiptInvoiceData Invoice;
        public PayData Pay;
        public string Messagge;


        public struct ReceiptTop
        {
            public string CompanyName;
            public string Address;
            public string Id;
        }
        public struct ReceiptClientData
        {
            public string ClientName;
            public string ClientId;
            public string ClientAddress;
            public string ClientMail;
        }
        public struct ReceiptInvoiceData
        {
            public string InvoiceNum;
            public string POS;
            public string CreationDate;
            public string UserName;
            public List<ReceiptElements> Elements;
            public string Total;
            public string Discount;
        }
        public struct PayData
        {
            public string PayType;
            public string PaysWith;
            public string Total;
            public string Change;
        }
        public struct ReceiptElements
        {
            public string Count;
            public string Name;
            public string Price;
            public string SubTotal;
        }
    }
}
