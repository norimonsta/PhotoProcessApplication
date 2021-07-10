using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProcessApplication
{
    public struct Result
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public static Result WithError(string errorMessage)
        {
            return new Result { Message = errorMessage, Success = false };
        }
        public static Result WithSuccess(string message)
        {
            return new Result() { Message = message, Success = true };
        }
    }
}
