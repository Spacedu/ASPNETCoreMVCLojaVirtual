using System;
using System.Collections.Generic;
using System.Text;

namespace PagarMe.Model
{
    public class Limit : Base.Model
    {
        protected override string Endpoint { get { return "/limits"; } }

        public Limit() : base(null) { }

        public Limit(PagarMeService service) : base(service) { }

        public Limit(PagarMeService service, string endpointPrefix = "") : base(service, endpointPrefix) { }

        public int Amount
        {
            get { return GetAttribute<int>("amount"); }
            set { SetAttribute("amount", value); }
        }

        public int Fee
        {
            get { return GetAttribute<int>("fee"); }
            set { SetAttribute("fee", value); }
        }

        public int AnticipationFee
        {
            get { return GetAttribute<int>("anticipation_fee"); }
            set { SetAttribute("anticipation_fee", value); }
        }
    }
}
