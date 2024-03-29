﻿namespace Cblx.Blocks.Json.Tests.FlattenRootAttributeUsageWithFluent;
[FlattenJsonRoot<PersonConfiguration>]
public class Person
{
    public required string Name { get; set; }
    [FlattenJsonProperty]
    public required Address Address { get; set; }
}
