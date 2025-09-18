using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MVVM
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        private string name { get; set; }
        private int age { get; set; }
        private EnumPost post;
        private EnumWeekend weekend; 
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
        public int Age
        {
            get { return age; }
            set
            {
                age = value;
                OnPropertyChanged("age");

            }

        }

        public EnumPost Post
        {
            get { return post; }
            set
            {
                post = value;
                OnPropertyChanged("post");
            }
        }
        public EnumWeekend Weekend
        {
            get { return    weekend; }
            set
            {
                weekend = value;
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



       

    }
}
