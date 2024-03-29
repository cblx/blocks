﻿using System.Text.Json.Serialization;

namespace Cblx.Blocks.Json.Tests.JsonIgnoreUsage;

public class Person
{
    public required string Name { get; set; }
    public required int Age { get; set; }
    [JsonIgnore]
    public required string InternalInfo { get; set; }

    public required string Description { get; set; }

    [FlattenJsonProperty]
    public required Address Address { get; set; }
}
