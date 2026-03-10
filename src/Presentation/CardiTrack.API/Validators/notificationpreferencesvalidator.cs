using CardiTrack.Application.DTOs.Requests;
using FluentValidation;

namespace CardiTrack.API.Validators;

public class NotificationPreferencesValidator : AbstractValidator<NotificationPreferencesRequest>
{
    public NotificationPreferencesValidator()
    {
        RuleFor(x => x.CardiMemberId)
            .NotEmpty().WithMessage("CardiMember ID is required");

        RuleFor(x => x.QuietHoursEnd)
            .Must((model, end) => BeValidQuietHours(model.QuietHoursStart, end))
            .WithMessage("Quiet hours end must be after quiet hours start")
            .When(x => x.QuietHoursStart.HasValue && x.QuietHoursEnd.HasValue);
    }

    private bool BeValidQuietHours(TimeOnly? start, TimeOnly? end)
    {
        if (!start.HasValue || !end.HasValue)
            return true;

        return end.Value > start.Value;
    }
}
