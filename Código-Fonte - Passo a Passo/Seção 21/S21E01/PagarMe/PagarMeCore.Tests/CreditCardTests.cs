using NUnit.Framework;

namespace PagarMeCore.Tests
{
    [TestFixture]
    public class CreditCardTests : PagarMeTestFixture
    {
        [Test]
        public void CreateHash()
        {
            Assert.That(GetCardHash(), Is.Not.Null.And.Not.Empty);
        }
    }
}
