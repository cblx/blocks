using System.Linq.Expressions;

namespace Cblx.Blocks.Json.Tests.FluentIgnoreWithRootConfiguration;

public class AddressVo
{
    public required string Street { get; set; }

    public required string? Ignored { get; set; }

    private string Address { get; set; }

    public static Expression<Func<AddressVo, object?>> AddressMember
        => (AddressVo e) => e.Address;

}