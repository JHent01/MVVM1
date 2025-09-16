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
using System.Windows.Controls;
using System.Windows.Input;

namespace MVVM
{
    public  class MainViewModel : INotifyPropertyChanged 
    {
        public MainViewModel()
        {
            Persons = new ObservableCollection<PersonViewModel> { new PersonViewModel { Name = "123", Age = "2", Post =default, Weekend = true, PhotoProf = new Image() }, new PersonViewModel { Name = "12", Age = "2", Post = default, Weekend = true, PhotoProf = new Image() } };
            

            SaveCommand = new RelayCommand(obj => Save(new PersonViewModel { Name=_names,Age=_age, Post=_post,Weekend=_weekend,PhotoProf=_photoProf })/*, obj => CanSaveData()*/);
        }
        public ObservableCollection<PersonViewModel> _persons//????
        {
            get { return Persons; }
            set { Persons = value;
                OnPropertyChanged("Person");//????
            }

        }
        public ObservableCollection<PersonViewModel> Persons { get; set; }

        private PersonViewModel _selectedPerson;
        public PersonViewModel SelectedPerson
        { 
            get { return _selectedPerson; }
            set
            {
               

                _selectedPerson = value;
                OnPropertyChanged("SelectedPerson");
                Names = SelectedPerson?.Name;
                Age = SelectedPerson?.Age;
                Post = (EnumPost)(SelectedPerson != null ? SelectedPerson.Post : default);
                Weekend = SelectedPerson != null ? SelectedPerson.Weekend : false;
                PhotoProf = SelectedPerson?.PhotoProf;
            }
        
        }

        //public ICommand SaveCommand => new RelayCommand(obj => Save(), obj => true);
        private string _names;
        public string Names
        {
            get  { return _names; }
            set
            {
                
                    _names = value;
                    OnPropertyChanged("Names");
                
               // _names = value;
               //OnPropertyChanged("Names");
                
            }
        }
        private string _age;
        public string Age
        {
            get { return _age; }
            set
            {

                _age = value;
                    OnPropertyChanged("Age");
                
            }
        }
       private bool  _weekend;
        public bool Weekend
        {
            get {return _weekend; }
            set
            {

                _weekend = value;
                    OnPropertyChanged("Weekend");
                
            }
        }
        private Image _photoProf;
        public Image PhotoProf
        {
            get {return _photoProf; }
            set
            {

                _photoProf = value;
                    OnPropertyChanged("PhotoProf");
                
            }
        }
        private EnumPost _post;
        public EnumPost Post
        {
            get => _post;
            set
            {
                if (_post == value) return;
                _post = value;
                OnPropertyChanged(nameof(Post));
            }
        }



        public ICommand SaveCommand { get; private set; }

        private void Save(PersonViewModel personViewModel)
        {
            var a = SelectedPerson?.Name;

           
            PersonViewModel dataToUpdate = Persons.FirstOrDefault(d => d.Name == a);
            if (dataToUpdate != null)
            {
                dataToUpdate.Name = personViewModel.Name;
                dataToUpdate.Age = personViewModel.Age;
                dataToUpdate.Post = (EnumPost)(personViewModel != null ? personViewModel.Post : default);
                dataToUpdate.Weekend = personViewModel != null ? personViewModel.Weekend : false;
                dataToUpdate.PhotoProf = personViewModel.PhotoProf;
                OnPropertyChanged("Persons");
            }
            //Persons.Remove(SelectedPerson);
            //Persons.Add(personViewModel);
             //SelectedPerson = personViewModel;
           // OnPropertyChanged("Name");
        }

        //private bool CanSaveData()
        //{

        //    return SelectedPerson != null;
        //}


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

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
            
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
