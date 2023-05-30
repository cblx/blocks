namespace Cblx.Blocks.Json.Tests.FluentNested;

public class CityConfiguration : FlattenJsonConfiguration<City> 
{
    public CityConfiguration()
    {
        HasJsonPropertyName(a => a.CityName, "city");
        HasJsonPropertyName(a => a.Country, "country");
    }
}