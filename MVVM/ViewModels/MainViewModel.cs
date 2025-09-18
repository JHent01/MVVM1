using Microsoft.VisualBasic;
using Microsoft.Win32;
using MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace MVVM
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            #region CommandsInitialization
           
            UpdetePhotoProf = new RelayCommand(UpdatePhotoProf); 
            AddCommand = new RelayCommand(Add);
            SaveCommand = new RelayCommand(Save);
            DeleteCommand = new RelayCommand(DeletePerson);
            CanselCommand = new RelayCommand(Cansel);
            ClearAllCommand = new RelayCommand(ClearAll);
            OpenJsonCommand = new RelayCommand(OpenJson);
            SaveJsonCommand = new RelayCommand(SaveJson);
            #endregion
        }

        private ObservableCollection<PersonViewModel> _persons = new ObservableCollection<PersonViewModel>();
        public ObservableCollection<PersonViewModel> Persons
        {
            get => _persons ; 
            set
            {

                    _persons = value;
                    SearchList = Persons.ToList();


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
                Age = (int?)SelectedPerson?.Age??0;
                if (SelectedPerson != null) Post = (EnumPost)(SelectedPerson != null ? SelectedPerson.Post : default);
                if (SelectedPerson != null) Weekend = (EnumWeekend)(SelectedPerson != null ? SelectedPerson.Weekend : default);
                PhotoProf = SelectedPerson?.PhotoProf;


                if (SelectedPerson != null)
                {
                    VisibilityConverts = true;
                }
                else
                {
                    VisibilityConverts = false;
                   
                }
               
                OnPropertyChanged();



            }

        }
      
        private bool _visibilityConvert ;
        public bool VisibilityConverts
        {
            get { return _visibilityConvert; }
            set
            {
                _visibilityConvert = value;
                OnPropertyChanged();
                
            }
        }

        private bool _enableButtonSaveJson;
        public bool EnableButtonSaveJson
        {
            get { return _enableButtonSaveJson; }
            set
            {
                _enableButtonSaveJson = value;
                OnPropertyChanged();
            }
        }
        private bool _enableButtonClear ;
        public bool EnableButtonClear
        {
            get { return _enableButtonClear; }
            set
            {
                _enableButtonClear = value;
                OnPropertyChanged();
            }
        }
        private List<PersonViewModel> _searchList  = new List<PersonViewModel>();
        public List<PersonViewModel> SearchList
        {
            get { return _searchList; }
            set
            {   
                _searchList = value;
                
                
                OnPropertyChanged();
            }
        }
        string _pattern;
        public string Pattern
        {
            get => _pattern;
            set
            {
                _pattern = value;
                if (_pattern != "")
                {
                    SearchList.Clear();


                    SearchList = Persons.Where(p => p.PersonName != null && p.PersonName.ToLower().Contains(Pattern.ToLower())||p.Age.ToString().ToLower().Contains(Pattern.ToLower())).ToList();     //.IndexOf(_pattern, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                        // VisibilityList = true;
                   if (SearchList.Count==0)
                    {
                        SearchList = Persons.ToList();
                    }
                }
                else
                {
                    SearchList = Persons.ToList();
                }

                OnPropertyChanged();
            }
        }
        private bool _visibilityList;
        public bool VisibilityList
        {
            get => _visibilityList;
            set
            {
                _visibilityList = value;
                OnPropertyChanged();
            }
        }


        #region Property


        private string _names;
        public string Names
        {
            get { return _names; }
            set
            {

                _names = value;
                OnPropertyChanged("Names");



            }
        }

        private int _age;
        public int Age
        {
            get { return _age; }
            set
            {
                if (int.TryParse(value.ToString(), out int result))
                {
                    _age = result;
                    OnPropertyChanged("Age");
                }
                    
              

            }
        }

        private EnumWeekend _weekend;
        public EnumWeekend Weekend
        {
            get { return _weekend; }
            set
            {

                _weekend = value;
                OnPropertyChanged("Weekend");

            }
        }

        private BitmapImage _photoProf;
        public BitmapImage PhotoProf
        {
            get { return _photoProf; }
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

        #endregion
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
                SearchList.Clear();
                string jsonContent = File.ReadAllText(dlg.FileName);
                OpenList = System.Text.Json.JsonSerializer.Deserialize<ObservableCollection<Person>>(jsonContent);


                for (int i = 0; i < OpenList.Count; i++)
                {
                    var person = OpenList[i] as Person;
                    if (person != null)
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(person.PfotoProf);// для того что б можно было менять тот же файл из которого брались фотки 
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        var parsePost = (EnumPost)Enum.Parse(typeof(EnumPost), person.Post);
                        var parseWeekend = (EnumWeekend)Enum.Parse(typeof(EnumWeekend), person.Weekend);

                        Persons.Add(new PersonViewModel { PersonName = person.Name, Age = person.Age, Post = parsePost, Weekend = parseWeekend, PhotoProf = bitmap });
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
                FileInfo fileInfo = new FileInfo(dlg.FileName);
                string pach = fileInfo.DirectoryName;
                              
                Directory.CreateDirectory(Path.Combine(pach,"Image"));

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
                        string filePath = Path.Combine(pach, "Image", $"image{i}.png");
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            encoder.Save(fileStream);
                        }
                        peopleToSave.Add(new Person { Name = person.PersonName, Age = person.Age, Post = person.Post.ToString(), Weekend = person.Weekend.ToString(), PfotoProf = filePath });
                    }
                    string jsonContent = System.Text.Json.JsonSerializer.Serialize(peopleToSave);
                    File.WriteAllText(dlg.FileName, jsonContent);
                }
                MessageBox.Show("Файл збережено" + dlg.FileName);
                EnableButtonSaveJson= false;
            }
        }
       
        
        public ICommand SaveCommand { get; private set; }
        private void Save()
        {
            
            PersonViewModel dataToUpdate = null;
            List<string> namesList = new List<string>();
            foreach (var person in Persons)
            {
                namesList.Add(person.PersonName);
            }
            bool check = false;
            for (int i = 0; i < Persons.Count; i++)
            {
                dataToUpdate = Persons[i];// Persons.OfType<PersonViewModel>().ElementAt(i);
                                          // ісправить
                                          // var a = dataToUpdate.GetType().GetProperties().Where(p=> dataToUpdate.PersonName== SelectedPerson.PersonName&& dataToUpdate.Age== Age&& dataToUpdate.Post==Post);
                var a = dataToUpdate.GetHashCode() == SelectedPerson.GetHashCode();
                if (a)
                {
                    check = true;
                    break;
                }

                //if (dataToUpdate.Age == SelectedPerson.Age && dataToUpdate.PersonName == SelectedPerson.PersonName && dataToUpdate.Post.ToString() == SelectedPerson.Post.ToString())
                //{
                //    check = true;

                //    break;
                //}
               
            }
            if (check)
            {
                if (!namesList.Contains(Names))
                {
                    dataToUpdate.PersonName = Names;
                }
                //MessageBox.Show("Таке Імя вже існує");
                dataToUpdate.Age = Age;
                dataToUpdate.Post = (EnumPost)(Post != null ? Post : default);
                dataToUpdate.Weekend = (EnumWeekend)(Weekend != null ? Weekend : default);
                dataToUpdate.PhotoProf = PhotoProf;
                SelectedPerson = dataToUpdate;
            }
               
            
            else 
            { 
                
                if (!namesList.Contains(Names))
                {
                    SelectedPerson = (new PersonViewModel { PersonName = Names, Age = Age, Post = Post, Weekend = Weekend, PhotoProf = PhotoProf });

                    Persons.Add(SelectedPerson);
                    SearchList = Persons.ToList();
                }
                else
               
                MessageBox.Show("Таке Імя вже існує");
               
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
                image = new BitmapImage(new Uri(selectedFileName));
                PhotoProf = image;
                OnPropertyChanged("PhotoProf");
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

                    Names = SelectedPerson?.PersonName;
                   Age = (int)SelectedPerson?.Age;
                   Post = (EnumPost)(SelectedPerson != null ? SelectedPerson.Post : default);
                    Weekend = (EnumWeekend)(SelectedPerson != null ? SelectedPerson.Weekend : default);
                    PhotoProf = SelectedPerson?.PhotoProf;
                    
                    OnPropertyChanged("Names");
                    OnPropertyChanged("Age");
                    OnPropertyChanged("Post");
                    OnPropertyChanged("Weekend");
                    OnPropertyChanged("PhotoProf");
                    VisibilityConverts = false;
                    OnPropertyChanged("visibility");
                }
            }
        }
       
         
        public ICommand AddCommand { get; private set; }
        private void Add()
        {
            SelectedPerson = (new PersonViewModel { PersonName = "New Person", Age = 0, Post = default, Weekend = default, PhotoProf = new BitmapImage(new Uri("/Image/Baze.jpg", UriKind.Relative)) });
            
            EnableButtonSaveJson= true;
            EnableButtonClear = true;
           
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
                       SearchList = Persons.ToList();
                    }
                    VisibilityConverts = false;
                    if (Persons.Count == 0)
                    {
                        EnableButtonSaveJson = false;
                        EnableButtonClear = false;
                    }   
                   
                }
            }
        }
        public ICommand ClearAllCommand { get; private set; }
        private void ClearAll()
        {
            MessageBoxResult result = MessageBox.Show("Видалити весь список?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            {
                if (result == MessageBoxResult.No)
                {
                    return;
                }
                else if (result == MessageBoxResult.Yes)
                {


                    Persons.Clear();
                    SearchList = Persons.ToList();
                    Names = string.Empty;
                    Age = 0;
                    Post = default;//__
                    Weekend = default;
                    PhotoProf= null;
                    VisibilityConverts = false;
                    EnableButtonSaveJson = false;
                    EnableButtonClear= false;
                    
                }
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


