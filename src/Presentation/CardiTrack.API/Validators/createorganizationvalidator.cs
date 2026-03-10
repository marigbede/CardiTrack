using CardiTrack.Application.DTOs.Requests;
using FluentValidation;

namespace CardiTrack.API.Validators;

public class CreateOrganizationValidator : AbstractValidator<CreateOrganizationRequest>
{
    public CreateOrganizationValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Organization name is required")
            .Length(2, 100).WithMessage("Name must be between 2 and 100 characters")
            .Matches(@"^[a-zA-Z0-9\s\-']+$")
            .WithMessage("Name can only contain letters, numbers, spaces, hyphens, and apostrophes");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid organization type");
    }
}
