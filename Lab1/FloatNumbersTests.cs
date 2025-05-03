using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOIS_Lab1.Tests
{
    [TestFixture]
    public class FloatNumbersTests
    {
        [Test]
        public void ToFloatFromDecimal_PositiveNumber_CorrectConversion()
        {
            float number = 12.5f;

            var result = FloatNumbers.ToFloatFromDecimal(number);

            Assert.That(result, Has.Length.EqualTo(32));
            Assert.That(result[0], Is.EqualTo(0));
        }

        [Test]
        public void ToFloatFromDecimal_NegativeNumber_CorrectConversion()
        {
            float number = -12.5f;

            var result = FloatNumbers.ToFloatFromDecimal(number);

            Assert.That(result, Has.Length.EqualTo(32));
            Assert.That(result[0], Is.EqualTo(1));
        }

        [Test]
        public void ToFloatFromDecimal_Zero_CorrectConversion()
        {
            float number = 0f;

            var result = FloatNumbers.ToFloatFromDecimal(number);

            Assert.That(result, Has.Length.EqualTo(32));
            for (int i = 0; i < 32; i++)
            {
                Assert.That(result[i], Is.EqualTo(0));
            }
        }

        [Test]
        public void ToDecimalFromFloat_PositiveNumber_CorrectConversion()
        {
            int[] bits = new int[32];
            bits[0] = 0;
            bits[1] = 1;
            bits[8] = 1;
            bits[9] = 1;

            float result = FloatNumbers.ToDecimalFromFloat(bits);

            Assert.That(result, Is.GreaterThan(0));
        }

        [Test]
        public void ToDecimalFromFloat_NegativeNumber_CorrectConversion()
        {
            int[] bits = new int[32];
            bits[0] = 1;
            bits[1] = 1;
            bits[8] = 1;
            bits[9] = 1;

            float result = FloatNumbers.ToDecimalFromFloat(bits);

            Assert.That(result, Is.LessThan(0));
        }

        [Test]
        public void ToDecimalFromFloat_Zero_CorrectConversion()
        {
            int[] bits = new int[32];

            float result = FloatNumbers.ToDecimalFromFloat(bits);

            Assert.That(result, Is.EqualTo(0f));
        }

        [Test]
        public void FloatSum_TwoPositiveNumbers_CorrectResult()
        {
            float num1 = 5.5f;
            float num2 = 3.5f;
            var bits1 = FloatNumbers.ToFloatFromDecimal(num1);
            var bits2 = FloatNumbers.ToFloatFromDecimal(num2);

            var resultBits = FloatNumbers.FloatSum(bits1, bits2);
            float result = FloatNumbers.ToDecimalFromFloat(resultBits);

            Assert.That(result, Is.EqualTo(9f));
        }

        [Test]
        public void FloatSum_PositiveAndNegativeNumbers_CorrectResult()
        {
            float num1 = 5.5f;
            float num2 = -3.5f;
            var bits1 = FloatNumbers.ToFloatFromDecimal(num1);
            var bits2 = FloatNumbers.ToFloatFromDecimal(num2);

            var resultBits = FloatNumbers.FloatSum(bits1, bits2);
            float result = FloatNumbers.ToDecimalFromFloat(resultBits);

            Assert.That(result, Is.EqualTo(2f));
        }

        [Test]
        public void FloatSum_InfinityCases_CorrectHandling()
        {
            int[] infBits = new int[32];
            infBits[0] = 0;
            for (int i = 1; i < 9; i++) infBits[i] = 1;
            int[] normalBits = FloatNumbers.ToFloatFromDecimal(1.5f);

            var result1 = FloatNumbers.FloatSum(infBits, normalBits);
            var result2 = FloatNumbers.FloatSum(infBits, infBits);
            Assert.Multiple(() =>
            {
                Assert.That(FloatNumbers.ToDecimalFromFloat(result1), Is.EqualTo(float.PositiveInfinity));
                Assert.That(FloatNumbers.ToDecimalFromFloat(result2), Is.EqualTo(float.PositiveInfinity));
            });
        }

        [Test]
        public void ToBinaryString_CorrectFormat()
        {
            int[] bits = new int[32];
            bits[0] = 1;
            bits[1] = 1;
            bits[8] = 1;
            bits[9] = 1;

            var result = FloatNumbers.ToBinaryString(bits);

            StringAssert.Contains("1 1", result);
            StringAssert.Contains("1 1", result);
        }
        [Test]
        public void FloatSum_Arguments_ThrowsException_1()
        {
            int[] num1 = new int[33];
            var num2 = new int[32];

            Assert.Throws<ArgumentException>(() =>
            {
                FloatNumbers.FloatSum(num1, num2);
            });
        }
        [Test]
        public void FloatSum_Arguments_ThrowsException_2()
        {
            int[] num1 = new int[32];
            var num2 = new int[33];

            Assert.Throws<ArgumentException>(() =>
            {
                FloatNumbers.FloatSum(num1, num2);
            });
        }
        [Test]
        public void FloatSum_Arguments_ThrowsException_3()
        {
            int[] num1 = new int[33];
            var num2 = new int[33];

            Assert.Throws<ArgumentException>(() =>
            {
                FloatNumbers.FloatSum(num1, num2);
            });
        }
        [Test]
        public void FloatSum_Arguments_ThrowsException_4()
        {
            int[] num1 = new int[32];
            var num2 = new int[32];

            Assert.DoesNotThrow(() =>
            {
                FloatNumbers.FloatSum(num1, num2);
            });
        }
    }
}
