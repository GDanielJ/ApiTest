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
using Microsoft.AspNetCore.Http.Extensions;

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
        public IActionResult GetPeople()
        {
            return Ok(_context.People.ToList().Select(_mapper.Map<Person, PersonDto>));
        }

        // GET /api/people/{id}
        [HttpGet("{id}")]
        public IActionResult GetPerson(int id)
        {
            var personInDb = _context.People.SingleOrDefault(p => p.Id == id);

            if (personInDb == null)
                return NotFound();

            return Ok(_mapper.Map<Person, PersonDto>(personInDb));
        }

        // POST /api/people
        [HttpPost]
        public IActionResult CreatePerson(PersonDto personDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var person = _mapper.Map<PersonDto, Person>(personDto);
            person.DateCreated = DateTime.Now;

            _context.Add(person);
            _context.SaveChanges();

            personDto.Id = person.Id;

            return Created(new Uri(Request.GetEncodedUrl() + "/" + personDto.Id), personDto);
        }

        // PUT /api/people/{id}
        [HttpPut("{id}")]
        public IActionResult UpdatePerson(int id, PersonDto personDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var personInDb = _context.People.SingleOrDefault(p => p.Id == id);

            if (personInDb == null)
                return NotFound();

            _mapper.Map(personDto, personInDb);

            _context.SaveChanges();

            return Ok();
        }

        // DELETE /Api/People/{id}
        [HttpDelete("{id}")]
        public IActionResult RemovePerson(int id)
        {
            var personIdDb = _context.People.SingleOrDefault(p => p.Id == id);

            if (personIdDb == null)
                return NotFound();

            _context.Remove(personIdDb);
            _context.SaveChanges();

            return Ok();
        }
    }
}