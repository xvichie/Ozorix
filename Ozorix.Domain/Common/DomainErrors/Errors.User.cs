using ErrorOr;

namespace Ozorix.Domain.Common.DomainErrors;

public static partial class Errors
{
    public static class User
    {
        public static Error DuplicateEmail => Error.Conflict(
            code: "User.DuplicateEmail",
            description: "Email is already in use.");

        public static Error UserNotFoundInCache => Error.Failure(
            code: "User.UserNotFound",
            description: "No user with such Id has been found in cache memory.");
    }
}