using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantData
{
    internal class CompanyData
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string CUIT { get; set; } = string.Empty;
        public string GrossReceipts { get; set; } = string.Empty;
        public string FiscalName { get; set; } = string.Empty;
        public string FiscalAddress {  get; set; } = string.Empty;
        public int POS { get; set; } = 0;
        public DateTime ActivitiesStart { get; set; } = DateTime.Now;
        public TaxCategory Category = TaxCategory.None;

        public static CompanyData MainUser()
        {
            return new();
        }
    }

    public enum TaxCategory
    {
        IVAResponsableIncripto, IVASujetoExento, ConsumidorFinal, ResponsableMonotributo, SujetoNoCategorizado, ProveedorDelExterior,
        ClienteDelExterior, IVALiberadoLey19640, MonotributistaSocial, IVANoAlcanzado, MonotributistaTrabajadorIndependientePromovido, None
    }
}

