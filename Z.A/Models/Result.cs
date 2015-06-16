using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Z.A.Models
{
    public class Result
    {
        public static Result Create(bool ok, string message) {
            Result result = new Result();
            result.Ok = ok;
            result.Message = message;
            return result;
        }
        public bool Ok;
        public string Message;
    }
}