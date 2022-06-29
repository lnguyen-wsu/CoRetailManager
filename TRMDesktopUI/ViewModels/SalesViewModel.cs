using AutoMapper;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Helpers;
using TRMDesktopUI.Library.Models;
using TRMDesktopUI.Models;

namespace TRMDesktopUI.ViewModels
{
    public class SalesViewModel: Screen
    {
        // Lesson 14B
        IProductEndPoint _productEndpoint;
        IConfigHelper _configHelper;
        ISaleEndPoint _saleEndpoint;
        IMapper _mapper;
        private readonly StatusInfoViewModel _status;
        private readonly IWindowManager _window;

        public SalesViewModel(IProductEndPoint productEndPoint, IConfigHelper configHelper, ISaleEndPoint saleEndPoint , 
            IMapper mapper , StatusInfoViewModel status , IWindowManager window)
        {
            _productEndpoint = productEndPoint;
            _configHelper = configHelper;
            _saleEndpoint = saleEndPoint;
            _mapper = mapper;
            _status = status;
            _window = window;
        }
        private async Task LoadProducts()
        {
            var productList = await _productEndpoint.GetAll();
            var products = _mapper.Map<List<ProductDisplayModel>>(productList);
            Products = new BindingList<ProductDisplayModel>(products);
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            try
            {
                await LoadProducts();
            }
            catch (Exception ex)
            {
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "System Error";

                if (ex.Message == "Unauthorized")
                {
                    _status.UpdateMessage("Unauthorized Access", "You do not have permission to interact with Sales form .");
                    _window.ShowDialog(_status, null, settings);
                }
                else
                {
                    _status.UpdateMessage("Fatal Exception", ex.Message);
                    _window.ShowDialog(_status, null, settings);
                }
                TryClose();
            }
        }

        // end session here
        private BindingList<ProductDisplayModel> _products;

        public BindingList<ProductDisplayModel> Products
        {
            get { return _products; }
            set { _products = value; 
                NotifyOfPropertyChange(() => Products);
            }
        }

        // lesson 15A: get selected item
        private ProductDisplayModel _selectedProduct;

        public ProductDisplayModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);              
            }
        }


        private async Task ResetSalesViewModel()
        {
            Cart = new BindingList<CartItemDisplayModel>();
            //TODO- Add clearing the selectedCartItem if it does not do itself
            await LoadProducts();  // After post Database data will be updated quantity, this loadProduct should get the most updated
            
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        private CartItemDisplayModel _selectedCartItem;

        public CartItemDisplayModel SelectedCartItem
        {
            get { return _selectedCartItem; }
            set {
                _selectedCartItem = value;
                NotifyOfPropertyChange(() => SelectedCartItem);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
            }
        }


        private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();       // Update it for Lesson15C

        public BindingList<CartItemDisplayModel> Cart
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
                
            }
            else
            {
                // Lesson 15C: Handle logic to get the selectedItem into this Cart           
                CartItemDisplayModel item = new CartItemDisplayModel
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

                return SelectedCartItem != null && SelectedCartItem?.QuantityInCart > 0;
            }
        }

        public void RemoveFromCart()
        {
            SelectedCartItem.Product.QuantityInStock += 1;
            if (SelectedCartItem.QuantityInCart > 1)
            {
                SelectedCartItem.QuantityInCart -= 1;              
            }
            else
            {
                Cart.Remove(SelectedCartItem);
            }
            
            //Lesson 15D
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => CanCheckOut);
            NotifyOfPropertyChange(() => CanAddToCart);
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

            await ResetSalesViewModel();
        }


    }
}
