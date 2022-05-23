using System.Diagnostics.CodeAnalysis;

namespace PhysicalUnits
{
    public struct UnitDouble : IEquatable<UnitDouble>, IEquatable<double>, IComparable, IComparable<UnitDouble>, IComparable<double>, IComparable<UnitInt>, IComparable<int>
    {
        public double Value { get; set; }
        public Unit Unit { get; set; } = Units.None;

        public override string ToString()
        {
            return ToString(null);
        }
        public string ToString(IFormatProvider? formatProvider)
        {
            return Value.ToString(formatProvider) + ' ' + Unit.ToString();
        }

        public static UnitDouble Parse(string source, IFormatProvider? formatProvider = null)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new()
            {
                Value = double.Parse(source.Split(' ').First(), formatProvider),
                Unit = Unit.Parse(source[source.IndexOf(' ', StringComparison.InvariantCulture)..])
            };
        }

        public UnitDouble(double value, Unit unit)
        {
            Value = value;
            Unit = unit;
        }

        public static UnitDouble operator *(UnitDouble left, UnitDouble right)
        {
            return new()
            {
                Value = left.Value * right.Value,
                Unit = left.Unit * right.Unit
            };
        }

        public static UnitDouble operator *(UnitDouble left, double right)
        {
            return new()
            {
                Value = left.Value * right,
                Unit = left.Unit
            };
        }

        public static UnitDouble operator *(UnitDouble left, int right)
        {
            return new()
            {
                Value = left.Value * right,
                Unit = left.Unit
            };
        }

        public static UnitDouble operator /(UnitDouble left, UnitDouble right)
        {
            return new()
            {
                Value = left.Value / right.Value,
                Unit = left.Unit / right.Unit
            };
        }

        public static UnitDouble operator /(UnitDouble left, double right)
        {
            return new()
            {
                Value = left.Value / right,
                Unit = left.Unit
            };
        }

        public static UnitDouble operator /(UnitDouble left, int right)
        {
            return new()
            {
                Value = left.Value / right,
                Unit = left.Unit
            };
        }

        public static UnitDouble operator /(double left, UnitDouble right)
        {
            return new()
            {
                Value = left / right.Value,
                Unit = right.Unit ^ -1
            };
        }

        public static UnitDouble operator /(int left, UnitDouble right)
        {
            return new()
            {
                Value = left / right.Value,
                Unit = right.Unit ^ -1
            };
        }

        public static UnitDouble operator +(UnitDouble left, UnitDouble right)
        {
            return new()
            {
                Value = left.Value + right.Value.Convert(right.Unit, left.Unit),
                Unit = left.Unit + right.Unit
            };
        }

        public static UnitDouble operator +(UnitDouble left, double right)
        {
            return new()
            {
                Value = left.Value + right,
                Unit = left.Unit
            };
        }

        public static UnitDouble operator -(UnitDouble left, UnitDouble right)
        {
            return new()
            {
                Value = left.Value - right.Value.Convert(right.Unit, left.Unit),
                Unit = left.Unit + right.Unit
            };
        }

        public static UnitDouble operator -(UnitDouble left, double right)
        {
            return new()
            {
                Value = left.Value + right,
                Unit = left.Unit
            };
        }

        public static UnitDouble operator *(double left, UnitDouble right)
        {
            return right * left;
        }

        public static UnitDouble operator +(double left, UnitDouble right)
        {
            return right + left;
        }

        public static UnitDouble operator -(double left, UnitDouble right)
        {
            return right - left;
        }

        public static implicit operator double(UnitDouble value)
        {
            return value.Value;
        }

        public static implicit operator Unit(UnitDouble value)
        {
            return value.Unit;
        }

        public static implicit operator UnitDouble(double value)
        {
            return new(value, Units.None);
        }

        public static implicit operator UnitDouble(Unit unit)
        {
            return new(0, unit);
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Equals(UnitDouble other)
        {
            return (other.Value == Value) && (other.Unit == Unit);
        }

        public bool Equals(double other)
        {
            return other == Value;
        }

        public int CompareTo(object? obj)
        {
            if (obj is UnitDouble ud)
            {
                return CompareTo(ud);
            }

            if (obj is double d)
            {
                return CompareTo(d);
            }

            if (obj is UnitInt ui)
            {
                return CompareTo(ui);
            }

            throw new ArgumentException($"Invalid comparison type for {typeof(UnitDouble)}: {obj?.GetType()}", nameof(obj));
        }
        public int CompareTo(UnitDouble other)
        {
            return (int)(Value - other.Value);
        }

        public int CompareTo(double other)
        {
            return (int)(Value - other);
        }

        public int CompareTo(UnitInt other)
        {
            return (int)(Value - other.Value);
        }

        public int CompareTo(int other)
        {
            return Value.CompareTo(other);
        }

        public static bool operator ==(UnitDouble left, UnitDouble right)
        {
            return left.Equals(right);
        }

        public static bool operator ==(double left, UnitDouble right)
        {
            return left.Equals(right.Value);
        }

        public static bool operator ==(UnitDouble left, double right)
        {
            return right == left;
        }

        public static bool operator !=(UnitDouble left, UnitDouble right)
        {
            return !(left == right);
        }

        public static bool operator !=(double left, UnitDouble right)
        {
            return !(left == right);
        }

        public static bool operator !=(UnitDouble left, double right)
        {
            return right != left;
        }

        public static bool operator <(UnitDouble left, UnitDouble right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(UnitDouble left, UnitDouble right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(UnitDouble left, UnitDouble right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(UnitDouble left, UnitDouble right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator <(UnitDouble left, double right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(UnitDouble left, double right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(UnitDouble left, double right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(UnitDouble left, double right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator <(UnitDouble left, UnitInt right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(UnitDouble left, UnitInt right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(UnitDouble left, UnitInt right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(UnitDouble left, UnitInt right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static UnitDouble Multiply(UnitDouble left, UnitDouble right)
        {
            return left*right;
        }

        public static UnitDouble Divide(UnitDouble left, UnitDouble right)
        {
            return left*right;
        }

        public static UnitDouble Add(UnitDouble left, UnitDouble right)
        {
            return left+right;
        }

        public static UnitDouble Subtract(UnitDouble left, UnitDouble right)
        {
            return left-right;
        }

        public double ToDouble()
        {
            return Value;
        }

        public Unit ToUnit()
        {
            return Unit;
        }

        public static UnitDouble FromDouble(double value)
        {
            return value;
        }
        public static UnitDouble FromUnit(Unit value)
        {
            return value;
        }
    }
}
