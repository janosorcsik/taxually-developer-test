namespace Taxually.TechnicalTest.Exceptions;

[Serializable]
public sealed class CountryNotSupportedException : Exception
{
    public CountryNotSupportedException() : base("Country is not supported") { }
}
