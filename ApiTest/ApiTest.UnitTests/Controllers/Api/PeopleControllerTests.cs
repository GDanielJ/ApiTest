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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ApiTest.UnitTests.Controllers.Api
{
    [TestFixture]
    class PeopleControllerTests
    {
        private Mock<IPersonService> _personService;
        private IMapper _mapper;
        private PeopleController _controller;
        private ControllerContext _controllerContext;
        private PersonDto _personDto;

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

            _personService.Setup(ps => ps.Create(It.IsAny<Person>())).Verifiable();

            _controller = new PeopleController(_personService.Object, _mapper);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "http";
            httpContext.Request.Host = new HostString("localhost/api");

            //Controller needs a controller context 
            _controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            _personDto = new PersonDto()
            {
                Id = 3,
                Firstname = "Karl",
                Lastname = "Karlsson",
                City = "Oslo",
                Email = "karl.karlsson@test.com"
            };
        }

        // GetPeople
        [Test]
        public void GetPeople_WhenCalled_ReturnStatusCode200()
        {
            var result = _controller.GetPeople();

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void GetPeople_WhenCalled_ReturnInstanceOfIEnumerablePersonDto()
        {
            var result = _controller.GetPeople();

            var okObject = result as OkObjectResult;
            Assert.IsInstanceOf<IEnumerable<PersonDto>>(okObject.Value);
        }

        [Test]
        public void GetPeople_WhenCalled_ReturnInstanceOfOkObjectResult()
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
        [Test]
        public void CreatePerson_WhenCalled_IsNotNull()
        {
            //assign context to controller
            _controller.ControllerContext = _controllerContext;

            var result = _controller.CreatePerson(_personDto);

            var createdResult = result as CreatedResult;

            Assert.IsNotNull(createdResult);
        }

        [Test]
        public void CreatePerson_WhenCalled_ReturnStatusCode201()
        {
            _controller.ControllerContext = _controllerContext;

            var result = _controller.CreatePerson(_personDto);

            var createdResult = result as CreatedResult;

            var statusCode = createdResult.StatusCode;

            Assert.That(statusCode, Is.EqualTo(201));
        }

        [Test]
        public void CreatePerson_WhenCalled_ReturnCorrectUri()
        {
            _controller.ControllerContext = _controllerContext;

            var result = _controller.CreatePerson(_personDto);

            var createdResult = result as CreatedResult;

            var location = createdResult.Location;

            Assert.That(location, Is.EqualTo("http://localhost/api//3"));
        }

        public void CreatePerson_WhenCalledWithIncompleteModel_ReturnBadRequest()
        {
            // TODO
        }

        // UpdatePerson

        // DeletePerson


        private IMapper GenerateConcreteInstance()
        {
            var config = new AutoMapper.MapperConfiguration(c =>
            {
                c.AddProfile(new ApplicationProfile());
            });

            return config.CreateMapper();
        }





        // https://asp.net-hacker.rocks/2019/01/15/unit-testing-data-access-dotnetcore.html
        // https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/testing?view=aspnetcore-3.0
    }
}
