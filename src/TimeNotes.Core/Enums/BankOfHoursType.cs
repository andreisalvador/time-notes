using TimeNotes.Core.Attributes;

namespace TimeNotes.Core.Enums
{
    public enum BankOfHoursType
    {
        [EnumDescription(nameof(None), "0")]
        None = 0,
        [EnumDescription(nameof(Monthly), "1")]
        Monthly = 1,
        [EnumDescription(nameof(Quarterly), "2")]
        Quarterly = 2,
        [EnumDescription(nameof(Semiannual), "3")]
        Semiannual = 3,
        [EnumDescription(nameof(Yearly), "4")]
        Yearly = 4
    }
}
