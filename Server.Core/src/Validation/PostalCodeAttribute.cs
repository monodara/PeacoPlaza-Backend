using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class PostalCodeAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        string? stringValue = value as string;

        if (string.IsNullOrWhiteSpace(stringValue))
        {
            return ValidationResult.Success;
        }

        string postalCodePattern = @"^[1-9]\d{4}$"; //5 digits, not starting with 0

        bool isValid = Regex.IsMatch(stringValue, postalCodePattern);
        if (isValid)
        {
            return ValidationResult.Success;
        }
        else
        {
            return new ValidationResult("Not valid postcode");
        }
    }
}