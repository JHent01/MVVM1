using Microsoft.Win32;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;

using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace MVVM
{
    public  class MainViewModel : INotifyPropertyChanged 
    {
        public MainViewModel()
        {
            UpdetePhotoProf = new RelayCommand(UpdatePhotoProf);
            DeleteCommand = new RelayCommand(DeletePerson);
            AddCommand = new RelayCommand(Add);
            SaveCommand = new RelayCommand(Save);
            CanselCommand = new RelayCommand(Cansel);
            ClearAllCommand = new RelayCommand(ClearAll);
            OpenJsonCommand = new RelayCommand(OpenJson);
            SaveJsonCommand = new RelayCommand(SaveJson);
        }
      
        private ObservableCollection<PersonViewModel> _persons = new ObservableCollection<PersonViewModel>();
        public ObservableCollection<PersonViewModel> Persons
        {
            get => _persons;
            set
            {
                
                _persons = value;
                OnPropertyChanged();
            }
        }
      
        private PersonViewModel _selectedPerson;
        public PersonViewModel SelectedPerson
        { 
            get { return _selectedPerson; }
            set
            {
                

                    _selectedPerson = value;
                    Names = SelectedPerson?.PersonName;
                    Age = SelectedPerson?.Age;
                    if (SelectedPerson != null) Post = (EnumPost)(SelectedPerson != null ? SelectedPerson.Post : default);
                    if (SelectedPerson != null) Weekend = (EnumWeekend)(SelectedPerson != null ? SelectedPerson.Weekend : default);
                    PhotoProf = SelectedPerson?.PhotoProf;
          

                if (SelectedPerson != null)
                {
                    VisibilityConvert = "visibility";
                }
                else 
                {
                    VisibilityConvert = null;
                    //Visibility.Collapsed; 
                }
               
                OnPropertyChanged("SelectedPerson");


                
            }

        }

        //private Visibility _visibility = Visibility.Hidden;
        //public Visibility Visibility
        //{
        //    get { return _visibility; }
        //    set
        //    {
        //        _visibility = value;
        //        OnPropertyChanged("visibility");
        //    }
        //}
        private object _visibilityConvert = new VisibilityConvert().Convert(null,null,null,null);
        public object VisibilityConvert
        {
            get { return _visibilityConvert; }
            set
            {
                _visibilityConvert = value;// проблма в нуле . не обрабатывается илиже не конвертируется 
                OnPropertyChanged("visibilityConvert");
            }
        }








        //private object _visibility = visibilityConvert.Convert();
        //public object Visibility
        //{
        //    get { return _visibility; }
        //    set
        //    {
        //        if (_visibility == null) _visibility= visibilityConvert.Convert(null, null, null, null); 

        //        _visibility = visibilityConvert.Convert(value,null,null,null);
        //        OnPropertyChanged("visibility");
        //    }
        //}

        private string _names;
        public string Names
        {
            get  { return _names; }
            set
            {
                
                    _names = value;
                    OnPropertyChanged("Names");
                
              
                
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

       private EnumWeekend _weekend;
        public EnumWeekend Weekend
        {
            get {return _weekend; }
            set
            {

                _weekend = value;
                    OnPropertyChanged("Weekend");
                
            }
        }

        private BitmapImage _photoProf;
        public BitmapImage PhotoProf
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
    

        #region Commands
        public ObservableCollection<Person> OpenList { get; set; } = new ObservableCollection<Person>();
        public ICommand OpenJsonCommand { get; private set; }
        private void OpenJson()
        {
        
            OpenFileDialog dlg = new OpenFileDialog();
            var people = new List<PersonViewModel>();
            
            dlg.Filter = "Json documents (.json)|*.json";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                Persons.Clear();
                
                string jsonContent = File.ReadAllText(dlg.FileName);
                OpenList = System.Text.Json.JsonSerializer.Deserialize<ObservableCollection<Person>>(jsonContent);
                

                for (int i = 0; i < OpenList.Count; i++)
                {
                    var person = OpenList[i] as Person;
                    if (person != null)
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(person.PfotoProf);
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        var parsePost = (EnumPost)Enum.Parse(typeof(EnumPost), person.Post);
                        var parseWeekend = (EnumWeekend)Enum.Parse(typeof(EnumWeekend), person.Weekend);

                       Persons.Add(new PersonViewModel {PersonName= person.Name, Age=person.Age,Post= parsePost,Weekend= parseWeekend, PhotoProf= bitmap });
                    }
                }
            }
        }

        public ICommand SaveJsonCommand { get; private set; }
        private void SaveJson()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Json documents (.json)|*.json";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string pach = dlg.FileName.Remove(dlg.FileName.Length - dlg.SafeFileName.Length, dlg.SafeFileName.Length);
                Directory.CreateDirectory(pach + "Image");
                var peopleToSave = new ObservableCollection<Person>();
             
                for (int i = 0; i < Persons.Count; i++)
                {
                    var person = Persons[i] as PersonViewModel;
                    if (person != null)
                    {
                        if (person.Post.ToString() == null)
                        {
                            person.Post = EnumPost.Стажер;
                        }
                        var encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create((BitmapSource)person.PhotoProf));

                        using (var fileStream = new FileStream(System.IO.Path.Combine(pach + "Image", $"image{i}.png"), FileMode.Create))
                        {
                            encoder.Save(fileStream);
                        }
                        peopleToSave.Add(new Person { Name = person.PersonName, Age= person.Age, Post=person.Post.ToString(),Weekend= person.Weekend.ToString(),PfotoProf= System.IO.Path.Combine(pach + "Image", $"image{i}.png") });
                    }
                    string jsonContent = System.Text.Json.JsonSerializer.Serialize(peopleToSave);
                    File.WriteAllText(dlg.FileName, jsonContent);
                }
            }
        }


        public ICommand SaveCommand { get; private set; }
        private void Save()
        {
            //var a = SelectedPerson?.PersonName;

           
            //PersonViewModel dataToUpdate = Persons.FirstOrDefault(d => d.PersonName == a);
            PersonViewModel dataToUpdate = null;
            
            bool check=false;
            for (int i = 0; i < Persons.Count; i++)
            {
                dataToUpdate = Persons.OfType<PersonViewModel>().ElementAt(i);
                if (dataToUpdate.Age==SelectedPerson.Age&& dataToUpdate.PersonName==SelectedPerson.PersonName&& dataToUpdate.Post.ToString()==SelectedPerson.Post.ToString())
                {
                    check = true;
                   
                    break;
                }
            }



            if (check)
            {
                dataToUpdate.PersonName = Names;
                dataToUpdate.Age = Age;
                dataToUpdate.Post = (EnumPost)(Post != null ? Post : default);
                dataToUpdate.Weekend = (EnumWeekend)(Weekend != null ? Weekend : default);
                dataToUpdate.PhotoProf = PhotoProf;
               

            }
            else 
            { 
                Persons.Add(new PersonViewModel { PersonName = Names, Age = Age, Post = Post, Weekend = Weekend, PhotoProf = PhotoProf });
                SelectedPerson = null;
            }
            
            
        }


        public ICommand AddCommand { get; private set; }
        private void Add()
        {
            SelectedPerson=(new PersonViewModel { PersonName = "New Person", Age = "0", Post = default, Weekend = default, PhotoProf = new BitmapImage(new Uri("/Image/Baze.jpg", UriKind.Relative)) });
            OnPropertyChanged("Persons");
        }

        public ICommand DeleteCommand { get; private set; }
        private void DeletePerson()
        {
            MessageBoxResult result = MessageBox.Show("Видалити Людину?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            {
                if (result == MessageBoxResult.No)
                {
                    return;
                }
                else if (result == MessageBoxResult.Yes)
                {

                    if (SelectedPerson != null)
                    {//_persons??
                        Persons.Remove(SelectedPerson);
                        SelectedPerson = null;
                        OnPropertyChanged("Persons");
                    }
                }
            }
        }

        public ICommand CanselCommand { get; private set; }
        private void Cansel()
        {
            MessageBoxResult result = MessageBox.Show("В1дхилити зм1ни?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            {
                if (result == MessageBoxResult.No)
                {
                    return;
                }
                else if (result == MessageBoxResult.Yes)
                {

                    _names = SelectedPerson?.PersonName;
                    _age = SelectedPerson?.Age;
                    _post = (EnumPost)(SelectedPerson != null ? SelectedPerson.Post : default);
                    _weekend = (EnumWeekend)(SelectedPerson != null ? SelectedPerson.Weekend : default);
                    _photoProf = SelectedPerson?.PhotoProf;
                    //SelectedPerson = _selectedPerson;
                    OnPropertyChanged("Names");
                    OnPropertyChanged("Age");
                    OnPropertyChanged("Post");
                    OnPropertyChanged("Weekend");
                    OnPropertyChanged("PhotoProf");
                    VisibilityConvert = null;
                    OnPropertyChanged("visibility");
                }
            }
        }

        public ICommand ClearAllCommand { get; private set; }
        private void ClearAll()
        { MessageBoxResult result = MessageBox.Show("Видалити весь список?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            {
                if (result == MessageBoxResult.No)
                {
                    return;
                }
                else if (result == MessageBoxResult.Yes)
                {
                

                Persons.Clear();
                _names = string.Empty;
                _age = string.Empty;
                _post = default;
                _weekend = default;
                _photoProf = null;
                OnPropertyChanged("Names");
                OnPropertyChanged("Age");
                OnPropertyChanged("Post");
                OnPropertyChanged("Weekend");
                OnPropertyChanged("PhotoProf");
                    VisibilityConvert =null;
                    //visibilityConvert.Convert(null, null, null, null);
                    OnPropertyChanged("visibility");
                }
            }
        }


        public ICommand UpdetePhotoProf { get; private set; }
        private void UpdatePhotoProf()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFileName = openFileDialog.FileName;

                BitmapImage image = new BitmapImage();
                image= new BitmapImage(new Uri(selectedFileName));
                PhotoProf = image;
                OnPropertyChanged("PhotoProf");
            }
        }
        #endregion

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
