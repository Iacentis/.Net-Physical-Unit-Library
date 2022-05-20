namespace PhysicalUnits
{
    internal class SymbolAttribute : Attribute
    {
        public string Value { get; set; }
        public SymbolAttribute(string value)
        {
            Value = value;
        }
    }
}