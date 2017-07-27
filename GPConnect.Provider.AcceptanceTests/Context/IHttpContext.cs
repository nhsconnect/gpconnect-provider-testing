namespace GPConnect.Provider.AcceptanceTests.Context
{
    using Helpers;

    public interface IHttpContext
    {
        JwtHelper Jwt { get; }
        // Security Context
        SecurityContext SecurityContext { get; }

    }
}