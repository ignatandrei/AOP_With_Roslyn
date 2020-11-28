using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkinnyControllerTest
{
    public record Person(int id, string Name);

    public class PersonRepository
    {
                
        public IEnumerable<Person> Get()
        {
            yield return new Person(1, "Andrei Ignat 1");
            yield return new Person(2, "Ignat Andrei 2");

        }
        public Person Get(int id)
        {
            return new Person(id, $"Ignat{id}Andrei");
        }
        public void Post(Person value)
        {
            //save person
            Console.WriteLine("POST"+value?.id);
            return;
        }
        public void Put(int id, Person value)
        {
            //update person
            Console.WriteLine("PUT" + value?.id);
            return;

        }
        
        public void Delete(int id)
        {
            //delete
            Console.WriteLine("Delete " + id);
            return;

        }

    }

}
