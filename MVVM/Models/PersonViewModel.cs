using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MVVM
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        private string name { get; set; }
        private string age { get; set; }
        private EnumPost post;
        private EnumWeekend weekend; //Enum?
        private BitmapImage photoProf;

       
        public string PersonName
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        public string Age
        {
            get { return age; }
            set
            {
                age = value;
                OnPropertyChanged("age");

            }

        }
        public Enum Post
        {
            get { return post; }
            set
            {
                post = value is EnumPost ep ? ep : default;
                OnPropertyChanged("post");
            }
        }
        public Enum Weekend
        {
            get { return    weekend; }
            set
            {
                weekend = value is EnumWeekend ep ? ep : default;
                OnPropertyChanged("weekend");
            }


        }
        public BitmapImage PhotoProf
        {
            get { return photoProf; }
            set
            {
                photoProf = (BitmapImage)value;
                OnPropertyChanged("pfotoprof");

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }



        //public PersonViewModel(string name, string age, EnumPost post, bool wikend, Image photoProf)
        //{
        //    Name = name;
        //    Age = age;
        //    Post = post;
        //    Wikend = wikend;
        //    PhotoProf = photoProf;
        //}

        //public personviewmodel(person person)
        //{
        //    _person = person;
        //}

    }
}
