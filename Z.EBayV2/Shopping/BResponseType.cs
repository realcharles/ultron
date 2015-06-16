using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.EBayV2.Shopping
{
    public class BResponseType
    {
        public DateTime Timestamp;
        public string Ack;
        public string Build;
        public string Version;
        public ErrorType[] Errors;
    }
    public class ErrorType
    {
        public string ShortMessage;
        public string LongMessage;
        public string ErrorCode;
        public string SeverityCode;
        public string ErrorClassification;
    }
}
