using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
            DummyDataInitialization();
            RepositoryInitialization();
        }

        /// <summary>
        /// RepositoryInitialization Function
        /// ---------------------------------
        /// 1. Creates directory and file for storing data as Json File.
        /// 2. Write dummy data to products.json by JsonWrite function if file not exists.
        /// 3. Reads data to _products if file exists.
        /// </summary>
        private void RepositoryInitialization()
        {
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

        /// <summary>
        /// DummyDataInitialization Function
        /// --------------------------------
        /// 1. Creates dummy categories and tags list to initialize _products list.
        /// 2. Initialize _products repository.
        /// </summary>
        private void DummyDataInitialization()
        {
            ObservableCollection<Categories> categories = new ObservableCollection<Categories>()
            {
                new Categories("123-456-7890","Others")
            };

            ObservableCollection<Tags> tags = new ObservableCollection<Tags>()
            {
                new Tags("123-456-7890","Available")
            };

            _products = new List<Products>
            {
                new Products("123-456-7890","Phone",10000,"Good Phone with good quality and features",null,categories,tags),
            };
        }

        /// <summary>
        /// JsonWrite Function
        /// ------------------
        /// 1. Writes JsonSerializedObject to products.json file.
        /// </summary>
        public void JsonWrite()
        {
            string jsonText = JsonConvert.SerializeObject(_products, Formatting.Indented);
            File.WriteAllText(_filePath, jsonText);
        }

        /// <summary>
        /// AddItem Function
        /// ----------------
        /// 1. _products list is updated with new product recieved as parameter.
        /// 2. Writes the updated _products list to Json file by JsonWrite function.
        /// </summary>
        /// <param name="product"></param>
        public void AddItem(Products product)
        {
            _products.Add(product);
            JsonWrite();
        }

        /// <summary>
        /// RemoveItem Function
        /// -------------------
        /// 1. Removes product that recieved as parameter from _products list.
        /// 2. Writes the updated _products list to Json file by JsonWrite function.
        /// </summary>
        /// <param name="product"></param>
        public void RemoveItem(Products product)
        {
            _products.Remove(product);
            JsonWrite();
        }

        /// <summary>
        /// GetAll Function
        /// </summary>
        /// <returns>
        ///     1. returns _products list which having all Products.
        /// </returns>
        public IEnumerable<Products> GetAll()
        {
            return _products;
        }
    }
}
