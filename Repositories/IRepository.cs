using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product_Manager.Models;

namespace Product_Manager.Repositories
{
    internal interface IRepository<T>
    {
        void AddItem(int id, string name);

        IEnumerable<T> GetAll();
    }
    internal interface IProductRepository<T>
    {
        void AddItem(int id, string name,int price,string description,string imageUrl,List<Categories> categories,List<Tags> tags);

        IEnumerable<T> GetAll();
    }
}
