﻿namespace Cblx.Blocks.Json.Tests.Nested;

public class Address
{
    public required string Street { get; set; }
    [Flatten]
    public required City City { get; set; }
}
