using System;
using System.Collections.Generic;
using System.Text;

namespace PagarMe.Model
{
    internal class BulkAnticipationLimit : Base.Model
    {
        protected override string Endpoint { get { return "/limits"; } }

        public BulkAnticipationLimit() : base(null) { }

        public BulkAnticipationLimit(PagarMeService service) : base(service) { }

        public BulkAnticipationLimit(PagarMeService service, string endpointPrefix) : base(service, endpointPrefix) { }
        
        public Limit Maximum
        {
            get { return GetAttribute<Limit>("maximum"); }
            set { SetAttribute("maximum", value); }
        }

        public Limit Minimum
        {
            get { return GetAttribute<Limit>("minimum"); }
            set { SetAttribute("minimum", value); }
        }

        public TimeFrame TimeFrame
        {
            get { return GetAttribute<TimeFrame>("timeframe"); }
            set { SetAttribute("timeframe", value); }
        }

        public string PaymentDate
        {
            get { return GetAttribute<string>("payment_date"); }
            set { SetAttribute("payment_date", value); }
        }
    }
}
