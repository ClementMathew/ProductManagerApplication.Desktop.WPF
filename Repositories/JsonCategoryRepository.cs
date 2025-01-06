using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Product_Manager.Models;

namespace Product_Manager.Repositories
{
    internal class JsonCategoryRepository : IRepository<Categories>
    {
        private string _filePath;
        private List<Categories> _categories;

        /// <summary>
        /// JsonCategoryRepository Constructor
        /// ----------------------------------
        /// 1. Initializes _categories list for newly installed application data.
        /// 2. Calling RepositoryInitialization function.
        /// </summary>
        public JsonCategoryRepository()
        {
            _categories = new List<Categories>
            {
                new Categories("123-456-7890","Others"),
            };
            RepositoryInitialization();
        }

        /// <summary>
        /// RepositoryInitialization Function
        /// ---------------------------------
        /// 1. Creates directory and file for storing data as Json File.
        /// 2. Write dummy data to categories.json by JsonWrite function if file not exists.
        /// 3. Reads data to _categories if file exists.
        /// </summary>
        private void RepositoryInitialization()
        {
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

        /// <summary>
        /// JsonWrite Function
        /// ------------------
        /// 1. Writes JsonSerializedObject to categories.json file.
        /// </summary>
        public void JsonWrite()
        {
            string jsonText = JsonConvert.SerializeObject(_categories, Formatting.Indented);
            File.WriteAllText(_filePath, jsonText);
        }

        /// <summary>
        /// AddItem Function
        /// ----------------
        /// 1. Recieves category object as parameter and adds to _categories list.
        /// 2. Write _categories to categories.json by JsonWrite Function.
        /// </summary>
        /// <param name="category"></param>
        public void AddItem(Categories category)
        {
            _categories.Add(category);
            JsonWrite();
        }

        /// <summary>
        /// RemoveItem Function
        /// -------------------
        /// 1. Recieves category object as parameter and removes from _categories list.
        /// 2. Write _categories to categories.json by JsonWrite Function.
        /// </summary>
        /// <param name="category"></param>
        public void RemoveItem(Categories category)
        {
            _categories.Remove(category);
            JsonWrite();
        }

        /// <summary>
        /// GetAll Function
        /// </summary>
        /// <returns>
        ///     1. returns _categories list which having all Categories.
        /// </returns>
        public IEnumerable<Categories> GetAll()
        {
            return _categories;
        }
    }
}
