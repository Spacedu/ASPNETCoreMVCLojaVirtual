using System;
using System.Text;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;

namespace PagarMe
{
	public class Utils {

		public static string CalculateRequestHash(HMac mac, string data) {
			return CalculateRequestHash(mac, Encoding.UTF8.GetBytes(data));
		}

		public static string CalculateRequestHash(HMac mac, byte[] data) {
			mac.Init(new KeyParameter(Encoding.UTF8.GetBytes(PagarMeService.DefaultApiKey)));
			mac.BlockUpdate(data, 0, data.Length);
			byte[] result = new byte[mac.GetMacSize()];
			mac.DoFinal(result, 0);
			string hex = BitConverter.ToString(result).Replace("-", "").ToLower();
			return hex;
		}

		public static HMac HMacBuilder(string data) {
			switch (data) {
				case "md5":
					return new HMac( new Org.BouncyCastle.Crypto.Digests.MD5Digest() );
				case "sha1":
					return new HMac( new Org.BouncyCastle.Crypto.Digests.Sha1Digest() );
				case "sha224":
					return new HMac( new Org.BouncyCastle.Crypto.Digests.Sha224Digest() );
				case "sha256":
					return new HMac( new Org.BouncyCastle.Crypto.Digests.Sha256Digest() );
				case "sha384":
					return new HMac( new Org.BouncyCastle.Crypto.Digests.Sha384Digest() );
				case "sha512":
					return new HMac( new Org.BouncyCastle.Crypto.Digests.Sha512Digest() );
				default:
					throw new Exception("Invalid HMac Digest Algorithm");
			}
		}

		public static bool ValidateRequestSignature(string data, string signature) {
			return ValidateRequestSignature(Encoding.UTF8.GetBytes(data), signature);
		}

		public static bool ValidateRequestSignature(byte[] data, string signature) {
			string[] parts = signature.Split(new[] { '=' }, 2);
			if (parts.Length == 2) {
				try {
					HMac mac = HMacBuilder(parts[0]);
					return CalculateRequestHash(mac, data) == parts[1];
				} catch (Exception) {
					return false;
				}
			}

            return false;
		}

        public static Int64 ConvertToUnixTimeStamp(DateTime date)
        {
            Int64 unixTimestamp = (Int64)(date.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
            return unixTimestamp;
        }

        public static DateTime ConvertToDateTime(Int64 unixTimestamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimestamp).ToLocalTime();
            return dtDateTime;
        }
	}
}

