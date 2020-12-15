using System;
using TimeNotes.Domain;

namespace TimeNotas.App.Models
{
    public struct HomeDashboardViewModel
    {
        public string MonthName { get; set; }
        public string MonthNameWithYear { get; private set; }
        public TimeSpan TotalExtraTimeInMonth { get; private set; }
        public TimeSpan TotalMissingTimeInMonth { get; private set; }
        public TimeSpan TotalWorkedTimeInMonth { get; private set; }
        public decimal SalaryInMonth { get; private set; }

        public HomeDashboardViewModel(MonthExtract monthExtract)
        {
            MonthName = new DateTime(monthExtract.Date.Year, monthExtract.Date.Month, 1).ToString("MMMM");
            MonthNameWithYear = $"{MonthName}/{monthExtract.Date.Year}";
            TotalExtraTimeInMonth = monthExtract.ExtraTime;
            TotalMissingTimeInMonth = monthExtract.MissingTime;
            TotalWorkedTimeInMonth = monthExtract.WorkedTime;
            SalaryInMonth = monthExtract.PredictedSalary;
        }
    }
}
