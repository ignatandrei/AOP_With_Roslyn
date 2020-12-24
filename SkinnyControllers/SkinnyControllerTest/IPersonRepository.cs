using System.Collections.Generic;

namespace SkinnyControllerTest
{
    public interface IPersonRepository
    {
        string X { get; set; }

        void Delete(int id);
        IEnumerable<Person> Get();
        Person Get(int id);
        void Post(Person value);
        void Put(int id, Person value);
    }
}