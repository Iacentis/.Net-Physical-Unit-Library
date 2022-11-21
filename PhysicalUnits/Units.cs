using System.Reflection;
namespace PhysicalUnits
{
    public static partial class Units
    {
        static Units()
        {
            FillDictionary();
        }
        private static partial void FillDictionary();
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

        [UnitSymbol("m^2")]
        public static Unit Area => Metre * Metre;
        [UnitSymbol("m^3")]
        public static Unit Volume => Metre ^ 3;
        [UnitSymbol("m/s")]
        public static Unit Velocity => Metre / Seconds;
        [UnitSymbol("m/s^2")]
        public static Unit Acceleration => Metre / (Seconds ^ 2);
        [UnitSymbol("\u0929")]
        public static Unit Density => Kilogram / Volume;
        [UnitSymbol("N")]
        public static Unit Newton => Kilogram * Acceleration;
        [UnitSymbol("Pa")]
        public static Unit Pascal => Newton / Area;
        [UnitSymbol("\u0957")]
        public static Unit Viscosity => Pascal * Seconds;
        [UnitSymbol("Hz")]
        public static Unit Hertz => None / Seconds;
        [UnitSymbol("J")]
        public static Unit Joule => Newton * Metre;
        [UnitSymbol("W")]
        public static Unit Watt => Joule / Seconds;
        [UnitSymbol("C")]
        public static Unit Coulomb => Seconds * Ampere;
        [UnitSymbol("V")]
        public static Unit Volt => Watt / Ampere;
        [UnitSymbol("F")]
        public static Unit Farad => Coulomb / Volt;
        [UnitSymbol("\u0937")]
        public static Unit Ohm => Volt / Ampere;
        [UnitSymbol("S")]
        public static Unit Siemens => None / Ohm;
        [UnitSymbol("Mx")]
        public static Unit Weber => Joule / Ampere;
        [UnitSymbol("T")]
        public static Unit Tesla => Volt * Seconds / Area;
        [UnitSymbol("H")]
        public static Unit Henry => Volt * Seconds / Ampere;
        [UnitSymbol("lm")]
        public static Unit Lumen => Candela;
        [UnitSymbol("lx")]
        public static Unit Lux => Candela / Area;
        [UnitSymbol("Gy")]
        public static Unit Gray => Joule / Kilogram;
        [UnitSymbol("Sv")]
        public static Unit Sievert => Gray;
        [UnitSymbol("kat")]
        public static Unit Katal => Mole / Seconds;

        public static Dictionary<string, Unit> BySymbol { get; } = new();

    }
}
