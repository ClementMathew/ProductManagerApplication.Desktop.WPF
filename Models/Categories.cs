namespace Product_Manager.Models
{
    public class Categories
    {
        public string CategoryId { get; set; }
        public string Name { get; set; }

        public Categories(string categoryId, string name)
        {
            CategoryId = categoryId;
            Name = name;
        }
    }
}
