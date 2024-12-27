using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Product_Manager.Models;

namespace Product_Manager.Repositories
{
    internal class JsonTagRepository : IRepository<Tags>
    {
        private string _filePath;
        private List<Tags> _tags;

        public JsonTagRepository()
        {
            _tags = new List<Tags>
            {
                new Tags("123-456-7890","Available"),
            };

            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string projectPath = Path.Combine(path, "DotNet", "Product Manager");

            if (!Directory.Exists(projectPath))
            {
                Directory.CreateDirectory(projectPath);
            }
            _filePath = Path.Combine(projectPath, "tags.json");

            if (!File.Exists(_filePath))
            {
                JsonWrite();
            }
            else
            {
                string jsonText = File.ReadAllText(_filePath);
                _tags = JsonConvert.DeserializeObject<List<Tags>>(jsonText);
            }
        }

        public void JsonWrite()
        {
            string jsonText = JsonConvert.SerializeObject(_tags, Formatting.Indented);
            File.WriteAllText(_filePath, jsonText);
        }

        public void AddItem(Tags tag)
        {
            _tags.Add(tag);
            JsonWrite();
        }

        public IEnumerable<Tags> GetAll()
        {
            return _tags;
        }
    }
}
