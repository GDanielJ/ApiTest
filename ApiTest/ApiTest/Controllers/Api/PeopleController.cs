using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiTest.Data;
using ApiTest.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiTest.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;
        public PeopleController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET /api/people
        [HttpGet]
        public IEnumerable<Person> GetPeople()
        {
            return _context.People.ToList();
        }

        // GET /api/people/{id}
        [HttpGet]
        public Person GetPerson(int id)
        {
            var personInDb = _context.People.SingleOrDefault(p => p.Id == id);

            //if (personIndb == null)

            return personInDb;
        }

        // POST /api/people
        [HttpPost]
        public Person CreatePerson(Person person)
        {
            //if(!ModelState.IsValid)
            //    throw new HttpResponseException()

            _context.Add(person);
            _context.SaveChanges();

            return person;
        }

        // PUT /api/people/{id}
        [HttpPut]
        public Person UpdatePerson(int id, Person person)
        {
            //if(!ModelState.IsValid)

            var personInDb = _context.People.SingleOrDefault(p => p.Id == id);

            personInDb.Firstname = person.Firstname;
            personInDb.Lastname = person.Lastname;
            personInDb.Email = person.Email;
            personInDb.City = person.City;
            personInDb.DateCreated = DateTime.Now;

            _context.SaveChanges();

            return personInDb;
        }

        // DELETE /Api/People/{id}
        public void RemovePerson(int id)
        {
            var personIdDb = _context.People.SingleOrDefault(p => p.Id == id);

            _context.Remove(personIdDb);
            _context.SaveChanges();
        }
    }
}