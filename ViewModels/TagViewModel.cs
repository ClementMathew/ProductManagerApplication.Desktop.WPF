using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        #endregion

        public TagViewModel()
        {
            _tagRepository = new JsonTagRepository();

            TagsToList = new ObservableCollection<Tags>(_tagRepository.GetAll());

            TagManagementSubmitCommand = new RelayCommand(HandleTagManagementSubmitCommand, CanHandleTagManagementSubmitCommand);
            TagIdGenerateCommand = new RelayCommand(HandleTagIdGenerateCommand, CanHandleTagIdGenerateCommand);
        }

        private bool CanHandleTagIdGenerateCommand(object arg)
        {
            return TagId == null || TagId == string.Empty;
        }

        private void HandleTagIdGenerateCommand(object obj)
        {
            TagId = Guid.NewGuid().ToString();
        }

        private bool CanHandleTagManagementSubmitCommand(object arg)
        {
            return !_results.Any();
        }

        private void HandleTagManagementSubmitCommand(object obj)
        {
            var tag = new Tags(TagId, Name);

            _tagRepository.AddItem(tag);
            TagsToList.Add(tag);
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
