using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product_Manager.Models
{
    public class Products
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public List<Categories> CategoryList { get; set; }
        public List<Tags> TagsList { get; set; }

        public Products(int productId,
            string name,
            int price,
            string description,
            string imageUrl,
            List<Categories> categories,
            List<Tags> tags)
        {
            ProductID = productId;
            Name = name;
            Price = price;
            Description = description;
            ImageUrl = imageUrl;
            CategoryList = categories;
            TagsList = tags;
        }
    }
}
