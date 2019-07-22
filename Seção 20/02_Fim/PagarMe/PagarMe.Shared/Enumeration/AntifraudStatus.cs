using System;
using System.Collections.Generic;
using System.Text;

namespace PagarMe.Enumeration
{
    public enum AntifraudStatus
    {
        [Base.EnumValue("approved")] Approved,
        [Base.EnumValue("failed")] Failed,
        [Base.EnumValue("processing")] Processing,
        [Base.EnumValue("deferred")] Deferred,
        [Base.EnumValue("refused")] Refused
    }
}
