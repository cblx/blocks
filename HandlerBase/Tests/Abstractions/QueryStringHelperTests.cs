namespace Cblx.Blocks.HandlerBase.Tests.Abstractions;

public class QueryStringHelperV2Tests
{
    [Fact]
    public void DeveConvertertGuidParaQueryStringQuandoExistir()
    {
        var obj = new
        {
            PropertyGuid = new Guid("f38515ef-04b7-4955-b302-9d29b582bba7")
        };

        var query = QueryStringHelper.ToQueryString(obj);
        query.Should().Be("PropertyGuid=f38515ef-04b7-4955-b302-9d29b582bba7");
    }
    
    [Fact]
    public void NaoDeveConvertertGuidParaQueryStringQuandoValorForNull()
    {
        var obj = new
        {
            PropertyGuid = (Guid?)null,
        };

        var query = QueryStringHelper.ToQueryString(obj);

        query.Should().BeEmpty();
    }
    
    [Fact]
    public void DeveConvertertBoolParaQueryStringQuandoExistir()
    {
        var obj = new
        {
            PropertyBool = true,
        };

        var query = QueryStringHelper.ToQueryString(obj);
        query.Should().Be("PropertyBool=true");
    }
    
    [Fact]
    public void NaoDeveConvertertBoolParaQueryStringQuandoValorForNull()
    {
        var obj = new
        {
            PropertyBool = (bool?)null,
        };

        var query = QueryStringHelper.ToQueryString(obj);

        query.Should().BeEmpty();
    }
    
    [Fact]
    public void DeveConvertertDateTimeParaQueryStringQuandoExistir()
    {
        var obj = new
        {
            PropertyDateTime = DateTime.Parse("2022-12-27T22:05:00")
        };

        var query = QueryStringHelper.ToQueryString(obj);
        query.Should().Be("PropertyDateTime=2022-12-27T22%3A05%3A00");
    }
    
    [Fact]
    public void NaoDeveConvertertDateTimeParaQueryStringQuandoValorForNull()
    {
        var obj = new
        {
            PropertyDateTime = (DateTime?)null,
        };

        var query = QueryStringHelper.ToQueryString(obj);

        query.Should().BeEmpty();
    }
    
    [Fact]
    public void DeveConvertertDateTimeOffsetParaQueryStringQuandoExistir()
    {
        var obj = new
        {
            PropertyDateTimeOffset = DateTimeOffset.Parse("2022-12-27T21:35:43.9029561-03:00")
        };

        var query = QueryStringHelper.ToQueryString(obj);
        query.Should().Be("PropertyDateTimeOffset=2022-12-27T21%3A35%3A43.9029561-03%3A00");
    }
    
    [Fact]
    public void NaoDeveConvertertDateTimeOffsetParaQueryStringQuandoValorForNull()
    {
        var obj = new
        {
            PropertyDateTimeOffset = (DateTimeOffset?)null,
        };

        var query = QueryStringHelper.ToQueryString(obj);

        query.Should().BeEmpty();
    }
    
    [Fact]
    public void DeveConvertertTimeOnlyParaQueryStringQuandoExistir()
    {
        var obj = new
        {
            PropertyTimeOnly = TimeOnly.Parse("21:34:44.2612089")
        };

        var query = QueryStringHelper.ToQueryString(obj);
        query.Should().Be("PropertyTimeOnly=21%3A34%3A44.2612089");
    }
    
    [Fact]
    public void NaoDeveConvertertTimeOnlyParaQueryStringQuandoValorForNull()
    {
        var obj = new
        {
            PropertyTimeOnly = (TimeOnly?)null
        };

        var query = QueryStringHelper.ToQueryString(obj);

        query.Should().BeEmpty();
    }
    
    [Fact]
    public void DeveConvertertDateOnlyParaQueryStringQuandoExistir()
    {
        var obj = new
        {
            PropertyDateOnly = DateOnly.Parse("2022-12-27"),
        };

        var query = QueryStringHelper.ToQueryString(obj);
        query.Should().Be("PropertyDateOnly=2022-12-27");
    }
    
    [Fact]
    public void NaoDeveConvertertDateOnlyParaQueryStringQuandoValorForNull()
    {
        var obj = new
        {
            PropertyDateOnly = (DateOnly?)null,
        };

        var query = QueryStringHelper.ToQueryString(obj);

        query.Should().BeEmpty();
    }
    
    [Fact]
    public void DeveConvertertDecimalParaQueryStringQuandoExistir()
    {
        var obj = new
        {
            PropertyDecimal = 123.23M
        };

        var query = QueryStringHelper.ToQueryString(obj);
        query.Should().Be("PropertyDecimal=123.23");
    }
    
    [Fact]
    public void NaoDeveConvertertDecimalParaQueryStringQuandoValorForNull()
    {
        var obj = new
        {
            PropertyDecimal = (decimal?)null
        };

        var query = QueryStringHelper.ToQueryString(obj);

        query.Should().BeEmpty();
    }
    
    [Fact]
    public void DeveConvertertIntParaQueryStringQuandoExistir()
    {
        var obj = new
        {
            PropertyInt = 123
        };

        var query = QueryStringHelper.ToQueryString(obj);
        query.Should().Be("PropertyInt=123");
    }
    
    [Fact]
    public void NaoDeveConvertertIntParaQueryStringQuandoValorForNull()
    {
        var obj = new
        {
            PropertyInt = (int?)null
        };

        var query = QueryStringHelper.ToQueryString(obj);

        query.Should().BeEmpty();
    }
    
    [Fact]
    public void DeveConvertertStringParaQueryStringQuandoExistir()
    {
        var obj = new
        {
            PropertyString = "Text Here",
        };

        var query = QueryStringHelper.ToQueryString(obj);
        query.Should().Be("PropertyString=Text%20Here");
    }
    
    [Fact]
    public void NaoDeveConvertertStringParaQueryStringQuandoValorForNull()
    {
        var obj = new
        {
            PropertyBool = (string?)null
        };

        var query = QueryStringHelper.ToQueryString(obj);

        query.Should().BeEmpty();
    }
    
    [Fact]
    public void DeveConvertertListOfIntParaQueryStringQuandoExistir()
    {
        var obj = new
        {
            PropertyListInt = new [] {1, 12, 15, 20}
        };

        var query = QueryStringHelper.ToQueryString(obj);
        
        query.Should().Be("PropertyListInt%5B0%5D=1&PropertyListInt%5B1%5D=12&PropertyListInt%5B2%5D=15&PropertyListInt%5B3%5D=20");
    }
    
    [Fact]
    public void NaoDeveConvertertListOfIntParaQueryStringQuandoValorForNull()
    {
        var obj = new
        {
            PropertyListInt = Array.Empty<int>()
        };

        var query = QueryStringHelper.ToQueryString(obj);

        query.Should().BeEmpty();
    }
    
    [Fact]
    public void DeveConvertertListOfEnumParaQueryStringQuandoExistir()
    {
        var obj = new
        {
            PropertyListEnum = new [] {TypeQuery.Dongo, TypeQuery.Read, TypeQuery.Ready}
        };

        var query = QueryStringHelper.ToQueryString(obj);
        
        query.Should().Be("PropertyListEnum%5B0%5D=3&PropertyListEnum%5B1%5D=2&PropertyListEnum%5B2%5D=1");
    }
    
    [Fact]
    public void NaoDeveConvertertListOfEnumParaQueryStringQuandoValorForNull()
    {
        var obj = new
        {
            PropertyListEnum = Array.Empty<TypeQuery>()
        };

        var query = QueryStringHelper.ToQueryString(obj);

        query.Should().BeEmpty();
    }
}
