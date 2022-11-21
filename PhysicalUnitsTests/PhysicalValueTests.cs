using PhysicalUnits;
using PhysicalValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace PhysicalUnitsTests
{
    public class PhysicalValueTests
    {
        private readonly ITestOutputHelper output;
        public PhysicalValueTests(ITestOutputHelper output)
        {
            this.output = output;
        }
        [Fact]
        public void Parse()
        {
            //Arrange
            string Parseable = "2 m/s";
            PhysicalValue<int> expected = new() { Value = 2, Unit = Units.Velocity };
            //Act
            PhysicalValue<int> actual = PhysicalValue<int>.Parse(Parseable);
            //Assert
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void ParseFailure()
        {
            //Arrange
            string Unparseable = "m/s";
            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => PhysicalValue<int>.Parse(Unparseable));
        }
        [Fact]
        public void TryParse()
        {

            //Arrange
            string Parseable = "2 m/s";
            PhysicalValue<int> expected = new() { Value = 2, Unit = Units.Velocity };
            //Act
            bool success = PhysicalValue<int>.TryParse(Parseable, null, result: out var actual);
            //Assert
            Assert.Equal(expected, actual);
            Assert.True(success);
        }
        [Fact]
        public void TryParseFailure()
        {

            //Arrange
            string Unparseable = "m/s";
            //Act
            bool success = PhysicalValue<int>.TryParse(Unparseable, null, result: out var actual);
            //Assert
            Assert.False(success);
        }
        [Fact]
        public void Multiply()
        {
            PhysicalValue<double> expected = new()
            {
                Unit = Units.Velocity,
                Value = 7.5
            };
            PhysicalValue<double> a = new()
            {
                Unit = Units.Seconds ^ (-1),
                Value = 2.5
            };
            PhysicalValue<double> b = new()
            {
                Unit = Units.Metre,
                Value = 3.0
            };
            PhysicalValue<double> c = a * b;
            Assert.Equal(expected, c);
        }

        [Fact]
        public void Division()
        {

            PhysicalValue<double> expected = new()
            {
                Unit = Units.Velocity,
                Value = 1.2
            };
            PhysicalValue<double> a = new()
            {
                Unit = Units.Seconds,
                Value = 2.5
            };
            PhysicalValue<double> b = new()
            {
                Unit = Units.Metre,
                Value = 3.0
            };
            PhysicalValue<double> c = b / a;
            Assert.Equal(expected, c);
        }

        [Fact]
        public void Addition()
        {
            PhysicalValue<double> expected = new()
            {
                Unit = Units.Seconds,
                Value = 2.3
            };
            PhysicalValue<double> a = new()
            {
                Unit = Units.Seconds,
                Value = 1.2
            };
            PhysicalValue<double> b = new()
            {
                Unit = Units.Seconds,
                Value = 1.1
            };
            PhysicalValue<double> c = a + b;
            Assert.Equal(expected, c);
        }

        [Fact]
        public void AdditionFailure()
        {
            PhysicalValue<double> expected = new()
            {
                Unit = Units.Seconds,
                Value = 2.3
            };
            PhysicalValue<double> a = new()
            {
                Unit = Units.Metre,
                Value = 1.2
            };
            PhysicalValue<double> b = new()
            {
                Unit = Units.Seconds,
                Value = 1.1
            };
            Assert.Throws<ArgumentException>(() => a + b);
        }
        [Fact]
        public void Subtraction()
        {
            PhysicalValue<double> expected = new()
            {
                Unit = Units.Seconds,
                Value = 0.1
            };
            PhysicalValue<double> a = new()
            {
                Unit = Units.Seconds,
                Value = 1.2
            };
            PhysicalValue<double> b = new()
            {
                Unit = Units.Seconds,
                Value = 1.1
            };
            PhysicalValue<double> c = a - b;
            Assert.Equal(expected, c);
        }

        [Fact]
        public void SubtractionFailure()
        {
            PhysicalValue<double> expected = new()
            {
                Unit = Units.Seconds,
                Value = 2.3
            };
            PhysicalValue<double> a = new()
            {
                Unit = Units.Metre,
                Value = 1.2
            };
            PhysicalValue<double> b = new()
            {
                Unit = Units.Seconds,
                Value = 1.1
            };
            Assert.Throws<ArgumentException>(() => a - b);
        }
        [Fact]
        public void Constructor()
        {
            var a = new PhysicalValue<int>(1, Units.Acceleration);
            var b = new PhysicalValue<double>(1.0, Units.Acceleration);
            var c = new PhysicalValue<byte>(1, Units.Acceleration);
            var d = new PhysicalValue<short>(1, Units.Acceleration);
            var e = new PhysicalValue<float>(1, Units.Acceleration);
            Assert.True(true);
        }
        [Fact]
        public void ConversionTest()
        {
            var a = new PhysicalValue<double>(1, Units.Minute);
            var b = new PhysicalValue<double>(1, Units.Seconds);
            var c = new PhysicalValue<double>(1, Units.Hour);
            var d = b + a + c;
            output.WriteLine(d.ToString());
        }
    }
}
