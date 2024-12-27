using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Product_Manager.Commands;
using Product_Manager.Models;
using Product_Manager.Repositories;

namespace Product_Manager.ViewModels
{
    internal class CategoryViewModel : ViewModelBase, IDataErrorInfo
    {
        private IRepository<Categories> _categoryRepository;
        private List<ValidationResult> _results;

        public ObservableCollection<Categories> CategoriesToList { get; set; }

        public RelayCommand CategoryManagementSubmitCommand { get; set; }
        public RelayCommand CategoryIdGenerateCommand { get; set; }

        #region Properties

        private string _name;

        [Required(ErrorMessage = "Name can't be empty.")]
        [MinLength(3, ErrorMessage = "Minimum length not satisfied.")]
        [MaxLength(20, ErrorMessage = "Maximum length reached.")]

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
                CategoryManagementSubmitCommand?.OnCanExecuteChanged();
            }
        }

        private string _categoryId;

        [Required(ErrorMessage = "Must generate a unique id.")]

        public string CategoryId
        {
            get { return _categoryId; }
            set
            {
                _categoryId = value;
                OnPropertyChanged();
                CategoryManagementSubmitCommand?.OnCanExecuteChanged();
            }
        }

        #endregion

        public CategoryViewModel()
        {
            _categoryRepository = new JsonCategoryRepository();

            CategoriesToList = new ObservableCollection<Categories>(_categoryRepository.GetAll());

            CategoryManagementSubmitCommand = new RelayCommand(HandleCategoryManagementSubmitCommand, CanHandleCategoryManagementSubmitCommand);
            CategoryIdGenerateCommand = new RelayCommand(HandleCategoryIdGenerateCommand, CanHandleCategoryIdGenerateCommand);
        }

        private bool CanHandleCategoryIdGenerateCommand(object arg)
        {
            return CategoryId == null || CategoryId == string.Empty;
        }

        private void HandleCategoryIdGenerateCommand(object obj)
        {
            CategoryId = Guid.NewGuid().ToString();
        }

        private bool CanHandleCategoryManagementSubmitCommand(object arg)
        {
            return !_results.Any();
        }

        private void HandleCategoryManagementSubmitCommand(object obj)
        {
            var category = new Categories(CategoryId,Name);

            _categoryRepository.AddItem(category);
            CategoriesToList.Add(category);
        }

        public string Error { get; }

        public string this[string columnName]
        {
            get
            {
                var context = new ValidationContext(this) { MemberName = columnName };
                _results = new List<ValidationResult>();

                var value = GetType().GetProperty(columnName).GetValue(this);

                var isValid = Validator.TryValidateProperty(value, context, _results);
                return isValid ? null : _results.First().ErrorMessage;
            }
        }
    }
}
