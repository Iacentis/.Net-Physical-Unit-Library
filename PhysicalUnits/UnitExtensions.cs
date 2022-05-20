namespace PhysicalUnits
{
    public static class UnitExtensions
    {
        public static double Convert(this double value, Unit from, Unit to)
        {
            if (from is null)
            {
                throw new ArgumentNullException(nameof(from));
            }

            if (to is null)
            {
                throw new ArgumentNullException(nameof(to));
            }

            if (!from.Compatible(to))
            {
                throw new ArgumentException("Units must be compatible to perform addition");
            }

            return value * from.ScaleFactor / to.ScaleFactor;
        }
        public static int Convert(this int value, Unit from, Unit to)
        {
            if (from is null)
            {
                throw new ArgumentNullException(nameof(from));
            }

            if (to is null)
            {
                throw new ArgumentNullException(nameof(to));
            }

            if (!from.Compatible(to))
            {
                throw new ArgumentException("Units must be compatible to perform addition");
            }

            return (int)(value * from.ScaleFactor / to.ScaleFactor);
        }
        /// <summary>
        /// Handles odd characters that are not part of the provided standard format and return the prepared list
        /// </summary>
        /// <param name="source">The source string</param>
        /// <param name="Components">A split string with handled characters</param>
        /// <param name="sign">a Boolean array indicating whether the particular component is behind a division sign or not, same size as Components</param>
        internal static void HandleSpecialSigns(string source, out string[] Components, out bool[] sign)
        {
            string? tmp = source.Replace('/', ' ');
            tmp = tmp.Replace('�', '2'); //Handle bad Unicode from DEWEsoft
            Components = tmp.Split(' ');

            //Prepare an array for handling '/' signs
            sign = new bool[Components.Length];
            int count = 0;
            int loc = source.IndexOf('/', StringComparison.InvariantCulture);
            for (int i = 0; i < Components.Length; i++)
            {
                count += 1 + Components[i].Length;
                if (loc != -1)
                {

                    sign[i] = count < loc;
                }
                else
                {
                    sign[i] = true;
                }
            }
        }

        /// <summary>
        /// Tries to read an SI unit from the given string, and computes it with the proper exponent into the result
        /// </summary>
        internal static void ReadSIUnit(ref Unit result, int exponent, string PreexponentString)
        {
            int MetricPrefixExponent = 0;
            int SIUnitIndex;
            switch (PreexponentString)
            {
                case "cd":
                    SIUnitIndex = (int)SI.candela;
                    break;
                case "mol":
                    SIUnitIndex = (int)SI.mole;
                    break;
                default:
                    //If no prefix is available, just get the SI
                    SIUnitIndex = (int)UnitSymbolLibrary.FromSISymbol(PreexponentString);
                    //Else, get prefix and SI.
                    if (SIUnitIndex == (int)SI.none)
                    {
                        MetricPrefixExponent = (int)UnitSymbolLibrary.FromPrefixSymbol(PreexponentString[0]);
                        SIUnitIndex = (int)UnitSymbolLibrary.FromSISymbol(PreexponentString.Length > 1 ? PreexponentString[1..] : PreexponentString);
                    }
                    break;
            }
            if (SIUnitIndex != (int)SI.none)
            {
                result.SizeExponent[SIUnitIndex] = MetricPrefixExponent;
                result.SIExponents[SIUnitIndex] = exponent;
                result.ScaleFactor *= Math.Pow(10, exponent * MetricPrefixExponent);
            }
        }
        /// <summary>
        /// Checks to see if a Composite unit (such as force or pressure) can be found in the string, and in that case computes it with the proper exponent and adds it to the result
        /// </summary>
        internal static bool TryGetCompositeUnit(ref Unit result, int exponent, string PreexponentString)
        {
            if (Units.BySymbol.TryGetValue(PreexponentString, out Unit? composite))
            {
                for (int j = 0; j < exponent; j++)
                {
                    result *= composite;
                }
                return true;
            }
            if (PreexponentString.Length > 1)
            {
                if (Units.BySymbol.TryGetValue(PreexponentString[1..], out Unit? subcomposite))
                {

                    MetricPrefixExponents sizeExp = UnitSymbolLibrary.FromPrefixSymbol(PreexponentString[0]);
                    for (int j = 0; j < exponent; j++)
                    {
                        result *= subcomposite;
                    }
                    result *= sizeExp;
                    return true;
                }

            }
            return false;
        }

        /// <summary>
        /// Splits a given string into exponent and non-exponent part
        /// </summary>
        /// <param name="Component">The string to split</param>
        /// <param name="sign">Whether or not this string is behind a divison '/' sign</param>
        /// <param name="PreexponentString">The content of the string before exponentiation was reached</param>
        /// <returns></returns>
        internal static int GetExponent(string Component, bool sign, out string PreexponentString)
        {
            PreexponentString = string.Empty;
            int exponent = 1;
            bool foundExp = false;
            for (int j = 0; j < Component.Length; j++)
            {
                char c = Component[j];
                if (!foundExp)
                {
                    if (UnitSymbolLibrary.SuperScriptMinus == c)
                    {
                        foundExp = true;
                    }

                    if (UnitSymbolLibrary.SuperscriptDigits.Any(x => x == c))
                    {
                        foundExp = true;
                    }

                    if (UnitSymbolLibrary.NumericDigits.Any(x => x == c))
                    {
                        foundExp = true;
                    }

                    if (UnitSymbolLibrary.ExponentDigits.Any(x => x == c))
                    {
                        foundExp = true;
                        continue;
                    }
                }
                if (foundExp)
                {
                    string rest = Component[j..];
                    try
                    {
                        exponent = rest.FromSuperScript();
                    }
                    catch (ArgumentException)
                    {
                        if (!int.TryParse(rest, out exponent))
                        {
                            exponent = 1;
                        }

                    }
                    exponent = sign ? exponent : -exponent;
                    break;
                }
                else
                {
                    PreexponentString += c;
                }
            }
            return exponent;
        }
    }
}
