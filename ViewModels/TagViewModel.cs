using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product_Manager.Models;
using Product_Manager.Repositories;

namespace Product_Manager.ViewModels
{
    internal class TagViewModel
    {
        private IRepository<Tags> _tagRepository;
        private IEnumerable<Tags> _tags;

        public ObservableCollection<Tags> TagsToList{ get; set; }

        public TagViewModel()
        {
            _tagRepository = new JsonTagRepository();
            _tags = new List<Tags>(_tagRepository.GetAll());

            TagsToList = new ObservableCollection<Tags>(_tags);
        }
    }
}
