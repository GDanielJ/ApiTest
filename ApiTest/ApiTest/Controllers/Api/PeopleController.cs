using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiTest.Data;
using ApiTest.Models;
using ApiTest.Dtos;
using ApiTest.Data.Interfaces;
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
        private IPersonService _personService;
        private IMapper _mapper;
        public PeopleController(IPersonService personService, IMapper mapper)
        {
            _personService = personService;
            _mapper = mapper;
        }

        // GET /api/people
        [HttpGet]
        public IActionResult GetPeople()
        {
            return Ok(_personService.GetAll().Select(_mapper.Map<Person, PersonDto>));
        }

        // GET /api/people/{id}
        [HttpGet("{id}")]
        public IActionResult GetPerson(int id)
        {
            var personInDb = _personService.Get(id);

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

            _personService.Create(person);
            _personService.Save();

            personDto.Id = person.Id;

            return Created(new Uri(Request.GetEncodedUrl() + "/" + personDto.Id), personDto);
        }

        // PUT /api/people/{id}
        [HttpPut("{id}")]
        public IActionResult UpdatePerson(int id, PersonDto personDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var personInDb = _personService.Get(id);

            if (personInDb == null)
                return NotFound();

            _mapper.Map(personDto, personInDb);

            _personService.Save();

            return Ok();
        }

        // DELETE /Api/People/{id}
        [HttpDelete("{id}")]
        public IActionResult RemovePerson(int id)
        {
            var personInDb = _personService.Get(id);

            if (personInDb == null)
                return NotFound();

            _personService.Delete(id);
            _personService.Save();

            return Ok();
        }
    }
}