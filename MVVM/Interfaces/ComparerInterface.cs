using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM
{
   public class ComparerInterface : IComparer<PersonViewModel>
    {
        public int ComparePerson(PersonViewModel? x, PersonViewModel? y)
        {if (x==y)  return 1;  
           
            else return 0;
        }


    }
}
public interface IComparer<in T>
{
    int ComparePerson(T? x, T? y);
}