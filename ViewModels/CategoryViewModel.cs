using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using Product_Manager.Commands;
using Product_Manager.Models;
using Product_Manager.Repositories;

namespace Product_Manager.ViewModels
{
    internal class CategoryViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly IRepository<Categories> _categoryRepository;
        private List<ValidationResult> _results;

        public ObservableCollection<Categories> CategoriesToList { get; set; }

        public RelayCommand CategoryManagementSubmitCommand { get; set; }
        public RelayCommand CategoryIdGenerateCommand { get; set; }
        public RelayCommand DeleteCategoryCommand { get; set; }

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

        private Categories _selectedCategoryItem;

        public Categories SelectedCategoryItem
        {
            get { return _selectedCategoryItem; }
            set
            {
                _selectedCategoryItem = value;
                OnPropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// CategoryViewModel Constructor
        /// -----------------------------
        /// 1. Initialize CategoriesToList by all categories from GetAll function of CategoryRepository.
        /// </summary>
        public CategoryViewModel()
        {
            _categoryRepository = new JsonCategoryRepository();

            CategoriesToList = new ObservableCollection<Categories>(_categoryRepository.GetAll());

            CategoryManagementSubmitCommand = new RelayCommand(HandleCategoryManagementSubmitCommand, CanHandleCategoryManagementSubmitCommand);
            CategoryIdGenerateCommand = new RelayCommand(HandleCategoryIdGenerateCommand, CanHandleCategoryIdGenerateCommand);
            DeleteCategoryCommand = new RelayCommand(HandleDeleteCategoryCommand);
        }

        /// <summary>
        /// HandleDeleteCategoryCommand Function
        /// ------------------------------------
        /// 1. Search for the categoryid in the CategoriesToList by command parameter 'obj' and sets SelectedCategoryItem by category object.
        /// 2. Asking confirmation to delete the SelectedCategoryItem.
        /// 3. Removes SelectedCategoryItem from Json Repository and CategoriesToList.
        /// </summary>
        /// <param name="obj"></param>
        private void HandleDeleteCategoryCommand(object obj)
        {
            foreach (Categories category in CategoriesToList)
            {
                if (category.CategoryId == (string)obj)
                {
                    SelectedCategoryItem = category;
                }
            }
            MessageBoxResult result = MessageBox.Show("Delete Category", "Delete Category", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _categoryRepository.RemoveItem(SelectedCategoryItem);
                CategoriesToList.Remove(SelectedCategoryItem);
            }
        }

        /// <summary>
        /// CanHandleCategoryIdGenerateCommand Function
        /// -------------------------------------------
        /// 1. Checks whether CategoryId is set or not to generate category id.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns>
        ///     1. returns true if CategoryId is null or empty.
        ///     2. returns false if CategoryId is set.
        /// </returns>
        private bool CanHandleCategoryIdGenerateCommand(object arg)
        {
            return CategoryId == null || CategoryId == string.Empty;
        }

        /// <summary>
        /// HandleCategoryIdGenerateCommand Function
        /// ----------------------------------------
        /// 1. Generate new id by Guid, and sets to CategoryId.
        /// </summary>
        /// <param name="obj"></param>
        private void HandleCategoryIdGenerateCommand(object obj)
        {
            CategoryId = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// CanHandleCategoryManagementSubmitCommand Function
        /// -------------------------------------------------
        /// 1. Checks whether _results list of ValidationResults has any errors.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns>
        ///     1. returns false if there any error present in the _results list.
        ///     2. returns true if the _results list is empty.
        /// </returns>
        private bool CanHandleCategoryManagementSubmitCommand(object arg)
        {
            return !_results.Any();
        }

        /// <summary>
        /// HandleCategoryManagementSubmitCommand Function
        /// ----------------------------------------------
        /// 1. Creates a new category.
        /// 2. Adds the category to the Json repository and CategoriesToList.
        /// 3. ClearFields function is called to clear all textbox values.
        /// </summary>
        /// <param name="obj"></param>
        private void HandleCategoryManagementSubmitCommand(object obj)
        {
            Categories category = new Categories(CategoryId, Name);

            _categoryRepository.AddItem(category);
            CategoriesToList.Add(category);

            ClearFields();

            MessageBox.Show("Category added successfully.");
        }

        /// <summary>
        /// ClearFields Function
        /// --------------------
        /// 1. Makes all properties empty.
        /// </summary>
        private void ClearFields()
        {
            Name = string.Empty;
            CategoryId = string.Empty;
        }

        /// <summary>
        /// Property where we get errors.
        /// </summary>
        public string Error { get; }

        /// <summary>
        /// CategoryViewModel Indexer
        /// -------------------------
        /// 1. Handles the validation of all properties in this class by IDataErroInfo and Data Annotations.
        /// 2. Creates the ValidationContext by the changed property as columnName.
        /// 3. Takes the value of property by GetType, GetProperty and GetValue function from memory.
        /// 4. Validates the context with TryValidateProperty method and stores the errors to _results list.
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns>
        ///     1. returns null if Validation is true.
        ///     2. returns first result in the _results list.
        /// </returns>
        public string this[string columnName]
        {
            get
            {
                ValidationContext context = new ValidationContext(this) { MemberName = columnName };
                _results = new List<ValidationResult>();

                object value = GetType().GetProperty(columnName).GetValue(this);

                bool isValid = Validator.TryValidateProperty(value, context, _results);
                return isValid ? null : _results.First().ErrorMessage;
            }
        }
    }
}
