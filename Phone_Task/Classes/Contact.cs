using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phone_Task.Classes
{
    public class Contact
    {
       
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Number{ get; set; }

        public override string ToString()
        {
            return $"Ad: {FullName}, Nomre: {Number}"; 
                
        }
    }
}
