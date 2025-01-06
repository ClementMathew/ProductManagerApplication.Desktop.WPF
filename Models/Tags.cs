namespace Product_Manager.Models
{
    public class Tags
    {
        public string TagId { get; set; }
        public string Name { get; set; }

        public Tags(string tagId, string name)
        {
            TagId = tagId;
            Name = name;
        }
    }
}
