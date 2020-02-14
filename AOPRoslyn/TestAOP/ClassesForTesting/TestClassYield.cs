using System;
using System.Collections.Generic;
using System.Text;

namespace TestAOP.ClassesForTesting
{
    class TestClassYield
    {
       
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Name(string separator = "")
        {
            return FirstName + separator + LastName;
        }

        
    }
}
