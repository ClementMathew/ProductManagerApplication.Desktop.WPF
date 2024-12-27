using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using Product_Manager.Models;

namespace Product_Manager.Repositories
{
    internal class JsonProductRepository : IRepository<Products>
    {
        private string _filePath;
        private List<Products> _products;

        public JsonProductRepository()
        {
            var categories = new ObservableCollection<Categories>()
            {
                new Categories("123-456-7890","Others")
            };

            var tags = new ObservableCollection<Tags>()
            {
                new Tags("123-456-7890","Available")
            };

            _products = new List<Products>
            {
                new Products("123-456-7890","Phone",10000,"Good Phone with good quality and features",null,categories,tags),
            };

            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string projectPath = Path.Combine(path, "DotNet", "Product Manager");

            if (!Directory.Exists(projectPath))
            {
                Directory.CreateDirectory(projectPath);
            }
            _filePath = Path.Combine(projectPath, "products.json");

            if (!File.Exists(_filePath))
            {
                JsonWrite();
            }
            else
            {
                string jsonText = File.ReadAllText(_filePath);
                _products = JsonConvert.DeserializeObject<List<Products>>(jsonText);
            }
        }

        public void JsonWrite()
        {
            string jsonText = JsonConvert.SerializeObject(_products, Formatting.Indented);
            File.WriteAllText(_filePath, jsonText);
        }

        public void AddItem(Products product)
        {
            _products.Add(product);
            JsonWrite();
        }

        public IEnumerable<Products> GetAll()
        {
            return _products;
        }
    }
}
