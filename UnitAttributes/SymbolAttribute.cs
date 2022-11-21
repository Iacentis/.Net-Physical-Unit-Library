using System;

namespace PhysicalUnits
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class UnitSymbolAttribute : Attribute
    {
        public string Value { get; private set; }
        public UnitSymbolAttribute(string value)
        {
            Value = value;
        }
    }
}