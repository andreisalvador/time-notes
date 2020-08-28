using FluentValidation;
using System;

namespace TimeNotes.Domain.Validators
{
    public class TimeEntryValidator : AbstractValidator<TimeEntry>
    {
        public TimeEntryValidator()
        {
            RuleFor(t => t.DateHourPointed)
                .NotEqual(DateTime.MinValue)
                .WithMessage("You must to inform the hour.");
        }
    }
}
