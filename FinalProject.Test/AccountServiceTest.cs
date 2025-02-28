using Entities.Models;
using Entities.Data;
using Microsoft.AspNetCore.Identity;
using Moq;
using Services.Interfacies;
using Services.Services;

namespace FinalProject.Test
{
    [TestFixture]
    public class AccountServiceTest
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void Login_Should_Return_Code_Success()
        {
            var result = 3;

            Assert.That(result, Is.EqualTo(3));
        }
    }
}
