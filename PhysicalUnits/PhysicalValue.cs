using PhysicalUnits;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
namespace PhysicalValues
{
    public struct PhysicalValue<T> : IParsable<PhysicalValue<T>>, IComparable<double>, IComparable<PhysicalValue<T>> where T : INumber<T>
    {
        public T Value { get; set; } = T.Zero;
        public Unit Unit { get; set; } = Units.None;


        public PhysicalValue(T value, Unit unit)
        {
            Value = value;
            Unit = unit;
        }
        public override string ToString() => Value.ToString() + ' ' + Unit.ToString();
        #region IParseable
        public static PhysicalValue<T> Parse(string? source, IFormatProvider? formatProvider = null)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            int UnitStart = source.IndexOf(' ', StringComparison.InvariantCulture);

            return new()
            {
                Value = T.Parse(source.AsSpan()[..UnitStart], formatProvider),
                Unit = Unit.Parse(source[UnitStart..])
            };
        }
        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out PhysicalValue<T> result)
        {
            try
            {
                result = Parse(s, provider);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }
        #endregion
        #region Math Functions
        public static PhysicalValue<T> Add<TOther>(PhysicalValue<T> left, TOther right) where TOther : INumber<TOther>
        {
            return new()
            {
                Value = left.Value + T.CreateChecked(right),
                Unit = left.Unit
            };
        }
        public static PhysicalValue<T> Multiply<TOther>(PhysicalValue<T> left, TOther right) where TOther : INumber<TOther>
        {
            return new()
            {
                Value = left.Value * T.CreateChecked(right),
                Unit = left.Unit
            };
        }
        public static PhysicalValue<T> Subtract<TOther>(PhysicalValue<T> left, TOther right) where TOther : INumber<TOther>
        {
            return new()
            {
                Value = left.Value - T.CreateChecked(right),
                Unit = left.Unit
            };
        }
        public static PhysicalValue<T> Divide<TOther>(PhysicalValue<T> left, TOther right) where TOther : INumber<TOther>
        {
            return new()
            {
                Value = left.Value / T.CreateChecked(right),
                Unit = left.Unit
            };
        }
        public static PhysicalValue<T> Subtract<TOther>(TOther left, PhysicalValue<T> right) where TOther : INumber<TOther>
        {
            return new()
            {
                Value = T.CreateChecked(left) - right.Value,
                Unit = right.Unit
            };
        }
        public static PhysicalValue<T> Divide<TOther>(TOther left, PhysicalValue<T> right) where TOther : INumber<TOther>
        {
            return new()
            {
                Value = T.CreateChecked(left) / right.Value,
                Unit = right.Unit
            };
        }

        public static PhysicalValue<T> Add(PhysicalValue<T> left, PhysicalValue<T> right)
        {
            return new()
            {
                Value = left.Value + T.CreateChecked(right.Unit.ScaleFactor) / T.CreateChecked(left.Unit.ScaleFactor) * right.Value,
                Unit = left.Unit + right.Unit
            };
        }
        public static PhysicalValue<T> Multiply(PhysicalValue<T> left, PhysicalValue<T> right)
        {
            return new()
            {
                Value = left.Value * right.Value,
                Unit = left.Unit * right.Unit
            };
        }
        public static PhysicalValue<T> Subtract(PhysicalValue<T> left, PhysicalValue<T> right)
        {
            return new()
            {
                Value = left.Value - T.CreateChecked(right.Unit.ScaleFactor) / T.CreateChecked(left.Unit.ScaleFactor) * right.Value,
                Unit = left.Unit + right.Unit,
            };
        }
        public static PhysicalValue<T> Divide(PhysicalValue<T> left, PhysicalValue<T> right)
        {
            return new()
            {
                Value = left.Value / right.Value,
                Unit = left.Unit / right.Unit,
            };
        }
        #endregion
        #region Math Operators
        public static PhysicalValue<T> operator *(PhysicalValue<T> left, PhysicalValue<T> right)
        {
            return Multiply(left, right);
        }

        public static PhysicalValue<T> operator *(PhysicalValue<T> left, double right)
        {
            return Multiply(left, right);
        }

        public static PhysicalValue<T> operator *(PhysicalValue<T> left, int right)
        {
            return Multiply(left, right);
        }

        public static PhysicalValue<T> operator /(PhysicalValue<T> left, PhysicalValue<T> right)
        {
            return Divide(left, right);
        }

        public static PhysicalValue<T> operator /(PhysicalValue<T> left, double right)
        {
            return Divide(left, right);
        }

        public static PhysicalValue<T> operator /(PhysicalValue<T> left, int right)
        {
            return Divide(left, right);
        }

        public static PhysicalValue<T> operator /(double left, PhysicalValue<T> right)
        {
            return Divide(left, right);
        }

        public static PhysicalValue<T> operator /(int left, PhysicalValue<T> right)
        {
            return Divide(left, right);
        }

        public static PhysicalValue<T> operator +(PhysicalValue<T> left, PhysicalValue<T> right)
        {
            return Add(left, right);
        }

        public static PhysicalValue<T> operator +(PhysicalValue<T> left, double right)
        {
            return Add(left, right);
        }

        public static PhysicalValue<T> operator -(PhysicalValue<T> left, PhysicalValue<T> right)
        {
            return Subtract(left,right);
        }

        public static PhysicalValue<T> operator -(PhysicalValue<T> left, double right)
        {
            return Subtract(left, right);
        }

        public static PhysicalValue<T> operator *(double left, PhysicalValue<T> right)
        {
            return Multiply(right, left);
        }

        public static PhysicalValue<T> operator +(double left, PhysicalValue<T> right)
        {
            return Add(right, left);
        }

        public static PhysicalValue<T> operator -(double left, PhysicalValue<T> right)
        {
            return Subtract(left, right);
        }
        #endregion
        #region Equality Functions
        public bool Equals(PhysicalValue<T> other)
        {
            return (other.Value == Value) && (other.Unit == Unit);
        }

        public bool Equals<TOther>(TOther other) where TOther : INumber<TOther>
        {
            return T.CreateChecked(other) == Value;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is PhysicalValue<T> other) return Equals(other);
            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode() + Unit.GetHashCode();
        }
        #endregion
        #region Equality Operators
        public static bool operator ==(PhysicalValue<T> left, PhysicalValue<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator ==(double left, PhysicalValue<T> right)
        {
            return left.Equals(right.Value);
        }

        public static bool operator ==(PhysicalValue<T> left, double right)
        {
            return right == left;
        }

        public static bool operator !=(PhysicalValue<T> left, PhysicalValue<T> right)
        {
            return !(left == right);
        }

        public static bool operator !=(double left, PhysicalValue<T> right)
        {
            return !(left == right);
        }

        public static bool operator !=(PhysicalValue<T> left, double right)
        {
            return right != left;
        }
        #endregion
        #region Comparison Operators
        public static bool operator <(PhysicalValue<T> left, PhysicalValue<T> right)
        {
            return left.CompareTo(right) < 0;
        }


        public static bool operator <=(PhysicalValue<T> left, PhysicalValue<T> right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(PhysicalValue<T> left, PhysicalValue<T> right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(PhysicalValue<T> left, PhysicalValue<T> right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator <(PhysicalValue<T> left, double right)
        {
            return left.CompareTo(right) < 0;
        }
        public static bool operator <=(PhysicalValue<T> left, double right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(PhysicalValue<T> left, double right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(PhysicalValue<T> left, double right)
        {
            return left.CompareTo(right) >= 0;
        }
        #endregion
        #region Comparison Functions
        private int CompareTo(PhysicalValue<T> right)
        {
            return int.CreateChecked(Value - right.Value);
        }
        private int CompareTo(double right)
        {
            return int.CreateChecked(Value - T.CreateChecked(right));
        }

        int IComparable<PhysicalValue<T>>.CompareTo(PhysicalValue<T> other)
        {
            return CompareTo(other);
        }

        int IComparable<double>.CompareTo(double other)
        {
            return CompareTo(other);
        }
        #endregion
        #region Implicit Conversions

        public static implicit operator T(PhysicalValue<T> value)
        {
            return value.Value;
        }

        public static implicit operator Unit(PhysicalValue<T> value)
        {
            return value.Unit;
        }

        public static implicit operator PhysicalValue<T>(T value)
        {
            return new(value, Units.None);
        }

        public static implicit operator PhysicalValue<T>(Unit unit)
        {
            return new(T.Zero, unit);
        }
        #endregion
    }
}
