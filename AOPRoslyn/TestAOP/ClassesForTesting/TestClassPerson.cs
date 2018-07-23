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

        public static TestClassPerson CopyConstructor(TestClassPerson t)
        {
            var newTC = new TestClassPerson(t.FirstName, t.LastName);
            return t;
        }
        public string Name(string separator = "")
        {
            return FirstName + separator + LastName;
        }
        
        public int Length()
        {
            return Name().Length;
        }
    }
}
