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

            // Här fattar jag inte riktigt hur jag ska få det att funka. Som konstruktorn ser ut nu tar den en IMapper, men vet inte hur jag ska 
            // lösa det här. Borde gå att lösa på något annat sätt, genom att flytta ut IMappern ur konstruktorn och skapa via en property istället
            // men det blir en fullösning tänker jag.
            var controller = new PeopleController(context.Object, /*????????*/);
        }
    }
}
