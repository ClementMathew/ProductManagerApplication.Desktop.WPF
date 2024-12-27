using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Product_Manager.Models;

namespace Product_Manager.Repositories
{
    internal class JsonCategoryRepository : IRepository<Categories>
    {
        private string _filePath;
        private List<Categories> _categories;

        public JsonCategoryRepository()
        {
            _categories = new List<Categories>
            {
                new Categories("123-456-7890","Others"),
            };

            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string projectPath = Path.Combine(path, "DotNet", "Product Manager");

            if (!Directory.Exists(projectPath))
            {
                Directory.CreateDirectory(projectPath);
            }
            _filePath = Path.Combine(projectPath, "categories.json");

            if (!File.Exists(_filePath))
            {
                JsonWrite();
            }
            else
            {
                string jsonText = File.ReadAllText(_filePath);
                _categories = JsonConvert.DeserializeObject<List<Categories>>(jsonText);
            }
        }

        public void JsonWrite()
        {
            string jsonText = JsonConvert.SerializeObject(_categories, Formatting.Indented);
            File.WriteAllText(_filePath, jsonText);
        }

        public void AddItem(Categories category)
        {
            _categories.Add(category);
            JsonWrite();
        }

        public IEnumerable<Categories> GetAll()
        {
            return _categories;
        }
    }
}
