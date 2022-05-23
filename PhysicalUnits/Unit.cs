using System.Text;

namespace PhysicalUnits
{
    public class Unit : IEquatable<Unit>
    {
        /// <summary>
        /// An array holding the exponents of the SI Units involved in the Unit
        /// </summary>
        internal int[] SIExponents = new int[7];
        /// <summary>
        /// An array holding the log10 exponents corresponding to the size of the Unit, such as 3 for kilo
        /// </summary>
        internal int[] SizeExponent = new int[7];
        /// <summary>
        /// The effective scale factor of the unit
        /// </summary>
        public double ScaleFactor { get; internal set; } = 1;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="SecondExponent"></param>
        /// <param name="MetreExponent"></param>
        /// <param name="GramExponent"></param>
        /// <param name="AmpereExponent"></param>
        /// <param name="KelvinExponent"></param>
        /// <param name="MoleExponent"></param>
        /// <param name="CandelaExponent"></param>
        public Unit(int SecondExponent = 0, int MetreExponent = 0, int GramExponent = 0, int AmpereExponent = 0, int KelvinExponent = 0, int MoleExponent = 0, int CandelaExponent = 0)
        {
            ScaleFactor = 1;
            SIExponents[0] = SecondExponent;
            SIExponents[1] = MetreExponent;
            SIExponents[2] = GramExponent;
            SIExponents[3] = AmpereExponent;
            SIExponents[4] = KelvinExponent;
            SIExponents[5] = MoleExponent;
            SIExponents[6] = CandelaExponent;
        }
        /// <summary>
        /// Implementation of IEquatable
        /// </summary>
        /// <param name="other">The Unit to check equality against</param>
        /// <returns>A bool indicating whether the Units are equal or not</returns>
        public bool Equals(Unit? other)
        {
            if (other is null)
            {
                return false;
            }

            if (!Compatible(other))
            {
                return false;
            }

            for (int i = 0; i < 7; i++)
            {
                if (SIExponents[i] != 0)
                {
                    if (SizeExponent[i] != other.SizeExponent[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Whether or not this Unit and another are compatible for addition/subtraction
        /// </summary>
        /// <param name="other">The Unit to compare to</param>
        /// <returns>A bool indicating whether addition/subtraction is valid</returns>
        public bool Compatible(Unit other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            for (int i = 0; i < 7; i++)
            {
                if (SIExponents[i] != other.SIExponents[i])
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Gets the total scale factor for the Unit
        /// </summary>
        public static Unit operator *(Unit left, Unit right)
        {
            if (left is null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right is null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            Unit result = new(0);
            for (int i = 0; i < 7; i++)
            {
                result.SizeExponent[i] = left.SizeExponent[i] + right.SizeExponent[i];
                result.SIExponents[i] = left.SIExponents[i] + right.SIExponents[i];
            }
            result.ScaleFactor = left.ScaleFactor * right.ScaleFactor;
            return result;
        }
        public static Unit operator /(Unit left, Unit right)
        {
            if (left is null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right is null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            Unit result = new(0);
            for (int i = 0; i < 7; i++)
            {
                result.SizeExponent[i] = left.SizeExponent[i] - right.SizeExponent[i];
                result.SIExponents[i] = left.SIExponents[i] - right.SIExponents[i];
            }
            result.ScaleFactor = left.ScaleFactor / right.ScaleFactor;
            return result;
        }
        public static Unit operator ^(Unit left, int right)
        {
            Unit result = new();
            if (right > 0)
            {
                for (int i = 0; i < right; i++)
                {
                    result *= left;
                }
            }
            if (right < 0)
            {
                for (int i = 0; i < -right; i++)
                {
                    result /= left;
                }
            }
            return result;
        }
        public static Unit operator +(Unit left, Unit right)
        {
            if (left is null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right is null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            if (!left.Equals(right))
            {
                throw new ArgumentException("Units must be equal to perform addition");
            }

            return left;
        }
        public static Unit operator *(Unit left, MetricPrefixExponents right)
        {
            Unit result = new Unit() * left;
            for (int i = 0; i < 7; i++)
            {
                if (result.SIExponents[i] == 0)
                {
                    continue;
                }

                result.SizeExponent[i] += (int)right;
                result.ScaleFactor *= Math.Pow(10, result.SIExponents[i] * (int)right);
            }
            return result;
        }
        public static Unit operator *(MetricPrefixExponents left, Unit right)
        {
            return right * left;
        }
        public static Unit operator *(Unit left, char right)
        {
            MetricPrefixExponents value = UnitSymbolLibrary.FromPrefixSymbol(right);
            if (value is not MetricPrefixExponents.none)
            {
                return left * value;
            }
            else
            {
                throw new ArgumentException($"No valid Metric Prefix conversion found for '{right}'");
            }
        }
        public static Unit operator *(char left, Unit right)
        {
            return right * left;
        }
        public static bool operator ==(Unit left, Unit right)
        {
            if (left is null)
            {
                return right is null;
            }
            return left.Equals(right);
        }

        public static bool operator !=(Unit left, Unit Right)
        {
            return !(left == Right);
        }
        public static bool operator ==(Unit left, string right)
        {
            if (left is null)
            {
                return false;
            }
            return left.Equals(Parse(right));
        }

        public static bool operator !=(Unit left, string right)
        {
            return !(left == right);
        }
        public static bool operator ==(string left, Unit right)
        {
            return right == left;
        }

        public static bool operator !=(string left, Unit right)
        {
            return right != left;
        }

        /// <summary>
        /// Convers the Unit down into string form with all the positive exponents first and all the negative exponents last.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder builder = new();
            for (int i = 0; i < 7; i++)
            {
                if (SIExponents[i] == 0)
                {
                    continue;
                }

                string exp = ((MetricPrefixExponents)SizeExponent[i]).GetSymbol();
                if (SIExponents[i] < 0)
                {
                    if (builder.Length > 0)
                    {
                        if (builder[^1] != ' ')
                        {
                            builder.Append(' ');
                        }
                    }

                    builder.Append(exp);
                    builder.Append(((SI)i).GetSymbol());
                    builder.Append(SIExponents[i].ToSuperScript());
                }
                else
                {
                    if (builder.Length > 0)
                    {
                        if (builder[0] != ' ')
                        {
                            builder.Insert(0, ' ');
                        }
                    }

                    builder.Insert(0, SIExponents[i].ToSuperScript());
                    builder.Insert(0, ((SI)i).GetSymbol());
                    builder.Insert(0, exp);
                }
            }
            return builder.ToString();
        }
        /// <summary>
        /// Parses a Unit from a string
        /// </summary>
        /// <param name="source">The string giving the Unit</param>
        /// <returns>A Unit representing the parsed string</returns>
        public static Unit Parse(string source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            Unit result = new();
            UnitExtensions.HandleSpecialSigns(source, out string[] Components, out bool[] sign);

            for (int i = 0; i < Components.Length; i++)
            {
                int exp = UnitExtensions.GetExponent(Components[i], sign[i], out string PreExp);
                if (PreExp.Length == 0)
                {
                    continue;
                }
                if (UnitExtensions.TryGetCompositeUnit(ref result, exp, PreExp))
                {
                    continue;
                }

                UnitExtensions.ReadSIUnit(ref result, exp, PreExp);
            }
            return result;
        }

        /// <summary>
        /// Scales this Unit to the largest fitting presentable value, assuming that the value is of the type that the Unit currently is. 
        /// </summary>
        /// <param name="value">The value to scale for</param>
        public void Fit(ref double value)
        {
            if (value == 0)
            {
                return;
            }

            if (value <= 0)
            {
                return;
            }

            int d = (int)Math.Floor(Math.Log10(value) / 3) * 3;
            if (d == 0)
            {
                return;
            }

            value /= Math.Pow(10, d);
            for (int i = 0; i < 7; i++)
            {
                if (SIExponents[i] != 0)
                {
                    SizeExponent[i] += d;
                    break;
                }
            }
        }
        public override bool Equals(object? obj)
        {
            if (obj is not Unit a)
            {
                return false;
            }

            return Equals(a);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Unit Multiply(Unit left, Unit right)
        {
            return left * right;
        }

        public static Unit Divide(Unit left, Unit right)
        {
            return left / right;
        }

        public static Unit Add(Unit left, Unit right)
        {
            return left + right;
        }

        public static Unit Xor(Unit left, int right)
        {
            return left ^ right;
        }
    }
}
