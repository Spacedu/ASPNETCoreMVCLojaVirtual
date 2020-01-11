using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace PagarMe.Tests
{
    [TestFixture]
    public class PagarMeProviderTests : PagarMeTestFixture
    {
        [Test]
        public void ValidateApiKey()
        {
            Assert.Throws<ArgumentException>(() => new PagarMeProvider("_invalid_key_", ""));
            Assert.Throws<ArgumentException>(() => new PagarMeProvider("ak_test_valid_key", "_invalid_key_"));
        }

        [Test]
        public void ValidateTransaction()
        {
            Assert.Throws<FormatException>(() => PagarMeProvider.ValidateTransaction(new TransactionSetup
            {
                PaymentMethod = PaymentMethod.CreditCard,
                CardHash = ""
            }));

            Assert.Throws<FormatException>(() => PagarMeProvider.ValidateTransaction(new TransactionSetup
            {
                PaymentMethod = PaymentMethod.Boleto,
                Amount = -10m
            }));
        }

        [Test]
        public void ValidateSubscription()
        {
            Assert.Throws<FormatException>(() => PagarMeProvider.ValidateSubscription(new SubscriptionSetup
            {
                PaymentMethod = PaymentMethod.CreditCard,
                CardHash = "__hash__",
                Plan = -1
            }));
        }
    }
}
