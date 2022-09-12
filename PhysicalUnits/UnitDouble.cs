using System.Diagnostics.CodeAnalysis;

namespace PhysicalUnits
{
    public struct PhysicalUnit<T> : IParseable,  IEquatable<double>, IComparable, IComparable<double>, IComparable<int>, IEquatable<PhysicalUnit<T>>
    {
        public T Value { get; set; }
        public Unit Unit { get; set; } = Units.None;

        public PhysicalUnit(double value, Unit unit)
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

        public static PhysicalUnit<T> Parse(string source, IFormatProvider? formatProvider = null)
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


        public static PhysicalUnit<T> operator *(PhysicalUnit<T> left, PhysicalUnit<T> right) where T: INumber<T>
        {
            return new()
            {
                Value = left.Value * right.Value,
                Unit = left.Unit * right.Unit
            };
        }

        public static PhysicalUnit operator *(PhysicalUnit left, double right)
        {
            return new()
            {
                Value = left.Value * right,
                Unit = left.Unit
            };
        }

        public static PhysicalUnit operator *(PhysicalUnit left, int right)
        {
            return new()
            {
                Value = left.Value * right,
                Unit = left.Unit
            };
        }

        public static PhysicalUnit operator /(PhysicalUnit left, PhysicalUnit right)
        {
            return new()
            {
                Value = left.Value / right.Value,
                Unit = left.Unit / right.Unit
            };
        }

        public static PhysicalUnit operator /(PhysicalUnit left, double right)
        {
            return new()
            {
                Value = left.Value / right,
                Unit = left.Unit
            };
        }

        public static PhysicalUnit operator /(PhysicalUnit left, int right)
        {
            return new()
            {
                Value = left.Value / right,
                Unit = left.Unit
            };
        }

        public static PhysicalUnit operator /(double left, PhysicalUnit right)
        {
            return new()
            {
                Value = left / right.Value,
                Unit = right.Unit ^ -1
            };
        }

        public static PhysicalUnit operator /(int left, PhysicalUnit right)
        {
            return new()
            {
                Value = left / right.Value,
                Unit = right.Unit ^ -1
            };
        }

        public static PhysicalUnit operator +(PhysicalUnit left, PhysicalUnit right)
        {
            return new()
            {
                Value = left.Value + right.Value.Convert(right.Unit, left.Unit),
                Unit = left.Unit + right.Unit
            };
        }

        public static PhysicalUnit operator +(PhysicalUnit left, double right)
        {
            return new()
            {
                Value = left.Value + right,
                Unit = left.Unit
            };
        }

        public static PhysicalUnit operator -(PhysicalUnit left, PhysicalUnit right)
        {
            return new()
            {
                Value = left.Value - right.Value.Convert(right.Unit, left.Unit),
                Unit = left.Unit + right.Unit
            };
        }

        public static PhysicalUnit operator -(PhysicalUnit left, double right)
        {
            return new()
            {
                Value = left.Value + right,
                Unit = left.Unit
            };
        }

        public static PhysicalUnit operator *(double left, PhysicalUnit right)
        {
            return right * left;
        }

        public static PhysicalUnit operator +(double left, PhysicalUnit right)
        {
            return right + left;
        }

        public static PhysicalUnit operator -(double left, PhysicalUnit right)
        {
            return right - left;
        }

        public static implicit operator double(PhysicalUnit value)
        {
            return value.Value;
        }

        public static implicit operator Unit(PhysicalUnit value)
        {
            return value.Unit;
        }

        public static implicit operator PhysicalUnit(double value)
        {
            return new(value, Units.None);
        }

        public static implicit operator PhysicalUnit(Unit unit)
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

        public bool Equals(PhysicalUnit other)
        {
            return (other.Value == Value) && (other.Unit == Unit);
        }

        public bool Equals(double other)
        {
            return other == Value;
        }

        public int CompareTo(object? obj)
        {
            if (obj is PhysicalUnit ud)
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

            throw new ArgumentException($"Invalid comparison type for {typeof(PhysicalUnit)}: {obj?.GetType()}", nameof(obj));
        }
        public int CompareTo(PhysicalUnit other)
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

        public static bool operator ==(PhysicalUnit left, PhysicalUnit right)
        {
            return left.Equals(right);
        }

        public static bool operator ==(double left, PhysicalUnit right)
        {
            return left.Equals(right.Value);
        }

        public static bool operator ==(PhysicalUnit left, double right)
        {
            return right == left;
        }

        public static bool operator !=(PhysicalUnit left, PhysicalUnit right)
        {
            return !(left == right);
        }

        public static bool operator !=(double left, PhysicalUnit right)
        {
            return !(left == right);
        }

        public static bool operator !=(PhysicalUnit left, double right)
        {
            return right != left;
        }

        public static bool operator <(PhysicalUnit left, PhysicalUnit right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(PhysicalUnit left, PhysicalUnit right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(PhysicalUnit left, PhysicalUnit right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(PhysicalUnit left, PhysicalUnit right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator <(PhysicalUnit left, double right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(PhysicalUnit left, double right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(PhysicalUnit left, double right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(PhysicalUnit left, double right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator <(PhysicalUnit left, UnitInt right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(PhysicalUnit left, UnitInt right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(PhysicalUnit left, UnitInt right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(PhysicalUnit left, UnitInt right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static PhysicalUnit Multiply(PhysicalUnit left, PhysicalUnit right)
        {
            return left*right;
        }

        public static PhysicalUnit Divide(PhysicalUnit left, PhysicalUnit right)
        {
            return left*right;
        }

        public static PhysicalUnit Add(PhysicalUnit left, PhysicalUnit right)
        {
            return left+right;
        }

        public static PhysicalUnit Subtract(PhysicalUnit left, PhysicalUnit right)
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

        public static PhysicalUnit FromDouble(double value)
        {
            return value;
        }
        public static PhysicalUnit FromUnit(Unit value)
        {
            return value;
        }

        public bool Equals(PhysicalUnit<T> other)
        {
            throw new NotImplementedException();
        }
    }
}
