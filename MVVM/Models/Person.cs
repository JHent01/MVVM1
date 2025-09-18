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
    public class Person 
    {
        private string name;
        private int age;
        private string post;
        private string weekend;
        private string pfotoProf;

        public string Name
        { get { return name; }

            set
            { 
                name = value;
              

            }
        }
        public int Age
        { 
            get { return age; }
            set
            {
                age = value;
            }
        
        }
        public string Post
        { 
            get { return post; }
            set
            {
                post = value; 
            }
            
        
        }
        public string Weekend
        {
            get { return weekend; }
            set { weekend = value; }
        }
        public string PfotoProf
        { 
            get { return pfotoProf; }

            set
            {
                pfotoProf = value; 
            }
        }


        
    }
}
