namespace Taxually.TechnicalTest.Exceptions;

[Serializable]
public sealed class CountryNotSupportedException : Exception
{
    public CountryNotSupportedException(string country) : base($"Country '{country}' is not supported") { }
}
