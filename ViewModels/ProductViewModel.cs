using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Product_Manager.Commands;
using Product_Manager.Models;
using Product_Manager.Repositories;

namespace Product_Manager.ViewModels
{
    internal class ProductViewModel : ViewModelBase, IDataErrorInfo
    {
        private IProductRepository<Products> _productRepository;
        private IEnumerable<Products> _products;

        private IRepository<Categories> _categoryRepository;
        private IEnumerable<Categories> _categories;

        private IRepository<Tags> _tagsRepository;
        private IEnumerable<Tags> _tags;

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
            }
        }

        private int _price;

        [Required(ErrorMessage = "Price can't be empty.")]

        public int Price
        {
            get { return _price; }
            set
            {
                _price = value;
                OnPropertyChanged();
            }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        private string _imageName;

        public string ImageName
        {
            get => _imageName;
            set
            {
                _imageName = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage _imageSource;

        public BitmapImage ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Categories> SelectedCategoryItems { get; set; }

        public ObservableCollection<Tags> SelectedTagItems { get; set; }

        #endregion

        public ProductViewModel()
        {
            _productRepository = new JsonProductRepository();
            _products = _productRepository.GetAll();

            _categoryRepository = new JsonCategoryRepository();
            _categories = _categoryRepository.GetAll();

            _tagsRepository = new JsonTagRepository();
            _tags = _tagsRepository.GetAll();

            ProductsToList = new ObservableCollection<Products>(_products);
            CategoriesToList = new ObservableCollection<Categories>(_categories);
            TagsToList = new ObservableCollection<Tags>(_tags);

            SelectedCategoryItems = new ObservableCollection<Categories>();
            SelectedTagItems = new ObservableCollection<Tags>();

            ChooseImageCommand = new RelayCommand(HandleChooseImageCommand, CanHandleChooseImageCommand);
            ProductsManagementSubmitCommand = new RelayCommand(HandleProductsManagementSubmitCommand, CanHandleProductsManagementSubmitCommand);
        }

        private bool CanHandleProductsManagementSubmitCommand(object arg)
        {
            return true;
        }

        private void HandleProductsManagementSubmitCommand(object obj)
        {
            MessageBox.Show($"{Name}-{Price}-{Description}-{ImageSource}-{SelectedCategoryItems[0].Name}-{SelectedTagItems.Count}");
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
                    bitmap.UriSource = new Uri(selectedFilePath);
                    bitmap.EndInit();

                    ImageName = selectedFilePath;
                    ImageSource = bitmap;
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