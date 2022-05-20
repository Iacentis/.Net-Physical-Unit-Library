namespace PhysicalUnits
{
    public static class UnitSymbolLibrary
    {
        public static string GetSymbol(this MetricPrefixExponents m)
        {
            return m switch
            {
                MetricPrefixExponents.nano => "n",
                MetricPrefixExponents.micro => "μ",
                MetricPrefixExponents.milli => "m",
                MetricPrefixExponents.kilo => "k",
                MetricPrefixExponents.mega => "M",
                MetricPrefixExponents.giga => "G",
                _ => string.Empty
            };
        }
        public static string GetSymbol(this SI unit)
        {
            return unit switch
            {
                SI.second => "s",
                SI.metre => "m",
                SI.gram => "g",
                SI.ampere => "A",
                SI.kelvin => "K",
                SI.mole => "mol",
                SI.candela => "cd",
                _ => string.Empty
            };
        }
        internal static SI FromSISymbol(string Symbol)
        {
            return Symbol switch
            {
                "s" => SI.second,
                "m" => SI.metre,
                "g" => SI.gram,
                "A" => SI.ampere,
                "K" => SI.kelvin,
                "mol" => SI.mole,
                "cd" => SI.candela,
                _ => SI.none
            };
        }
        internal static MetricPrefixExponents FromPrefixSymbol(char Symbol)
        {
            return Symbol switch
            {
                'n' => MetricPrefixExponents.nano,
                'μ' => MetricPrefixExponents.micro,
                'm' => MetricPrefixExponents.milli,
                'k' => MetricPrefixExponents.kilo,
                'M' => MetricPrefixExponents.mega,
                'G' => MetricPrefixExponents.giga,
                _ => MetricPrefixExponents.none
            };
        }
        internal const string SuperscriptDigits =
            "\u2070\u00b9\u00b2\u00b3\u2074\u2075\u2076\u2077\u2078\u2079";
        internal const char SuperScriptMinus = (char)0x207B;
        internal const string ExponentDigits = "^Â";
        internal const string NumericDigits = "-0123456789";
        internal static string ToSuperScript(this int number)
        {
            if (number == 0 ||
                number == 1)
            {
                return string.Empty;
            }

            string Superscript = string.Empty;

            if (number < 0)
            {
                //Adds superscript minus
                Superscript = SuperScriptMinus.ToString();
                number *= -1;
            }


            Superscript += new string(number.ToString()
                                            .Select(x => SuperscriptDigits[x - '0'])
                                            .ToArray()
                                      );

            return Superscript;
        }
        internal static int FromSuperScript(this string superscript)
        {
            if (superscript.Length == 0 || superscript == string.Empty)
            {
                return 1;
            }
            //If first character is a superscript -, number is negative
            bool negative = superscript[0] == SuperScriptMinus;
            //Remove the first character from the rest of the conversion if it's negative
            if (negative)
            {
                superscript = superscript[1..];
            }

            int[] digits = new int[superscript.Length];
            //Read from SuperScript UniCodes
            for (int i = 0; i < superscript.Length; i++)
            {
                bool CouldConvertChar = false;
                for (int j = 0; j < SuperscriptDigits.Length; j++)
                {
                    if (superscript[i] == SuperscriptDigits[j])
                    {
                        CouldConvertChar = true;
                        digits[i] = j;
                    }
                }
                if (!CouldConvertChar)
                {
                    throw new ArgumentException($"Provided string \"{superscript}\" is not a valid superscript number");
                }
            }

            //Calculate actual value from digits
            int val = 0;
            int mul = 1;
            for (int i = digits.Length - 1; i >= 0; i--)
            {
                val += mul * digits[i];
                mul *= 10;
            }
            if (negative)
            {
                val = -val;
            }

            return val;


        }
    }
}
