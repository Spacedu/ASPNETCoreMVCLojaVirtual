//
// CardHash.cs
//
// Author:
//       Jonathan Lima <jonathan@pagar.me>
//
// Copyright (c) 2014 jonathanlima
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.OpenSsl;
using Newtonsoft.Json;

#if HAS_ASYNC
using System.Threading.Tasks;
#endif

namespace PagarMe
{
    public class CardHash
    {
        private class CardHashKey
        {
            [JsonProperty("id")]
            public int Id { get; private set; }

            [JsonProperty("public_key")]
            public string PublicKey { get; private set; }
        }

        private PagarMeService _service;

        public string CardNumber { get; set; }
        public string CardExpirationDate { get; set; }
        public string CardHolderName { get; set; }
        public string CardCvv { get; set; }

        public CardHash()
            : this(null)
        {

        }

        public CardHash(PagarMeService service)
        {
            if (service == null)
                service = PagarMeService.GetDefaultService();

            _service = service;
        }

        public string Generate()
        {
            var request = new PagarMeRequest(_service, "GET", "/transactions/card_hash_key");

            request.UseEncryptionKey = true;

            var response = request.Execute();
            var key = JsonHelper.Deserialize<CardHashKey>(_service, response.Body);

            return Encrypt(key);
        }

        #if HAS_ASYNC
        public async Task<string> GenerateAsync()
        {
            var request = new PagarMeRequest(_service, "GET", "/transactions/card_hash_key");

            request.UseEncryptionKey = true;

            var response = await request.ExecuteAsync();
            var key = JsonHelper.Deserialize<CardHashKey>(_service, response.Body);

            return Encrypt(key);
        }
        #endif

        private string Encrypt(CardHashKey key)
        {
            var args = new List<Tuple<string, string>>();

            args.Add(new Tuple<string, string>("card_number", CardNumber));
            args.Add(new Tuple<string, string>("card_expiration_date", CardExpirationDate));
            args.Add(new Tuple<string, string>("card_holder_name", CardHolderName));
            args.Add(new Tuple<string, string>("card_cvv", CardCvv));

            var encrypted = EncryptRsa(key.PublicKey, Encoding.UTF8.GetBytes(PagarMeRequest.BuildQueryString(args)));

            return key.Id + "_" + Convert.ToBase64String(encrypted);
        }

        private byte[] EncryptRsa(string publicKey, byte[] data)
        {
            using (var reader = new StringReader(publicKey))
            {
                var pemReader = new PemReader(reader);
                var key = (AsymmetricKeyParameter)pemReader.ReadObject();
                var rsa = new Pkcs1Encoding(new RsaEngine());

                rsa.Init(true, key);

                return rsa.ProcessBlock(data, 0, data.Length);
            }
        }
    }
}

