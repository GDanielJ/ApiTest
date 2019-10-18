using ApiTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTest.Data.Interfaces
{
    public interface IPersonService
    {
        IEnumerable<Person> GetAll();
        Person Get(int id);
        void Create(Person person);
        void Save();

    }
}
