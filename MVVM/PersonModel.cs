using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM
{
    public class PersonModel
    {
        public string Name { get; set; }
            public string Age { get; set; }
            public string Post { get; set; }
            public bool Wikend { get; set; }
            public string PhotoProf { get; set; }



            public PersonModel(string name, string age, string post, bool wikend, string photoProf)
            {
                Name = name;
                Age = age;
                Post = post;
                Wikend = wikend;
                PhotoProf = photoProf;
            }
        
    }
}
