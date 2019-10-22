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
using ApiTest.Dtos;

namespace ApiTest.UnitTests.Controllers.Api
{
    [TestFixture]
    class PeopleControllerTests
    {
        private Mock<IPersonService> _personService;
        private IMapper _mapper;
        private PeopleController _controller;

        [SetUp]
        public void SetUp()
        {
            _mapper = GenerateConcreteInstance();

            _personService = new Mock<IPersonService>();
            _personService.Setup(ps => ps.GetAll()).Returns(new List<Person>
            {
                new Person()
                    { Id = 1, Firstname = "Torsten", Lastname = "Torstensson", Email = "torsten@gmail.com", City = "Berlin", DateCreated = DateTime.Now },
                new Person()
                    { Id = 2, Firstname = "Uno", Lastname = "Unosson", Email = "uno@gmail.com", City = "Amsterdam", DateCreated = DateTime.Now }
            }.AsQueryable());

            _personService.Setup(ps => ps.Get(1)).Returns(new Person()
            {
                Id = 1,
                Firstname = "Torsten",
                Lastname = "Torstensson",
                Email = "torsten@gmail.com",
                City = "Berlin",
                DateCreated = DateTime.Now
            });

            _controller = new PeopleController(_personService.Object, _mapper);
        }


        // TODO - Fixa till denna. Fungerar! Men ska returnera status 200. lyft ut andra testet i ett eget.
        // GetAll
        [Test]
        public void GetAll_WhenCalled_ReturnStatusCode200()
        {
            var result = _controller.GetPeople();

            var okObject = result as OkObjectResult;
            // Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<IEnumerable<PersonDto>>(okObject.Value);
            //var idea = returnValue.FirstOrDefault();
            //Assert.Equal("One", idea.Name);

            //Assert.That(result,Is.InstanceOf<ActionResult<IEnumerable<Person>>>());

            //Den gamla:   Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void GetAll_WhenCalled_ReturnPeopleInDb()
        {
            var result = _controller.GetPeople();

            var okObjectResult = result as OkObjectResult;

            var content = okObjectResult.Value; // as IEnumerable<Person>; //Funkar inte som IEnum...

            Assert.IsNotNull(content);
        }

        // GetPerson
        [Test]
        public void GetPerson_CallWithInvalidId_ReturnStatusCode404()
        {
            var result = _controller.GetPerson(99);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void GetPerson_CallWithValidId_ReturnStatusCode200()
        {
            var result = _controller.GetPerson(1);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void GetPerson_CallWithValidId_ReturnPersonWithId1()
        {
            var result = _controller.GetPerson(1);

            var okObjectResult = result as OkObjectResult;

            var content = okObjectResult.Value;

            Assert.IsNotNull(content);
        }

        // CreatePerson



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
