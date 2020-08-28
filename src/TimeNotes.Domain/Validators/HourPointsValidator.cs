using FluentValidation;
using System;

namespace TimeNotes.Domain.Validators
{
    public class HourPointsValidator : AbstractValidator<HourPoints>
    {
        public HourPointsValidator()
        {
            RuleFor(h => h.Date)
                .NotEqual(DateTime.MinValue)
                .WithMessage("You must to inform the date.");

            RuleFor(h => h.UserId)
                .NotEqual(Guid.Empty)
                .WithMessage("You must to inform the user.");
        }
    }
}
