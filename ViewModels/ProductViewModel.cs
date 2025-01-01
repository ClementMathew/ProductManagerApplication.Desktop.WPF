using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Product_Manager.Commands;
using Product_Manager.Models;
using Product_Manager.Repositories;
using Product_Manager.Validators;

namespace Product_Manager.ViewModels
{
    internal class ProductViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly IRepository<Products> _productRepository;
        private readonly IRepository<Categories> _categoryRepository;
        private readonly IRepository<Tags> _tagsRepository;

        private BitmapImage _imageSource;
        private List<ValidationResult> _results;

        public ObservableCollection<Products> ProductsToList { get; set; }
        public ObservableCollection<Categories> CategoriesToList { get; set; }
        public ObservableCollection<Tags> TagsToList { get; set; }

        public RelayCommand ChooseImageCommand { get; set; }
        public RelayCommand ProductsManagementSubmitCommand { get; set; }
        public RelayCommand HomeDeleteCommand { get; set; }

        #region Properties

        private string _name;

        [Required(ErrorMessage = "Name can't be empty.")]
        [MinLength(3, ErrorMessage = "Minimum length not satisfied.")]
        [MaxLength(20, ErrorMessage = "Maximum length exceeded.")]

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
                ProductsManagementSubmitCommand?.OnCanExecuteChanged();
            }
        }

        private int _price;

        [Required(ErrorMessage = "Price can't be empty.")]
        [PriceValidation]

        public int Price
        {
            get { return _price; }
            set
            {
                _price = value;
                OnPropertyChanged();
                ProductsManagementSubmitCommand?.OnCanExecuteChanged();
            }
        }

        private string _description;

        [Required(ErrorMessage = "Description can't be empty.")]
        [MinLength(15, ErrorMessage = "Minimum length not satisfied.")]

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
                ProductsManagementSubmitCommand?.OnCanExecuteChanged();
            }
        }

        private string _imageName;

        [Required(ErrorMessage = "Must choose image.")]

        public string ImageName
        {
            get => _imageName;
            set
            {
                _imageName = value;
                OnPropertyChanged();
                ProductsManagementSubmitCommand?.OnCanExecuteChanged();
            }
        }

        private Products _selectedProductItem;

        public Products SelectedProductItem
        {
            get => _selectedProductItem;
            set
            {
                _selectedProductItem = value;
                OnPropertyChanged();
                HomeDeleteCommand?.OnCanExecuteChanged();
            }
        }

        public ObservableCollection<Categories> SelectedCategoryItems { get; set; }

        public ObservableCollection<Tags> SelectedTagItems { get; set; }

        #endregion

        /// <summary>
        /// ProductViewModel Constructor
        /// -----------------------------
        /// 1. Initialize CategoriesToList by all categories from GetAll function of CategoryRepository.
        /// 2. Initialize ProductsToList by all products from GetAll function of ProductRepository.
        /// 3. Initialize TagsToList by all tags from GetAll function of TagRepository.
        /// 
        /// </summary>
        public ProductViewModel()
        {
            _productRepository = new JsonProductRepository();
            _categoryRepository = new JsonCategoryRepository();
            _tagsRepository = new JsonTagRepository();

            ProductsToList = new ObservableCollection<Products>(_productRepository.GetAll());
            CategoriesToList = new ObservableCollection<Categories>(_categoryRepository.GetAll());
            TagsToList = new ObservableCollection<Tags>(_tagsRepository.GetAll());

            SelectedCategoryItems = new ObservableCollection<Categories>();
            SelectedTagItems = new ObservableCollection<Tags>();

            ChooseImageCommand = new RelayCommand(HandleChooseImageCommand);
            ProductsManagementSubmitCommand = new RelayCommand(HandleProductsManagementSubmitCommand, CanHandleProductsManagementSubmitCommand);
            HomeDeleteCommand = new RelayCommand(HandleHomeDeleteCommand);
        }

        /// <summary>
        /// HandleHomeDeleteCommand Function
        /// --------------------------------
        /// 1. Deletes a selected product from the product listview.
        /// 2. If SelectedProductItem is null then a hint is shown and returns.
        /// 3. If SelectedProductItem is not null then confirmation is asked for deletion.
        /// 4. If confirmed then SelectedProductItem is removed from Json repository and ProductsToList.
        /// </summary>
        /// <param name="obj"></param>
        private void HandleHomeDeleteCommand(object obj)
        {
            if (SelectedProductItem == null)
            {
                MessageBox.Show("You must select a product.");
                return;
            }
            MessageBoxResult result = MessageBox.Show("Delete Product", "Delete Product", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _productRepository.RemoveItem(SelectedProductItem);
                ProductsToList.Remove(SelectedProductItem);
            }
        }

        /// <summary>
        /// CanHandleProductsManagementSubmitCommand Function
        /// -------------------------------------------------
        /// 1. Checks whether _results list of ValidationResults has any errors.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns>
        ///     1. returns false if there any error present in the _results list.
        ///     2. returns true if the _results list is empty.
        /// </returns>
        private bool CanHandleProductsManagementSubmitCommand(object arg)
        {
            return !_results.Any();
        }

        /// <summary>
        /// HandleProductsManagementSubmitCommand Function
        /// ----------------------------------------------
        /// 1. Creates a new product.
        /// 2. Creates a new productid by Guid.
        /// 3. Converts BitmapImage from localdisk to Base64String by MemoryStream and PngBitmapEncoder.
        /// 4. Creating frames by BitmapFrame and adds to object of PngBitmapEncoder.
        /// 5. Then converts MemoryStream object to Base64String.
        /// 6. Creating product object and adds to _productRepository and ProductsToList.
        /// 7. Fields are made empty after successfull submission.
        /// </summary>
        /// <param name="obj"></param>
        private void HandleProductsManagementSubmitCommand(object obj)
        {
            string productId = Guid.NewGuid().ToString();

            string imageSourceBase64;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(_imageSource));
                encoder.Save(memoryStream);
                imageSourceBase64 = Convert.ToBase64String(memoryStream.ToArray());
            };

            Products product = new Products(productId,
                Name, Price, Description, imageSourceBase64, SelectedCategoryItems, SelectedTagItems);

            _productRepository.AddItem(product);
            ProductsToList.Add(product);

            ClearFields();

            MessageBox.Show("Product successfully added.");
        }

        /// <summary>
        /// ClearFields Function
        /// --------------------
        /// 1. Makes all properties empty.
        /// </summary>
        private void ClearFields()
        {
            Name = string.Empty;
            Price = default;
            Description = string.Empty;
            ImageName = string.Empty;
            _imageSource = null;
            SelectedCategoryItems.Clear();
            SelectedTagItems.Clear();
        }

        /// <summary>
        /// HandleChooseImageCommand Function
        /// ---------------------------------
        /// 1. Opens the explorer window to select the image file from local disk using OpenFileDialog.
        /// 2. Applying filter using Filter property.
        /// 3. If opened then the BitmapImage Uri of the image file is assigned.
        /// 4. ImageName property is assigned with filename of the file selected.
        /// 5. _imageSource field is updated with bitmap image object.
        /// </summary>
        /// <param name="obj"></param>
        private void HandleChooseImageCommand(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string selectedFilePath = openFileDialog.FileName;

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(selectedFilePath, UriKind.Absolute);
                    bitmap.EndInit();

                    ImageName = selectedFilePath;
                    _imageSource = bitmap;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        /// <summary>
        /// Property where we get errors.
        /// </summary>
        public string Error { get; }

        /// <summary>
        /// ProductViewModel Indexer
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