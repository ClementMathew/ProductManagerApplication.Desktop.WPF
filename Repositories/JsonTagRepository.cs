using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Product_Manager.Models;

namespace Product_Manager.Repositories
{
    internal class JsonTagRepository : IRepository<Tags>
    {
        private string _filePath;
        private List<Tags> _tags;

        /// <summary>
        /// JsonTagRepository Constructor
        /// -----------------------------
        /// 1. Initializes _tags list for newly installed application data.
        /// 2. Calling RepositoryInitialization function.
        /// </summary>
        public JsonTagRepository()
        {
            _tags = new List<Tags>
            {
                new Tags("123-456-7890","Available"),
            };
            RepositoryInitialization();
        }

        /// <summary>
        /// RepositoryInitialization Function
        /// ---------------------------------
        /// 1. Creates directory and file for storing data as Json File.
        /// 2. Write dummy data to tags.json by JsonWrite function if file not exists.
        /// 3. Reads data to _tags if file exists.
        /// </summary>
        private void RepositoryInitialization()
        {
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

        /// <summary>
        /// JsonWrite Function
        /// ------------------
        /// 1. Writes JsonSerializedObject to tags.json file.
        /// </summary>
        public void JsonWrite()
        {
            string jsonText = JsonConvert.SerializeObject(_tags, Formatting.Indented);
            File.WriteAllText(_filePath, jsonText);
        }

        /// <summary>
        /// AddItem Function
        /// ----------------
        /// 1. Recieves tag object as parameter and adds to _tags list.
        /// 2. Write _tags to tags.json by JsonWrite Function.
        /// </summary>
        /// <param name="tag"></param>
        public void AddItem(Tags tag)
        {
            _tags.Add(tag);
            JsonWrite();
        }

        /// <summary>
        /// RemoveItem Function
        /// -------------------
        /// 1. Recieves tag object as parameter and removes from _tags list.
        /// 2. Write _tags to tags.json by JsonWrite Function.
        /// </summary>
        /// <param name="tag"></param>
        public void RemoveItem(Tags tag)
        {
            _tags.Remove(tag);
            JsonWrite();
        }

        /// <summary>
        /// GetAll Function
        /// </summary>
        /// <returns>
        ///     1. returns _tags list which having all Tags.
        /// </returns>
        public IEnumerable<Tags> GetAll()
        {
            return _tags;
        }
    }
}
