namespace GPConnect.Provider.AcceptanceTests.Helpers
{
    using System;
    using Hl7.Fhir.Model;

    public static class TimePeriodHelper
    {
        private static FhirDateTime DefaultStartDate => new FhirDateTime(DateTime.UtcNow.AddYears(-2).Year);
        private static FhirDateTime DefaultEndDate => new FhirDateTime(DateTime.UtcNow.Year);
        private static FhirDateTime InvalidDate => new FhirDateTime("abcd");

        public static Period GetTimePeriod(string startDate, string endDate)
        {
            return new Period(new FhirDateTime(startDate), new FhirDateTime(endDate));
        }

        public static Period GetDefaultTimePeriod()
        {
            return new Period(DefaultStartDate, DefaultEndDate);
        }

        public static Period GetTimePeriodInvalidStartDate()
        {
            return new Period(InvalidDate, DefaultEndDate);
        }

        public static Period GetTimePeriodInvalidEndDate()
        {
            return new Period(DefaultStartDate, InvalidDate);
        }

        public static Period GetTimePeriodStartDateAfterEndDate()
        {
            return new Period(DefaultEndDate, DefaultStartDate);
        }

        public static Period GetTimePeriodStartDateOnly()
        {
            return new Period(DefaultStartDate, null);
        }

        public static Period GetTimePeriodEndDateOnly()
        {
            return new Period(null, DefaultEndDate);
        }

        public static Period GetTimePeriodStartDateTodayEndDateDays(int days)
        {
            var date = DateTime.UtcNow.Date;
            return new Period(new FhirDateTime(date), new FhirDateTime(date.AddDays(days)));
        }
    }
}
