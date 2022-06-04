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

        // lesson 15A: get selected item
        private ProductModel _selectedProduct;

        public ProductModel SelectedProduct
        {
            get { return _selectedProduct; }
            set {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();       // Update it for Lesson15C

        public BindingList<CartItemModel> Cart
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
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        // Testing

        // Lesson 15D: Add the logic to sum all the costs
        public string SubTotal
        {
            get {
                decimal subTotal = 0;
                foreach (var item in Cart)
                {
                    subTotal += item.Product.RetailPrice * item.QuantityInCart;
                }
                return subTotal.ToString(); 
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

        // Lesson 15: Can Add To Cart logic
        public bool CanAddToCart { get {
                //Logic: Making sure Something is selected
                //      Making sure there is an item quantity
                return (ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity);             
            } 
        }

        public void AddToCart () {
            // Lesson 15C: Handle logic to get the selectedItem into this Cart           
            CartItemModel item = new CartItemModel
            {
                Product = SelectedProduct,
                QuantityInCart = ItemQuantity
            };
            // 15D: Logic of checking before adding to the cart

            Cart.Add(item);
            //Lesson 15D
            NotifyOfPropertyChange(() => SubTotal);
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
            //Lesson 15D
            NotifyOfPropertyChange(() => SubTotal);
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
