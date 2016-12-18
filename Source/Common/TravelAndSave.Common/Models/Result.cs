namespace TravelAndSave.Common.Models
{
    using System;
    using System.Linq;

    public class Result
    {
        protected const string FailedResultArgumentExceptionMessage = "The input should contain at least one failed result.";

        public string Message { get; set; }

        public bool IsSuccess { get; }

        public string ErrorMessage { get; }

        public int? StatusCode { get; set; }

        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, string error)
        {
            IsSuccess = isSuccess;
            ErrorMessage = error;
        }

        protected Result(bool isSuccess, string error, int? statusCode)
            : this(isSuccess, error)
        {
            this.StatusCode = statusCode;
        }

        public static Result Fail(string message)
        {
            return new Result(false, message);
        }

        public static Result Fail(string message, int? statusCode)
        {
            return new Result(false, message, statusCode);
        }

        public static Result<T> Fail<T>(string message)
        {
            return new Result<T>(default(T), false, message);
        }

        public static Result<T> Fail<T>(string message, int? statusCode)
        {
            return new Result<T>(default(T), false, message, statusCode);
        }

        public static Result Ok()
        {
            return new Result(true, string.Empty);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, string.Empty);
        }

        public static Result Fail(params Result[] results)
        {
            var failedResults = results.Where(t => t.IsFailure);
            if (!failedResults.Any())
            {
                throw new ArgumentException(FailedResultArgumentExceptionMessage);
            }

            var errorMessage = failedResults
                .Select(t => t.ErrorMessage)
                .Aggregate((current, next) => current + Environment.NewLine + next);

            return new Result(false, errorMessage);
        }

        public static Result<T> Fail<T>(params Result[] results)
        {
            var failedResults = results.Where(t => t.IsFailure);
            if (!failedResults.Any())
            {
                throw new ArgumentException(FailedResultArgumentExceptionMessage);
            }

            var errorMessage = failedResults
                .Select(t => t.ErrorMessage)
                .Aggregate((current, next) => current + Environment.NewLine + next);

            var statusCode = failedResults.FirstOrDefault().StatusCode;

            return new Result<T>(default(T), false, errorMessage, statusCode);
        }
    }

    public class Result<T> : Result
    {
        public T Value { get; }

        protected internal Result(T value, bool isSuccess, string error)
            : base(isSuccess, error)
        {
            Value = value;
        }

        protected internal Result(T value, bool isSuccess, string error, int? statusCode)
            : base(isSuccess, error, statusCode)
        {
            Value = value;
        }
    }
}
