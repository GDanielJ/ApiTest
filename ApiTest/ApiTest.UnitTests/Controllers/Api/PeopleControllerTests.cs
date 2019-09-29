using ApiTest.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using Moq;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ApiTest.Data;
using ApiTest.Controllers.Api;

namespace ApiTest.UnitTests.Controllers.Api
{
    [TestFixture]
    class PeopleControllerTests
    {
        [Test]
        public void GetPeople_WhenCalled_ReturnListOfPeople()
        {
            var context = new Mock<ApplicationDbContext>();
            context.Setup(c => c.People.ToList()).Returns(new List<Person>
            {
                new Person() { Id = 1, Firstname = "Torsten", Lastname = "Torstensson", Email = "torsten@gmail.com", City = "Berlin", DateCreated = DateTime.Now},
                new Person() { Id = 2, Firstname = "Uno", Lastname = "Unosson", Email = "uno@gmail.com", City = "Amsterdam", DateCreated = DateTime.Now}
            });

            var controller = new PeopleController(context.Object, new Mapper());
        }
    }
}
