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
    internal class InMemoryCategoryRepository : IRepository<Categories>
    {
        private List<Categories> _categories;
        private string _filePath;

        public InMemoryCategoryRepository()
        {
            _categories = new List<Categories>
            {
                new Categories(1,"Electronics"),
                new Categories(2,"Automobiles"),
                new Categories(3,"Others"),
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
                JsonWrite(_filePath);
            }
            else
            {
                string jsonText = File.ReadAllText(_filePath);
                _categories = JsonConvert.DeserializeObject<List<Categories>>(jsonText);
            }
        }

        public void JsonWrite(string filePath)
        {
            string jsonText = JsonConvert.SerializeObject(_categories, Formatting.Indented);
            File.WriteAllText(filePath, jsonText);
        }

        public void AddItem(int id, string name)
        {
            _categories.Add(new Categories(id, name));
            JsonWrite(_filePath);
        }

        public IEnumerable<Categories> GetAll()
        {
            return _categories;
        }
    }
}
