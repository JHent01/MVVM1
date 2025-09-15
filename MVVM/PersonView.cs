using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MVVM
{
    public class PersonView
    {
        public string Name { get; set; }
        public string Age { get; set; }
        public EnumPost Post;
        public bool Wikend; //Enum?
        public Image PhotoProf;



        public PersonView(string name, string age, EnumPost post, bool wikend, Image photoProf)
        {
            Name = name;
            Age = age;
            Post = post;
            Wikend = wikend;
            PhotoProf = photoProf;
        }
    }
}
