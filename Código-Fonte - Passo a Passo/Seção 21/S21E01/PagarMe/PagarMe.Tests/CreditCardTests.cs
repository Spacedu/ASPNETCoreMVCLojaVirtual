using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace PagarMe.Tests
{
    [TestFixture]
    public class CreditCardTests : PagarMeTestFixture
    {
        [Test]
        public void CreateHash()
        {
            Assert.IsNotNullOrEmpty(GetCardHash());
        }
    }
}
