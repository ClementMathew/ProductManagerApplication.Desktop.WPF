using System.Collections.Generic;

namespace Product_Manager.Repositories
{
    internal interface IRepository<T>
    {
        /// <summary>
        /// AddItem Method
        /// --------------
        /// 1. Common method to add item to the repository.
        /// 2. Takes a object of type T as parameter.
        /// </summary>
        /// <param name="Titem"></param>
        void AddItem(T Titem);

        /// <summary>
        /// RemoveItem Method
        /// -----------------
        /// 1. Common Method to remove item from the repository.
        /// 2. Takes a object of type T as a parameter.
        /// </summary>
        /// <param name="Titem"></param>
        void RemoveItem(T Titem);

        /// <summary>
        /// GetAll Method
        /// -------------
        /// 1. Common method to get all items from the repository.
        /// </summary>
        /// <returns>
        ///     1. returns an IEnumerable of Type T.
        /// </returns>
        IEnumerable<T> GetAll();
    }
}
