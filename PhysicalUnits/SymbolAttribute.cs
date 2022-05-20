namespace PhysicalUnits
{
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class SymbolAttribute : Attribute
    {
        public string Value { get; private set; }
        public SymbolAttribute(string value)
        {
            Value = value;
        }
    }
}