using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product_Manager.Models;

namespace Product_Manager.Repositories
{
    internal class InMemoryProductRepository : IProductRepository<Products>
    {
        private List<Categories> _categories;
        private List<Tags> _tags;
        private List<Products> _products;

        private IRepository<Tags> _tagRepository;
        private IRepository<Categories> _categoryRepository;

        public InMemoryProductRepository()
        {
            _categoryRepository = new InMemoryCategoryRepository();
            _tagRepository = new InMemoryTagRepository();

            _tags = new List<Tags>(_tagRepository.GetAll());
            _categories = new List<Categories>(_categoryRepository.GetAll());

            _products = new List<Products>
            {
                new Products(1,"Phone",10000,"Good Phone","",_categories,_tags),
                new Products(2,"Laptop",50000,"Good Laptop","",_categories,_tags),
                new Products(3,"Table",500,"Good Table","",_categories,_tags),
                new Products(4,"Bike",55000,"Good Bike","",_categories,_tags),

            };
        }

        public void AddItem(int id, string name, int price, string description, string imageUrl, List<Categories> categories, List<Tags> tags)
        {
            _products.Add(new Products(id, name, price, description, imageUrl, categories, tags));
        }

        public IEnumerable<Products> GetAll()
        {
            return _products;
        }
    }
}
