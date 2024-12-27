using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Product_Manager.Commands;
using Product_Manager.Models;
using Product_Manager.Repositories;

namespace Product_Manager.ViewModels
{
    internal class HomeViewModel : ViewModelBase
    {
        private ProductViewModel _productViewModel;
        private CategoryViewModel _categoryViewModel;
        private TagViewModel _tagViewModel;

        public RelayCommand HomeDeleteCommand{ get; set; }

        public HomeViewModel()
        {
            _productViewModel = new ProductViewModel();
            _categoryViewModel = new CategoryViewModel();
            _tagViewModel = new TagViewModel();

            HomeDeleteCommand = new RelayCommand(HandleHomeDeleteCommand, CanHandleHomeDeleteCommand);
        }

        private bool CanHandleHomeDeleteCommand(object arg)
        {
            return true;
        }

        private void HandleHomeDeleteCommand(object obj)
        {
            MessageBox.Show("");
        }
    }
}
