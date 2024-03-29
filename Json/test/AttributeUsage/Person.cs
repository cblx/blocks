﻿using System.Text.Json.Serialization;

namespace Cblx.Blocks.Json.Tests.AttributeUsage;

[JsonConverter(typeof(FlattenJsonConverter<Person>))]
public class Person
{
    public required string Name { get; set; }
    [FlattenJsonProperty]
    public required Address Address { get; set; }
}
