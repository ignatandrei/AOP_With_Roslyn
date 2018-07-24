using System;
using System.Collections.Generic;
using System.Text;

namespace TestAOP
{
    class TestClassPersonWithArguments
    { 
        public TestClassPersonWithArguments(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; }
        public string LastName { get; }

        public static TestClassPersonWithArguments CopyConstructor(TestClassPersonWithArguments t)
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
        public static TestClassPersonWithArguments ConstructorFromProperties(string firstName, string lastName)
        {
            return new TestClassPersonWithArguments(firstName, lastName);
        }
    }
}
