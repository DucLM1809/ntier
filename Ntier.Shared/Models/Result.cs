using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ntier.Shared.Models
{
    public class Result
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }
        public object? Data { get; private set; }
        public int StatusCode { get; private set; }

        private Result(bool success, string message, object? data = null, int statusCode = 200)
        {
            Success = success;
            Message = message;
            Data = data;
            StatusCode = statusCode;
        }

        public static Result SuccessResult(string message, object? data = null, int statusCode = 200)
        {
            return new Result(true, message, data, statusCode);
        }

        public static Result FailureResult(string message, object? data = null, int statusCode = 400)
        {
            return new Result(false, message, data, statusCode);
        }
    }
}