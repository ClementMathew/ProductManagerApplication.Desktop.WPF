using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product_Manager.Models
{
    public class Tags
    {
        public int TagId { get; set; }
        public string Name { get; set; }
        public Tags(int tagId,string name)
        {
            TagId = tagId;
            Name = name;
        }
    }
}
