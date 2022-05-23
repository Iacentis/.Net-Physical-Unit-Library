using System.Diagnostics.CodeAnalysis;

namespace PhysicalUnits
{
    public struct UnitInt : IEquatable<UnitInt>, IEquatable<UnitDouble>, IEquatable<int>, IComparable, IComparable<UnitDouble>, IComparable<int>, IComparable<UnitInt>
    {
        public int Value { get; set; }
        public Unit Unit { get; set; } = Units.None;

        public UnitInt(int value, Unit unit)
        {
            Value = value;
            Unit = unit;
        }
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

            return new UnitDouble
            {
                Value = int.Parse(source.Split(' ').First(), formatProvider),
                Unit = Unit.Parse(source[source.IndexOf(' ', StringComparison.InvariantCulture)..])
            };
        }

        public static UnitInt operator *(UnitInt left, UnitInt right)
        {
            return new UnitInt
            {
                Value = left.Value * right.Value,
                Unit = left.Unit * right.Unit
            };
        }
        public static UnitInt operator *(UnitInt left, int right)
        {
            return new UnitInt
            {
                Value = left.Value * right,
                Unit = left.Unit
            };
        }
        public static UnitInt operator /(UnitInt left, UnitInt right)
        {
            return new UnitInt
            {
                Value = left.Value / right.Value,
                Unit = left.Unit / right.Unit
            };
        }
        public static UnitInt operator /(UnitInt left, int right)
        {
            return new UnitInt
            {
                Value = left.Value / right,
                Unit = left.Unit
            };
        }
        public static UnitInt operator /(int left, UnitInt right)
        {
            return new UnitInt
            {
                Value = left / right.Value,
                Unit = right.Unit ^ -1
            };
        }
        public static UnitInt operator +(UnitInt left, UnitInt right)
        {
            return new UnitInt
            {
                Value = left.Value + right.Value.Convert(right.Unit, left.Unit),
                Unit = left.Unit + right.Unit
            };
        }
        public static UnitInt operator +(UnitInt left, int right)
        {
            return new UnitInt
            {
                Value = left.Value + right,
                Unit = left.Unit
            };
        }
        public static UnitInt operator -(UnitInt left, UnitInt right)
        {
            return new UnitInt
            {
                Value = left.Value - right.Value.Convert(right.Unit, left.Unit),
                Unit = left.Unit + right.Unit
            };
        }
        public static UnitInt operator -(UnitInt left, int right)
        {
            return new UnitInt
            {
                Value = left.Value + right,
                Unit = left.Unit
            };
        }
        public static UnitInt operator *(int left, UnitInt right)
        {
            return right * left;
        }

        public static UnitInt operator +(int left, UnitInt right)
        {
            return right + left;
        }

        public static UnitInt operator -(int left, UnitInt right)
        {
            return right - left;
        }

        public static implicit operator int(UnitInt value)
        {
            return value.Value;
        }

        public static implicit operator Unit(UnitInt value)
        {
            return value.Unit;
        }

        public static implicit operator UnitInt(int value)
        {
            return new(value, Units.None);
        }

        public static implicit operator UnitInt(Unit unit)
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

        public bool Equals(UnitInt other)
        {
            return (other.Value == Value) && (other.Unit == Unit);
        }

        public bool Equals(UnitDouble other)
        {
            return (other.Value == Value) && (other.Unit == Unit);
        }

        public bool Equals(double other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object? obj)
        {
            return Value.CompareTo(obj);
        }

        public int CompareTo(UnitDouble other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(double other)
        {
            return Value.CompareTo(other);
        }

        public int CompareTo(UnitInt other)
        {
            if (other.Unit != Unit)
            {
                throw new ArgumentException("Invalid types for comparision", nameof(other));
            }

            return Value.CompareTo(other.Value);
        }

        public bool Equals(int other)
        {
            return Value.Equals(other);
        }

        public int CompareTo(int other)
        {
            return Value.CompareTo(other);
        }

        public static bool operator ==(UnitInt left, UnitInt right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(UnitInt left, UnitInt right)
        {
            return !(left == right);
        }

        public static bool operator <(UnitInt left, UnitInt right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(UnitInt left, UnitInt right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(UnitInt left, UnitInt right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(UnitInt left, UnitInt right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator <(UnitInt left, UnitDouble right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(UnitInt left, UnitDouble right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(UnitInt left, UnitDouble right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(UnitInt left, UnitDouble right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator <(UnitInt left, int right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(UnitInt left, int right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(UnitInt left, int right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(UnitInt left, int right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static UnitInt Multiply(UnitInt left, UnitInt right)
        {
            throw new NotImplementedException();
        }

        public static UnitInt Divide(UnitInt left, UnitInt right)
        {
            throw new NotImplementedException();
        }

        public static UnitInt Add(UnitInt left, UnitInt right)
        {
            throw new NotImplementedException();
        }

        public static UnitInt Subtract(UnitInt left, UnitInt right)
        {
            throw new NotImplementedException();
        }

        public Unit ToUnit()
        {
            return Unit;
        }

        public static UnitInt ToUnitInt(int value)
        {
            return value;
        }
        public static UnitInt ToUnitInt(Unit value)
        {
            return value;
        }

        public int ToInt32()
        {
            return Value;
        }
    }
}
