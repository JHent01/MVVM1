using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MVVM
{
    public class PersonViewModel
    {
        public string name { get; set; }
        public string age { get; set; }
        public EnumPost post;
        public bool weekend; //Enum?
        public Image photoProf;



        //public PersonViewModel(string name, string age, EnumPost post, bool wikend, Image photoProf)
        //{
        //    Name = name;
        //    Age = age;
        //    Post = post;
        //    Wikend = wikend;
        //    PhotoProf = photoProf;
        //}
       
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("name");
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
        public bool Weekend
        {
            get { return    weekend; }
            set
            {
                weekend = value;
                OnPropertyChanged("weekend");
            }


        }
        public Image PhotoProf
        {
            get { return photoProf; }
            set
            {
                photoProf = (Image)value;
                OnPropertyChanged("pfotoprof");

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        //public personviewmodel(person person)
        //{
        //    _person = person;
        //}

    }
}
