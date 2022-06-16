using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Helpers;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
    public class SalesViewModel: Screen
    {
        // Lesson 14B
        IProductEndPoint _productEndpoint;
        IConfigHelper _configHelper;
        ISaleEndPoint _saleEndpoint;
        public SalesViewModel(IProductEndPoint productEndPoint, IConfigHelper configHelper, ISaleEndPoint saleEndPoint)
        {
            _productEndpoint = productEndPoint;
            _configHelper = configHelper;
            _saleEndpoint = saleEndPoint;
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
                NotifyOfPropertyChange(() => CanCheckOut);
            }
        }


        private int _itemQuantity = 1;

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
                return CalculateSubTotal().ToString("C"); 
            }
        }
        private decimal CalculateSubTotal()
        {
             decimal subTotal = 0;
                foreach (var item in Cart)
                {
                    subTotal += item.Product.RetailPrice * item.QuantityInCart;
                }
            return subTotal;
        }
        private decimal CalculateTax()
        {              
                var taxRate = (decimal)_configHelper.GetTaxRate();
                decimal taxAmount = Cart.Where(x => x.Product.IsTaxable)
                                        .Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate);
               
                return Decimal.Round(taxAmount/100);
        }
        // Lesson 16A: Added the logic of Tax
        public string Tax
        {
            get
            {             
                return CalculateTax().ToString("C");              
            }
        }

        public string Total
        {
            get
            {               
                return (CalculateTax() + CalculateTax()).ToString("C");
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
            //Lesson 15D
            var existingITem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
            if (existingITem != null)
            {
                existingITem.QuantityInCart += ItemQuantity;
                // hack- trick to update the displayText which should not be proper way to handle 
                // There should be better way to do it 
                Cart.Remove(existingITem);
                Cart.Add(existingITem);
            }
            else
            {
                // Lesson 15C: Handle logic to get the selectedItem into this Cart           
                CartItemModel item = new CartItemModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };
                // 15D: Logic of checking before adding to the cart

                Cart.Add(item);
            }
            // set 
            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            //end Lesson 15D
            NotifyOfPropertyChange(() => SubTotal);  
            NotifyOfPropertyChange(() => Total);   
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => CanCheckOut);
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
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        public bool CanCheckOut
        {
            get
            {
                return Cart.Count > 0 ? true : false;
            }
        }

        public async Task CheckOut()
        {
            // Create saleModel and post to API
            var sale = new SaleModel();
            foreach (var item in Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart
                });                            
            }
            // Now we have to post it to API
            await _saleEndpoint.PostSale(sale);
        }


    }
}
