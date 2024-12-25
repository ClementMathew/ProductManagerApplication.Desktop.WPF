using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        void AddItem(int id, string name, int price, string description, string imageUrl, ObservableCollection<Categories> categories, ObservableCollection<Tags> tags);

        IEnumerable<T> GetAll();
    }
}
