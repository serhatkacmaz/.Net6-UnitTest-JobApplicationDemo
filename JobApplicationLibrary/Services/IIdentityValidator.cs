namespace JobApplicationLibrary.Services
{
    public interface IIdentityValidator
    {
        bool IsValid(string identityNumber);

        //bool CheckConnectionToRemoteServer();

        ICountryDataProvider CountryDataProvider { get; }
    }

    public interface ICountryData
    {
        string Country { get; }
    }

    public interface ICountryDataProvider
    {
        ICountryData CountryData { get; }
    }
}