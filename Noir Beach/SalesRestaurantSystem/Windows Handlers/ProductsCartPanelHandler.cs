using RestaurantData.TablesDataClasses;
using SalesRestaurantSystem.WindowsHandlers.Listview_Handlers;
using SalesRestaurantSystem.WindowsHandlers.ListviewHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SalesRestaurantSystem.WindowsHandlers
{
    public class ProductsCartPanelHandler : CartPanelHanlder<ProductData>
    {
        public ProductsCartPanelHandler(Window win) : base(win) { }

        public override void SetListView(ListView list)
        {
            CartListViewHandler = new ProductsCartListViewHandler(_window);
            CartListViewHandler.SetListViewer(list, null);

        }

        public override void AddElements(ProductData[] items)
        {
            var elements = GenericToCartElements(items);
            CartListViewHandler.AddItemsToList(elements);
            UpdatePrices();
            OnElementAdded?.Invoke();
        }

        public override bool RemoveElements(ProductData[] items)
        {
            var elements = GenericToCartElements(items);
            CartListViewHandler.RemoveItemsToList(elements);       
            UpdatePrices();
            OnElementRemoved?.Invoke();
            return !CartListViewHandler.Items.Any(x => items.Any(y => y.Equals(x)));
        }

        public override List<CartElementData> GenericToCartElements(ProductData[] items)
        {
            List<CartElementData> elemets = new List<CartElementData>();
            for(int i = 0; i < items.Length; i++)
            {
                ProductData item = items[i];
                elemets.Add(new CartElementData()
                {
                    Name = item.ProductName,
                    Price = item.SellPrice,
                    Amount = 1,
                    Subtotal = item.SellPrice
                });
            }
            return elemets;
        }
    }
}
