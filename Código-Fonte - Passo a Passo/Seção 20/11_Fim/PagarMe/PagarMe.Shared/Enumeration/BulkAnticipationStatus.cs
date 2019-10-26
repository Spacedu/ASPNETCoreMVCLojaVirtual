using System;
using System.Collections.Generic;
using System.Text;

namespace PagarMe.Enumeration
{
    public enum BulkAnticipationStatus
    {
        [Base.EnumValue("building")] Building,
        [Base.EnumValue("pending")] Pending,
        [Base.EnumValue("approved")] Approved,
        [Base.EnumValue("refused")] Refused,
        [Base.EnumValue("canceled")] Canceled
    }
}
