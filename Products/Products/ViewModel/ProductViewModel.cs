using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Products.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Products.ViewModel
{
    public class ProductViewModel : INotifyPropertyChanged
    {


        #region Attributes
        private List<Product> product;
        private ObservableCollection<Product> products;
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties
        public ObservableCollection<Product> Products
        {
            get
            {
                return products;
            }
            set
            {
                if (products != value)
                {
                    products = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Products)));
                }
            }
        }
        #endregion

        #region Constructor
        public ProductViewModel(List<Product> product)
        {
            this.product = product;
            Products = new ObservableCollection<Product>(product.OrderBy(p=>p.Description));            
        }

        private void LoadProduct(List<Product> product)
        {
            Products.Clear();
            foreach (var item in product)
            {
                Products.Add(new Product
                {
                    Description=item.Description,
                    Image = item.Image,
                    IsActive = item.IsActive,
                    LastPurchase = item.LastPurchase,
                    Price = item.Price,
                    ProductId = item.ProductId,
                    Remarks = item.Remarks,
                    Sctock = item.Sctock,
                    
                });
            }
        }
        #endregion
    }
}
