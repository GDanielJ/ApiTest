using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ApiTest.Models;
using ApiTest.Data;
using ApiTest.Controllers.Api;
using ApiTest.Utility.Profile;
using ApiTest.Data.Interfaces;
using NUnit.Framework;
using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ApiTest.UnitTests.Controllers.Api
{
    [TestFixture]
    class PeopleControllerTests
    {
        private IPersonService _personService;
        private IMapper _mapper;
        private PeopleController _controller;


        // OBS! Ändrat nu så att jag använder unitOfWork istället. 2019-10-18. Kommer Mocka med den...
        [SetUp]
        public void SetUp()
        {
            _mapper = GenerateConcreteInstance();

            //var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDb").Options;
            //_personService = new PersonService();

            //_context.People.Add(new Person()
            //{ Id = 1, Firstname = "Torsten", Lastname = "Torstensson", Email = "torsten@gmail.com", City = "Berlin", DateCreated = DateTime.Now });
            //_context.People.Add(new Person()
            //{ Id = 2, Firstname = "Uno", Lastname = "Unosson", Email = "uno@gmail.com", City = "Amsterdam", DateCreated = DateTime.Now });
            //_context.SaveChanges();

            //_controller = new PeopleController(_context, _mapper); // Redan här blir det fel tror jag
        }

        // Det här testet funkar!
        [Test]
        public void GetAll_WhenCalled_ReturnStatusCode200()
        {
            var result = _controller.GetPeople();

            Assert.IsInstanceOf<OkObjectResult>(result);
        }


        // Det här lyckas jag inte lösa. Jag vill testa så att jag hämtar rätt data. Jag har bytt från att mocka till att använda InMemory-databas.
        // Läste att det är så man bör göra för att testa DbContext.
        // De två Person-objekt som jag lägger in i min InMemort-db läsers in korrekt. Men när jag ska kolla metoden hämtar ut dem igen, så får
        // jag bara tillbaka Null. Lite osäker på vad min Assert ska vara. Tänkte kolla så att Count() = 2 eller nåt sånt.

        [Test]
        public void GetAll_WhenCalled_ReturnPeopleInDb()
        {
            var result = _controller.GetPeople();

            var okObjectResult = result as OkObjectResult;
            var content = okObjectResult.Value as IEnumerable<Person>; //Funkar inte
            Assert.IsNotNull(content);
            //Assert.AreEqual(result);

        }

        [Test]
        public void GetPerson_CallWithInvalidId_ReturnStatusCode404()
        {
            var result = _controller.GetPerson(99);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }


        // Fungerar inte
        [Test]
        public void GetPerson_CallWithValidId_ReturnStatusCode200()
        {
            var result = _controller.GetPerson(1);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }


        private IMapper GenerateConcreteInstance()
        {
            var config = new AutoMapper.MapperConfiguration(c =>
            {
                c.AddProfile(new ApplicationProfile());
            });

            return config.CreateMapper();
        }
    }
}
