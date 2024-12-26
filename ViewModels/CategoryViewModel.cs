using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Product_Manager.Commands;
using Product_Manager.Models;
using Product_Manager.Repositories;

namespace Product_Manager.ViewModels
{
    internal class CategoryViewModel : ViewModelBase
    {
        private IRepository<Categories> _repository;
        private IEnumerable<Categories> _categories;
        private string _name;

        #region Properties

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private int _categoryId;

        public int CategoryId
        {
            get { return _categoryId; }
            set
            {
                _categoryId = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public ObservableCollection<Categories> CategoriesToList { get; set; }

        public RelayCommand NewCategoryCommand { get; set; }
        public RelayCommand NewCategorySubmitCommand { get; set; }

        public CategoryViewModel()
        {
            _repository = new JsonCategoryRepository();
            _categories = new List<Categories>(_repository.GetAll());

            CategoriesToList = new ObservableCollection<Categories>(_categories);

            NewCategoryCommand = new RelayCommand(HandleNewCategoryCommand);
            NewCategorySubmitCommand = new RelayCommand(HandleNewCategorySubmitCommand);
        }

        private void HandleNewCategorySubmitCommand(object obj)
        {
            _repository.AddItem(CategoryId, Name);
        }

        private void HandleNewCategoryCommand(object obj)
        {
           
        }
    }
}
