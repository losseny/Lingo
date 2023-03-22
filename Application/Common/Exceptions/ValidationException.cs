using FluentValidation.Results;

namespace Application.Common.Exceptions;

public class ValidationException : Exception
{
    private ValidationException() : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures, bool hasLanguage = false) : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => hasLanguage ? e.ErrorMessage : e.ErrorCode)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public IDictionary<string, string[]> Errors { get; }
}