using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product_Manager.Models;

namespace Product_Manager.Repositories
{
    internal class InMemoryTagRepository : IRepository<Tags>
    {
        private List<Tags> _tags = new List<Tags>
        {
            new Tags(1,"Available"),
        };

        public void AddItem(int id,string name)
        {
            _tags.Add(new Tags(id,name));
        }

        public IEnumerable<Tags> GetAll()
        {
            return _tags;
        }
    }
}
