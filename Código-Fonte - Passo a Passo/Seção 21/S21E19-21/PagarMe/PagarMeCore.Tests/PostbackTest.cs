using System.Linq;
using NUnit.Framework;
using PagarMe;
using PagarMe.Enumeration;
using PagarMe.Model;

namespace PagarMeCore.Tests
{
    [TestFixture]
    class PostbackTest
    {
        private static Transaction transaction = PagarMeTestFixture.BoletoTransactionPaidWithPostbackURL();

        [Test]
        public void FindAllPostbacksTest()
        {
            var postbacks = transaction.Postbacks.FindAll(new Postback());

            foreach (var postback in postbacks)
            {
                Assert.IsTrue(postback.ModelId.Equals(transaction.Id));
            }
        }

        [Test]
        public void FindPostbackTest()
        {
            Postback postback = transaction.Postbacks.FindAll(new Postback()).FirstOrDefault();
            Postback postbackReturned = transaction.Postbacks.Find(postback.Id);

            Assert.IsNotNull(postbackReturned.Id);
        }

        [Test]
        public void RedeliverPostbackTest()
        {
            Postback postback = transaction.Postbacks.FindAll(new Postback()).FirstOrDefault();
            postback.Redeliver();

            Assert.IsTrue(postback.Status == PostbackStatus.PendingRetry);
        }
    }
}
