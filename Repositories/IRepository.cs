using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Product_Manager.Models;

namespace Product_Manager.Repositories
{
    internal interface IRepository<T>
    {
        void AddItem(T Titem);

        IEnumerable<T> GetAll();
    }
}
