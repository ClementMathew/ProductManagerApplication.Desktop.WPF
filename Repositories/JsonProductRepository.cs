using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product_Manager.Models;

namespace Product_Manager.Repositories
{
    internal class JsonProductRepository : IProductRepository<Products>
    {
        private ObservableCollection<Categories> _categories;
        private ObservableCollection<Tags> _tags;
        private List<Products> _products;

        private IRepository<Tags> _tagRepository;
        private IRepository<Categories> _categoryRepository;

        public JsonProductRepository()
        {
            _categoryRepository = new JsonCategoryRepository();
            _tagRepository = new JsonTagRepository();

            _tags = new ObservableCollection<Tags>(_tagRepository.GetAll());
            _categories = new ObservableCollection<Categories>(_categoryRepository.GetAll());

            _products = new List<Products>
            {
                new Products(1,"Phone",10000,"Good Phone","pack://application:,,,/Assets (Solution)/Images/Product.jpg",_categories,_tags),
                new Products(2,"Laptop",50000,"Good Laptop","",_categories,_tags),
                new Products(3,"Laptop",50000,"Good Laptop","",_categories,_tags),
                new Products(3,"Laptop",50000,"Good Laptop","",_categories,_tags),
                new Products(3,"Laptop",50000,"Good Laptop","",_categories,_tags),
                new Products(4,"Table",500,"Good Table","",_categories,_tags),
                new Products(5,"Bike",55000,"Good Bike","",_categories,_tags),
            };
        }

        public void AddItem(int id, string name, int price, string description, string imageUrl, ObservableCollection<Categories> categories, ObservableCollection<Tags> tags)
        {
            _products.Add(new Products(id, name, price, description, imageUrl, categories, tags));
        }

        public IEnumerable<Products> GetAll()
        {
            return _products;
        }
    }
}
