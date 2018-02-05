namespace GPConnect.Provider.AcceptanceTests.Extensions
{
    using System;
    using Shouldly;

    internal static class EnumExtensions
    {
        internal static void ShouldBeDefinedIn(this Enum actual, Type comparator, string message)
        {
            Enum.IsDefined(comparator, actual)
                .ShouldBeTrue(message);
        }
    }
}
