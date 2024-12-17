using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product_Manager.Models;
using Product_Manager.Repositories;

namespace Product_Manager.ViewModels
{
    internal class ProductViewModel : ViewModelBase
    {
        private IProductRepository<Products> _productRepository;
        private IEnumerable<Products> _products;

        public ObservableCollection<Products> ProductsToList { get; set; }
        public ProductViewModel()
        {
            _productRepository = new InMemoryProductRepository();
            _products = _productRepository.GetAll();

            ProductsToList  = new ObservableCollection<Products>(_products);
        }
    }
}
