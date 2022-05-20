using Xunit;
using Xunit.Abstractions;
using PhysicalUnits;
using System;

namespace DataTypesTests
{
    public class UnitDataTypeTests
    {
        private readonly ITestOutputHelper output;
        public UnitDataTypeTests(ITestOutputHelper output)
        {
            this.output = output;
        }
        [Fact]
        public void ParseTest()
        {
            //Arrange
            Unit unit = Units.Newton * Units.Henry / Units.Kilogram;
            //Act
            string asString = unit.ToString();
            Unit newUnit = Unit.Parse(asString);
            output.WriteLine(asString);
            //Assert
            Assert.Equal(unit, newUnit);
        }
        [Fact]
        public void ParseTest2()
        {
            //Arrange
            string one = "m/s�";
            string two = "m/s2";
            string three = "m/s²";
            string four = "m/sÂ²";
            string five = "m s-2";
            string six = "ms-2";
            string seven = "kPa^2";
            //Act
            Unit unitOne = Unit.Parse(one);
            Unit unitTwo = Unit.Parse(two);
            Unit unitThree = Unit.Parse(three);
            Unit unitFour = Unit.Parse(four);
            Unit unitFive = Unit.Parse(five);
            Unit unitSix = Unit.Parse(six);
            Unit unitSeven = Unit.Parse(seven)*'k'*'m';
            //Assert
            Assert.Equal(Units.Acceleration, unitOne);
            Assert.Equal(Units.Acceleration, unitTwo);
            Assert.Equal(Units.Acceleration, unitThree);
            Assert.Equal(Units.Acceleration, unitFour);
            Assert.Equal(Units.Acceleration, unitFive);
            Assert.Equal('m' * (Units.Seconds ^ -2), unitSix);
            Assert.Equal('k' * (Units.Pascal ^ 2), unitSeven);
        }
        [Fact]
        public void MultiplyTest()
        {
            //Arrange
            Unit expected = new(0, 2);
            //Act
            Unit c = Units.Area;
            //Assert
            Assert.Equal(expected, c);
        }
        [Fact]
        public void DivisionTest()
        {
            Unit expected = new(-2, 1);

            //Act
            Unit c = Units.Acceleration;
            //Assert
            Assert.Equal(expected, c);
            Assert.Equal(1000, (Units.Metre / ('m' * Units.Metre)).ScaleFactor);
            Assert.Equal(1000000, (Units.Area / ('m' * Units.Area)).ScaleFactor);
        }
        [Fact]
        public void ExponentiationTest()
        {
            Unit expected = new(0, 3);
            Unit secondExpected = new(0, -3);
            //Act
            Unit c = Units.Volume;
            Unit d = c ^ -1;
            //Assert
            Assert.Equal(expected, c);
            Assert.Equal(secondExpected, d);
        }
        [Fact]
        public void FitTest()
        {
            //Arrange
            Unit a = Units.Kilogram;
            double val = 0.123;
            Unit b = Units.Metre;
            double val2 = 1200;
            Unit c = Units.Ampere;
            double val3 = 3;
            Unit d = Units.Mole;
            double val4 = 1.231e-5;

            //Act
            a.Fit(ref val);
            b.Fit(ref val2);
            c.Fit(ref val3);
            d.Fit(ref val4);
            //Assert
            Assert.Equal(123, val);
            Assert.Equal(1.2, val2);
            Assert.Equal(3, val3);
            Assert.Equal(12.31, val4);
            Assert.Equal("g", a.ToString());
            Assert.Equal("km", b.ToString());
            Assert.Equal("A", c.ToString());
            Assert.Equal(MetricPrefixExponents.micro.GetSymbol()+"mol", d.ToString());
        }
        [Fact]
        public void ConvertTest()
        {
            //Arrange
            double val = 10;

            //Act
            double output1 = val.Convert(Units.Kilogram, 'm' * Units.Kilogram);
            double output2 = output1.Convert('m' * Units.Kilogram, Units.Kilogram);


            //Assert
            Assert.Equal(10000, output1);
            Assert.Equal(val, output2);
            Assert.Throws<ArgumentException>(() => val.Convert(Units.Kilogram, Units.Katal));
            Assert.Throws<ArgumentException>(() => val.Convert(Units.Pascal, Units.Newton));
        }
    }
}