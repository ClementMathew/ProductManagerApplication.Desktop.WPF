using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product_Manager.Models
{
    public class Categories
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public Categories(int categoryId,string name)
        {
            CategoryId = categoryId;
            Name = name;
        }
    }
}
