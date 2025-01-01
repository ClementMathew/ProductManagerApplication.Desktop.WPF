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
    internal class TagViewModel : ViewModelBase, IDataErrorInfo
    {
        private IRepository<Tags> _tagRepository;
        private List<ValidationResult> _results;

        public ObservableCollection<Tags> TagsToList { get; set; }

        public RelayCommand TagManagementSubmitCommand { get; set; }
        public RelayCommand TagIdGenerateCommand { get; set; }
        public RelayCommand DeleteTagCommand { get; set; }

        #region Properties

        private string _tagId;

        [Required(ErrorMessage = "Must generate a unique id.")]

        public string TagId
        {
            get { return _tagId; }
            set
            {
                _tagId = value;
                OnPropertyChanged();
                TagManagementSubmitCommand?.OnCanExecuteChanged();
            }
        }

        private string _name;

        [Required(ErrorMessage = "Name can't be empty.")]
        [MinLength(3, ErrorMessage = "Minimum length not satisfied.")]
        [MaxLength(20, ErrorMessage = "Maximum length reached.")]

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
                TagManagementSubmitCommand?.OnCanExecuteChanged();
            }
        }

        private Tags _selectedTagItem;

        public Tags SelectedTagItem
        {
            get { return _selectedTagItem; }
            set
            {
                _selectedTagItem = value;
                OnPropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// TagViewModel Constructor
        /// ------------------------
        /// 1. Initialize TagsToList by all tags from GetAll function of TagRepository.
        /// </summary>
        public TagViewModel()
        {
            _tagRepository = new JsonTagRepository();

            TagsToList = new ObservableCollection<Tags>(_tagRepository.GetAll());

            TagManagementSubmitCommand = new RelayCommand(HandleTagManagementSubmitCommand, CanHandleTagManagementSubmitCommand);
            TagIdGenerateCommand = new RelayCommand(HandleTagIdGenerateCommand, CanHandleTagIdGenerateCommand);
            DeleteTagCommand = new RelayCommand(HandleDeleteTagCommand);
        }

        /// <summary>
        /// HandleDeleteTagCommand Function
        /// -------------------------------
        /// 1. Search for the tagid in the TagsToList by command parameter 'obj' and sets SelectedTagItem by tag object.
        /// 2. Asking confirmation to delete the SelectedTagItem.
        /// 3. Removes SelectedTagItem from Json Repository and TagsToList.
        /// </summary>
        /// <param name="obj"></param>
        private void HandleDeleteTagCommand(object obj)
        {
            foreach (Tags tag in TagsToList)
            {
                if (tag.TagId == (string)obj)
                {
                    SelectedTagItem = tag;
                }
            }
            MessageBoxResult result = MessageBox.Show("Delete Tag", "Delete Tag", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _tagRepository.RemoveItem(SelectedTagItem);
                TagsToList.Remove(SelectedTagItem);
            }
        }

        /// <summary>
        /// CanHandleTagIdGenerateCommand Function
        /// --------------------------------------
        /// 1. Checks whether TagId is set or not to generate tag id.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns>
        ///     1. returns true if TagId is null or empty.
        ///     2. returns false if TagId is set.
        /// </returns>
        private bool CanHandleTagIdGenerateCommand(object arg)
        {
            return TagId == null || TagId == string.Empty;
        }

        /// <summary>
        /// HandleTagIdGenerateCommand Function
        /// -----------------------------------
        /// 1. Generate new id by Guid, and sets to TagId.
        /// </summary>
        /// <param name="obj"></param>
        private void HandleTagIdGenerateCommand(object obj)
        {
            TagId = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// CanHandleTagManagementSubmitCommand Function
        /// --------------------------------------------
        /// 1. Checks whether _results list of ValidationResults has any errors.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns>
        ///     1. returns false if there any error present in the _results list.
        ///     2. returns true if the _results list is empty.
        /// </returns>
        private bool CanHandleTagManagementSubmitCommand(object arg)
        {
            return !_results.Any();
        }

        /// <summary>
        /// HandleTagManagementSubmitCommand Function
        /// -----------------------------------------
        /// 1. Creates a new tag.
        /// 2. Adds the tag to the Json repository and TagsToList.
        /// 3. ClearFields function is called to clear all textbox values.
        /// </summary>
        /// <param name="obj"></param>
        private void HandleTagManagementSubmitCommand(object obj)
        {
            Tags tag = new Tags(TagId, Name);

            _tagRepository.AddItem(tag);
            TagsToList.Add(tag);

            ClearFields();

            MessageBox.Show("Tag added successfully.");
        }

        /// <summary>
        /// ClearFields Function
        /// --------------------
        /// 1. Makes all properties empty.
        /// </summary>
        private void ClearFields()
        {
            Name = string.Empty;
            TagId = string.Empty;
        }

        /// <summary>
        /// Property where we get errors.
        /// </summary>
        public string Error { get; }

        /// <summary>
        /// TagViewModel Indexer
        /// --------------------
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
