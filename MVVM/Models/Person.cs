using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MVVM
{
    public class Person //: INotifyPropertyChanged
    {
        private string name;
        private string age;
        private string post;
        private bool weekend;
        private string pfotoProf;

        public string Name
        { get { return name; }

            set
            { 
                name = value;
                //OnPropertyChanged("Name");

            }
        }
        public string Age
        { 
            get { return age; }
            set
            {
                age = value; /*OnPropertyChanged("Age");*/
            }
        
        }
        public string Post
        { 
            get { return post; }
            set
            {
                post = value; /*OnPropertyChanged("Post");*/
            }
            
        
        }
        public bool Weekend
        {
            get { return weekend; }
            set { weekend = value; /*OnPropertyChanged("Weekend"); }*/}
        }
        public string PfotoProf
        { 
            get { return pfotoProf; }

            set
            {
                pfotoProf = value; /*OnPropertyChanged("PfotoProf");*/
            }
        }


        //public event PropertyChangedEventHandler PropertyChanged;
        //public void OnPropertyChanged([CallerMemberName] string prop = "")
        //{
        //    if (PropertyChanged != null)
        //        PropertyChanged(this, new PropertyChangedEventArgs(prop));
        //}

    }
}
