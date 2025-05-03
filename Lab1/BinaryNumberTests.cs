using NUnit.Framework;
using AOIS_Lab1;
using System;

namespace AOIS_Lab1.Tests
{
    [TestFixture]
    public class BinaryNumberTests
    {
        [Test]
        public void ToDirectBinaryNumber_PositiveNumber_CorrectConversion()
        {
            var binaryNumber = new BinaryNumber();
            int number = 5;

            binaryNumber.ToDirectBinaryNumber(number);

            Assert.That(binaryNumber.ToDecimalNumber(), Is.EqualTo(5));
        }

        [Test]
        public void ToDirectBinaryNumber_NegativeNumber_CorrectConversion()
        {
            var binaryNumber = new BinaryNumber();
            int number = -5;

            binaryNumber.ToDirectBinaryNumber(number);

            Assert.That(binaryNumber.ToDecimalNumber(), Is.EqualTo(-5));
        }

        [Test]
        public void ToDirectBinaryNumber_Zero_CorrectConversion()
        {
            var binaryNumber = new BinaryNumber();
            int number = 0;

            binaryNumber.ToDirectBinaryNumber(number);

            Assert.That(binaryNumber.ToDecimalNumber(), Is.EqualTo(0));
        }

        [Test]
        public void ToDirectBinaryNumber_InvalidInput_PrintsErrorMessage()
        {
            var binaryNumber = new BinaryNumber();

            var simulatedInput = new StringReader("abc\n128\n127\n");
            Console.SetIn(simulatedInput);

            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            binaryNumber.ToDirectBinaryNumber(128);

            var output = consoleOutput.ToString();
            StringAssert.Contains("Неправильный формат", output);
        }


        [Test]
        public void Addition_TwoPositiveNumbers_CorrectResult()
        {
            var num1 = new BinaryNumber();
            var num2 = new BinaryNumber();
            num1.ToDirectBinaryNumber(5);
            num2.ToDirectBinaryNumber(3);

            var result = num1 + num2;

            Assert.That(result.ToDecimalNumber(), Is.EqualTo(8));
        }

        [Test]
        public void Addition_PositiveAndNegativeNumbers_CorrectResult()
        {
            var num1 = new BinaryNumber();
            var num2 = new BinaryNumber();
            num1.ToDirectBinaryNumber(5);
            num2.ToDirectBinaryNumber(-3);

            var result = num1 + num2;

            Assert.That(result.ToDecimalNumber(), Is.EqualTo(2));
        }

        [Test]
        public void Subtraction_TwoPositiveNumbers_CorrectResult()
        {
            var num1 = new BinaryNumber();
            var num2 = new BinaryNumber();
            num1.ToDirectBinaryNumber(5);
            num2.ToDirectBinaryNumber(3);

            var result = num1 - num2;

            Assert.That(result.ToDecimalNumber(), Is.EqualTo(2));
        }

        [Test]
        public void Multiplication_TwoPositiveNumbers_CorrectResult()
        {
            var num1 = new BinaryNumber();
            var num2 = new BinaryNumber();
            num1.ToDirectBinaryNumber(5);
            num2.ToDirectBinaryNumber(3);

            var result = num1 * num2;

            Assert.That(result.ToDecimalNumber(), Is.EqualTo(15));
        }

        [Test]
        public void Division_TwoPositiveNumbers_CorrectResult()
        {
            var num1 = new BinaryNumber();
            var num2 = new BinaryNumber();
            num1.ToDirectBinaryNumber(10);
            num2.ToDirectBinaryNumber(2);

            var result = num1 / num2;

            Assert.That(result.ToDecimalNumber(), Is.EqualTo(5));
        }

        [Test]
        public void Division_ByZero_ThrowsException()
        {
            var num1 = new BinaryNumber();
            var num2 = new BinaryNumber();
            num1.ToDirectBinaryNumber(10);
            num2.ToDirectBinaryNumber(0);

            Assert.Throws<DivideByZeroException>(() =>
            {
                var result = num1 / num2;
            });
        }

        [Test]
        public void Show_OutputsCorrectInformation()
        {
            var binaryNumber = new BinaryNumber();
            binaryNumber.ToDirectBinaryNumber(10);
            var consoleOutput = new System.IO.StringWriter();
            Console.SetOut(consoleOutput);

            binaryNumber.Show();

            StringAssert.Contains("Десятичное число: 10", consoleOutput.ToString());
            StringAssert.Contains("Прямой код:", consoleOutput.ToString());
            StringAssert.Contains("Обратный код:", consoleOutput.ToString());
            StringAssert.Contains("Дополнительный код:", consoleOutput.ToString());
        }
    }
}