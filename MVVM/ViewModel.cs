using Microsoft.Win32;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MVVM
{
    public  class ViewModel : INotifyPropertyChanged 
    {
        public ViewModel()
        {
            Persons = new ObservableCollection<PersonViewModel> { new PersonViewModel { Name = "123", Age = "2", Post = "2", Weekend = true, PfotoProf = "" }, new Person { Name = "12", Age = "2", Post = "2", Weekend = true, PfotoProf = "" } };
            SaveCommand = new DelegateCommand(Save);
        }

        public ObservableCollection<PersonViewModel> Persons { get; set; }

        private Person _selectedPerson;
        public Person SelectedPerson
        { 
            get { return _selectedPerson; }
            set
            {
                _selectedPerson = value;
                OnPropertyChanged("SelectedPerson");
            }
        
        }
        private EnumPost _enumPost;//?
        public EnumPost EnumPost//?
        {
            get => _enumPost;
            set
            {
                _enumPost = value;
              //  OnPropertyChanged("EnumPost");
            }
        }          

        public DelegateCommand SaveCommand { get; } 
        private void Save() 
        {
            //;Persons = new ObservableCollection<Person> { new Person { Name = "123", Age = "2", Post = "2", Weekend = true, PfotoProf = "" }, new Person { Name = "12", Age = "2", Post = "2", Weekend = true, PfotoProf = "" } };
            return;
        }
      //  private static void Load1( ) { return; }

        public void SelectedP( string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }

        }



        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
               PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        #endregion
    }
}
