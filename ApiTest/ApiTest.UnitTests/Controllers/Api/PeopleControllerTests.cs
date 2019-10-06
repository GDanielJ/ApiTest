using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ApiTest.Models;
using ApiTest.Data;
using ApiTest.Controllers.Api;
using ApiTest.Utility.Profile;
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
        private ApplicationDbContext _context;
        private IMapper _mapper;
        private PeopleController _controller;

        [SetUp]
        public void SetUp()
        {
            _mapper = GenerateConcreteInstance();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDb").Options;
            _context = new ApplicationDbContext(options);

            _context.People.Add(new Person()
            { Id = 1, Firstname = "Torsten", Lastname = "Torstensson", Email = "torsten@gmail.com", City = "Berlin", DateCreated = DateTime.Now });
            _context.People.Add(new Person()
            { Id = 2, Firstname = "Uno", Lastname = "Unosson", Email = "uno@gmail.com", City = "Amsterdam", DateCreated = DateTime.Now });

            _controller = new PeopleController(_context, _mapper);
        }

        [Test]
        public void TestGet_WhenCalled_ReturnStatusCode200()
        {
            var result = _controller.GetPeople();

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
