using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiTest.Data;
using ApiTest.Models;
using ApiTest.Dtos;
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
        public IEnumerable<PersonDto> GetPeople()
        {
            return _context.People.ToList().Select(_mapper.Map<Person, PersonDto>);
        }

        // GET /api/people/{id}
        [HttpGet("{id}")]
        public PersonDto GetPerson(int id)
        {
            var personInDb = _context.People.SingleOrDefault(p => p.Id == id);

            //if (personIndb == null)

            return _mapper.Map<Person, PersonDto>(personInDb);
        }

        // POST /api/people
        [HttpPost]
        public PersonDto CreatePerson(PersonDto personDto)
        {
            //if(!ModelState.IsValid)
            //    throw new HttpResponseException()

            var person = _mapper.Map<PersonDto, Person>(personDto);
            person.DateCreated = DateTime.Now;

            _context.Add(person);
            _context.SaveChanges();

            personDto.Id = person.Id;

            return personDto;
        }

        // PUT /api/people/{id}
        [HttpPut("{id}")]
        public PersonDto UpdatePerson(int id, PersonDto personDto)
        {
            //if(!ModelState.IsValid)

            var personInDb = _context.People.SingleOrDefault(p => p.Id == id);

            _mapper.Map(personDto, personInDb);

            _context.SaveChanges();

            return personDto;
        }

        // DELETE /Api/People/{id}
        [HttpDelete("{id}")]
        public void RemovePerson(int id)
        {
            var personIdDb = _context.People.SingleOrDefault(p => p.Id == id);

            _context.Remove(personIdDb);
            _context.SaveChanges();
        }
    }
}