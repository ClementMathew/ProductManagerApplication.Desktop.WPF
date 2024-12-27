using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private IRepository<Products> _productRepository;
        private IRepository<Categories> _categoryRepository;
        private IRepository<Tags> _tagsRepository;

        private BitmapImage _imageSource;
        private List<ValidationResult> _results;

        public ObservableCollection<Products> ProductsToList { get; set; }
        public ObservableCollection<Categories> CategoriesToList { get; set; }
        public ObservableCollection<Tags> TagsToList { get; set; }

        public RelayCommand ChooseImageCommand { get; set; }
        public RelayCommand ProductsManagementSubmitCommand { get; set; }

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

        public ObservableCollection<Categories> SelectedCategoryItems { get; set; }

        public ObservableCollection<Tags> SelectedTagItems { get; set; }

        #endregion

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

            ChooseImageCommand = new RelayCommand(HandleChooseImageCommand, CanHandleChooseImageCommand);
            ProductsManagementSubmitCommand = new RelayCommand(HandleProductsManagementSubmitCommand, CanHandleProductsManagementSubmitCommand);
        }

        private bool CanHandleProductsManagementSubmitCommand(object arg)
        {
            return !_results.Any();
        }

        private void HandleProductsManagementSubmitCommand(object obj)
        {
            var productId = Guid.NewGuid().ToString();

            string imageSourceBase64;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(_imageSource));
                encoder.Save(memoryStream);
                imageSourceBase64 = Convert.ToBase64String(memoryStream.ToArray());
            };

            var product = new Products(productId,
                Name, Price, Description, imageSourceBase64, SelectedCategoryItems, SelectedTagItems);

            _productRepository.AddItem(product);
            ProductsToList.Add(product);

            ClearFields();

            MessageBox.Show("Product successfully added.");
        }

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

        private bool CanHandleChooseImageCommand(object arg)
        {
            return true;
        }

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