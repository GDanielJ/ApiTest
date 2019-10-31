using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ApiTest.Models;
using ApiTest.Data.Interfaces;
using ApiTest.Utility.Profile;

namespace ApiTest.Data
{
    public class PersonService : IPersonService
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;
        public PersonService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<Person> GetAll()
        {
            return _context.People.ToList();
        }
        public Person Get(int id)
        {
            return _context.People.SingleOrDefault(p => p.Id == id);
        }
        public void Create(Person person)
        {
            _context.People.Add(person);
        }

        public void Delete(int id)
        {
            var personInDb = _context.People.SingleOrDefault(p => p.Id == id);
            _context.Remove(personInDb);
        }

        // För update använd Get()

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
