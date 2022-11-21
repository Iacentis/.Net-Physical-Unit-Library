using System;

namespace PhysicalUnits
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SymbolAttribute : Attribute
    {
        public string Value { get; private set; }
        public SymbolAttribute(string value)
        {
            Value = value;
        }
    }
}