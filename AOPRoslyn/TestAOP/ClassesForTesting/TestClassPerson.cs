using System;
using System.Collections.Generic;
using System.Text;

namespace TestAOP
{
    class TestClassPerson
    {
        public TestClassPerson(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; }
        public string LastName { get; }

        public string Name()
        {
            return FirstName + " " + LastName;
        }
        
        public int Length()
        {
            return Name().Length;
        }
    }
}
