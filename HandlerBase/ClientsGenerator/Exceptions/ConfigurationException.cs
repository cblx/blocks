using System;

namespace Cblx.Blocks.Exceptions;

public class ConfigurationException : Exception
{
    public ConfigurationException(string message) : base(message) { }
    public ConfigurationException(string message, Exception innerExcepion) : base(message, innerExcepion) { }
}
