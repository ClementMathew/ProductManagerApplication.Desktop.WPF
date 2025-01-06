using System.Collections.ObjectModel;

namespace Product_Manager.Models
{
    public class Products
    {
        public string ProductID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public string ImageSourceBase64 { get; set; }

        /// <summary>
        /// To store the categories to which this product included.
        /// </summary>
        public ObservableCollection<Categories> CategoryList { get; set; }

        /// <summary>
        /// To store the tags to which this product included.
        /// </summary>
        public ObservableCollection<Tags> TagsList { get; set; }

        public Products(string productId,
            string name,
            int price,
            string description,
            string imageSourceBase64,
            ObservableCollection<Categories> categories,
            ObservableCollection<Tags> tags)
        {
            ProductID = productId;
            Name = name;
            Price = price;
            Description = description;
            ImageSourceBase64 = imageSourceBase64;
            CategoryList = categories;
            TagsList = tags;
        }
    }
}
