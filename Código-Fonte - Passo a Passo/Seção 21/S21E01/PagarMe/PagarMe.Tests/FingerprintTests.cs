using System;
using NUnit.Framework;
using PagarMe;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;

namespace PagarMe.Tests
{
	[TestFixture]
	public class FingerprintTests : PagarMeTestFixture
	{
		[Test]
		public void CheckFingerprint() {
			var expectedSignature = "fd29daad6c47ff78c1604395320b60bac87830cb";
			var expectedResult    = "sha1=" + expectedSignature;
			var bizareDigest      = "lol=" + expectedSignature;
			var inputData         = "{\"sample\":\"payload\",\"value\":true}";

			var hmac = new HMac( new Org.BouncyCastle.Crypto.Digests.Sha1Digest() );
			Assert.AreEqual( PagarMe.Utils.CalculateRequestHash(hmac, inputData), expectedSignature );

			Assert.IsTrue(  PagarMe.Utils.ValidateRequestSignature( inputData, expectedResult    ) );
			Assert.IsFalse( PagarMe.Utils.ValidateRequestSignature( inputData, bizareDigest      ) );
			Assert.IsFalse( PagarMe.Utils.ValidateRequestSignature( inputData, expectedSignature ) );
		}
	}
}
