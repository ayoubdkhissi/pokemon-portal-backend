namespace Shared.Constants;
public static class ErrorCodes
{
    // General Errors
    public const string NotFound = "not_found";
    public const string ValidationFailed = "validation_failed";

    // Validation Errors
    public const string PropertyIsRequired = "property_is_required";
    public const string LengthExceeded = "length_exceeded";
    public const string NegativeValue = "negative_value";
}
