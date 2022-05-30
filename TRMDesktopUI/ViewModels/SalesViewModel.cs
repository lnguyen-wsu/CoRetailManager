using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
    public class SalesViewModel: Screen
    {
        // Lesson 14B
        IProductEndPoint _productEndpoint;
        public SalesViewModel(IProductEndPoint productEndPoint)
        {
            _productEndpoint = productEndPoint;         
        }
        private async Task LoadProducts()
        {
            var productList = await _productEndpoint.GetAll();
            Products = new BindingList<ProductModel>(productList);
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }

        // end session here
        private BindingList<ProductModel> _products;

        public BindingList<ProductModel> Products
        {
            get { return _products; }
            set { _products = value; 
                NotifyOfPropertyChange(() => Products);
            }
        }


        private BindingList<ProductModel> _cart;

        public BindingList<ProductModel> Cart
        {
            get { return _cart; }
            set { _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }


        private int _itemQuantity;

        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set { _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
            }
        }

        // Testing


        public string SubTotal
        {
            get {
                // ToDo: Replace with Calculation
                return "$0.00"; 
            }
        }

        public string Tax
        {
            get
            {
                // ToDo: Replace with Calculation
                return "$0.00";
            }
        }

        public string Total
        {
            get
            {
                // ToDo: Replace with Calculation
                return "$0.00";
            }
        }


        public bool CanAddToCart { get {
                //Todo: Making sure Something is selected
                //      Making sure there is an item quantity
                return false;     
            } 
        }

        public void AddToCart () { 
        }

        public bool CanRemoveFromCart
        {
            get
            {
                //Todo: Making sure Something is selected
               
                return false;
            }
        }

        public void RemoveFromCart()
        {
        }

        public bool CanCheckOut
        {
            get
            {
                //Todo: Making sure there is something in the cart 

                return false;
            }
        }

        public void CheckOut()
        {
        }


    }
}
