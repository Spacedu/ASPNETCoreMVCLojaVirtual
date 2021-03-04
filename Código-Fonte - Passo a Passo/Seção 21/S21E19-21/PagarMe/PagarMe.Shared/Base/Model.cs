//
// Model.cs
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
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PagarMe.Base
{
    public abstract class Model : AbstractModel
    {
        protected readonly string endpointPrefix;

        public string Id
        {
            get
            {
                var result = GetAttribute<object>("id");

                if (result == null)
                    return null;

                return result.ToString();
            }
            set { SetAttribute("id", value); }
        }
    	public DateTime? DateCreated
    	{
    		get
    		{
    			var result = GetAttribute<DateTime?>("date_created");
    			if (result == null)
    				return null;
    			return result;
    		}
    	}
    	public DateTime? DateUpdated
    	{
    		get
    		{
    			var result = GetAttribute<DateTime?>("date_updated");
    			if (result == null)
    				return null;
    			return result;
    		}
    	}

        private Model() : this(null)
        {
        }

        protected Model(PagarMeService service, string endpointPrefix = "") : base(service)
        {
            this.endpointPrefix = endpointPrefix;
        }

        public void ExecuteSelfRequest(PagarMeRequest request)
        {
            LoadFrom(request.Execute().Body);
        }

        public async Task ExecuteSelfRequestAsync(PagarMeRequest request)
        {
            LoadFrom((await request.ExecuteAsync()).Body);
        }

        public void Refresh()
        {
            Refresh(Id);
        }

        internal void Refresh(string id)
        {
            if (id == null)
                throw new InvalidOperationException("Cannot refresh not existing object.");

            var request = CreateCollectionRequest("GET", "/" + id);
            var response = request.Execute();

            LoadFrom(response.Body);
        }

        #if HAS_ASYNC
        public async Task RefreshAsync()
        {
            await RefreshAsync(Id);
        }

        internal async Task RefreshAsync(string id)
        {
            if (id == null)
                throw new InvalidOperationException("Cannot refresh not existing object.");

            var request = CreateCollectionRequest("GET", "/" + id);
            var response = await request.ExecuteAsync();

            LoadFrom(response.Body);
        }
        #endif

        public void Save()
        {
            if (Id == null)
            {
                var request = CreateCollectionRequest("POST");

                request.Body = ToJson(SerializationType.Full);

                var response = request.Execute();

                LoadFrom(response.Body);
            }
            else
            {
                var request = CreateRequest("PUT");

                request.Body = ToJson(SerializationType.Shallow);

                var response = request.Execute();

                LoadFrom(response.Body);
            }
        }

        #if HAS_ASYNC
        public async Task SaveAsync()
        {
            if (Id == null)
            {
                var request = CreateCollectionRequest("POST");

                request.Body = ToJson(SerializationType.Full);

                var response = await request.ExecuteAsync();

                LoadFrom(response.Body);
            }
            else
            {
                var request = CreateRequest("PUT");

                request.Body = ToJson(SerializationType.Shallow);

                var response = await request.ExecuteAsync();

                LoadFrom(response.Body);
            }
        }
        #endif

        protected PagarMeRequest CreateRequest(string method, string endpoint = "")
        {
            return new PagarMeRequest(Service, method, endpointPrefix + Endpoint + "/" + Id + endpoint);
        }

        protected PagarMeRequest CreateCollectionRequest(string method, string endpoint = "")
        {
            return new PagarMeRequest(Service, method, endpointPrefix + Endpoint + endpoint);
        }

        internal void SetId(string id)
        {
            Id = id;
        }

        protected virtual bool CanSave()
        {
            return true;
        }

        protected abstract string Endpoint { get; }
    }
}

