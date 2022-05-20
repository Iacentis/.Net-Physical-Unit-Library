using System.Reflection;
namespace PhysicalUnits
{
    public static class Units
    {
        static unsafe Units()
        {
            Type t = typeof(Units);
            IEnumerable<PropertyInfo>? propInfos = t.GetProperties().Where(prop => prop.IsDefined(typeof(SymbolAttribute), false));
            foreach (PropertyInfo propInfo in propInfos)
            {
                if (propInfo.GetCustomAttributes(typeof(SymbolAttribute), false).FirstOrDefault() is not SymbolAttribute attr)
                {
                    continue;
                }

                if (attr.Value is null)
                {
                    continue;
                }

                if (propInfo.GetValue(null, null) is not Unit a)
                {
                    continue;
                }

                BySymbol.Add(attr.Value, a);
            }
        }
        /// <summary>
        /// Standard units easily accessible
        /// </summary>
        public static Unit None => new(0);
        public static Unit Seconds => new(1);
        public static Unit Metre => new(0, 1);
        public static Unit Kilogram => 'k' * new Unit(0, 0, 1);
        public static Unit Ampere => new(0, 0, 0, 1);
        public static Unit Kelvin => new(0, 0, 0, 0, 1);
        public static Unit Mole => new(0, 0, 0, 0, 0, 1);
        public static Unit Candela => new(0, 0, 0, 0, 0, 0, 1);
        //Derived from SI units

        [Symbol("m^2")]
        public static Unit Area => Metre * Metre;
        [Symbol("m^3")]
        public static Unit Volume => Metre ^ 3;
        [Symbol("m/s")]
        public static Unit Velocity => Metre / Seconds;
        [Symbol("m/s^2")]
        public static Unit Acceleration => Metre / (Seconds ^ 2);
        [Symbol("\u0929")]
        public static Unit Density => Kilogram / Volume;
        [Symbol("N")]
        public static Unit Newton => Kilogram * Acceleration;
        [Symbol("Pa")]
        public static Unit Pascal => Newton / Area;
        [Symbol("\u0957")]
        public static Unit Viscosity => Pascal * Seconds;
        [Symbol("Hz")]
        public static Unit Hertz => None / Seconds;
        [Symbol("J")]
        public static Unit Joule => Newton * Metre;
        [Symbol("W")]
        public static Unit Watt => Joule / Seconds;
        [Symbol("C")]
        public static Unit Coulomb => Seconds * Ampere;
        [Symbol("V")]
        public static Unit Volt => Watt / Ampere;
        [Symbol("F")]
        public static Unit Farad => Coulomb / Volt;
        [Symbol("\u0937")]
        public static Unit Ohm => Volt / Ampere;
        [Symbol("S")]
        public static Unit Siemens => None / Ohm;
        [Symbol("Mx")]
        public static Unit Weber => Joule / Ampere;
        [Symbol("T")]
        public static Unit Tesla => Volt * Seconds / Area;
        [Symbol("H")]
        public static Unit Henry => Volt * Seconds / Ampere;
        [Symbol("lm")]
        public static Unit Lumen => Candela;
        [Symbol("lx")]
        public static Unit Lux => Candela / Area;
        [Symbol("Gy")]
        public static Unit Gray => Joule / Kilogram;
        [Symbol("Sv")]
        public static Unit Sievert => Gray;
        [Symbol("kat")]
        public static Unit Katal => Mole / Seconds;

        public static Dictionary<string, Unit> BySymbol { get; } = new();

    }
}
