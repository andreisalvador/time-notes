using FluentValidation;
using System;

namespace TimeNotes.Domain.Validators
{
    public class HourPointConfigurationsValidator : AbstractValidator<HourPointConfigurations>
    {
        public HourPointConfigurationsValidator()
        {
            RuleFor(h => h.LunchTime)
                .NotEqual(TimeSpan.MinValue)
                .WithMessage("You must to inform the lunch time.");

            RuleFor(h => h.OfficeHour)
                .NotEqual(TimeSpan.MinValue)
                .WithMessage("You must to inform the office hour.");

            RuleFor(h => h.StartWorkTime)
               .NotEqual(TimeSpan.MinValue)
               .WithMessage("You must to inform the start work time.");

            RuleFor(h => h.ToleranceTime)
                .NotEqual(TimeSpan.MinValue)
                .WithMessage("You must to inform the tolerance time.");

            RuleFor(h => h.WorkDays)
                .IsInEnum()
                .WithMessage("Days of work is not valid.");

            RuleFor(h => h.BankOfHours)
                .IsInEnum()
                .WithMessage("Bank of hours is not valid.");

            RuleFor(h => h.UserId)
                .NotEqual(Guid.Empty)
                .WithMessage("You must to inform the user.");

            RuleFor(h => h.HourValue)
                .GreaterThanOrEqualTo(0m);
        }
    }
}
