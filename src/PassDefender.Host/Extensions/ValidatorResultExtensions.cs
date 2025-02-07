using FluentValidation.Results;

namespace PassDefender.Host.Extensions;

public static class ValidatorResultExtensions
{
    public static Dictionary<string, string[]> CreateValidationProblem(this ValidationResult result)
    {
        var errorDictionary = new Dictionary<string, string[]>(1);

        foreach (var error in result.Errors)
        {
            string[] newDescriptions;

            if (errorDictionary.TryGetValue(error.ErrorCode, out var descriptions))
            {
                newDescriptions = new string[descriptions.Length + 1];
                Array.Copy(descriptions, newDescriptions, descriptions.Length);
                newDescriptions[descriptions.Length] = error.ErrorMessage;
            }
            else
            {
                newDescriptions = [error.ErrorCode];
            }

            errorDictionary[error.ErrorCode] = newDescriptions;
        }

        return errorDictionary;
    }
}