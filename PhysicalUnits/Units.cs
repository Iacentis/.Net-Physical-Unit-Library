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

        [Symbol("m^2")]
        public static Unit Area => Metre * Metre;
        [Symbol("m^3")]
        public static Unit Volume => Metre ^ 3;
        [Symbol("m/s")]
        public static Unit Velocity => Metre / Seconds;
        [Symbol("m/s^2")]
        public static Unit Acceleration => Metre / (Seconds ^ 2);
        [Symbol("ρ")]
        public static Unit Density => Kilogram / Volume;
        [Symbol("N")]
        public static Unit Newton => Kilogram * Acceleration;
        [Symbol("Pa")]
        public static Unit Pascal => Newton / Area;
        [Symbol("μ")]
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
        [Symbol("Ω")]
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
