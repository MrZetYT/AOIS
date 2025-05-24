using AOIS_Lab3;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AOIS.Tests3
{
    [TestFixture]
    public class OtherTests
    {
        [Test]
        public void Program_ComplexExpression_ReturnsMinimalSOP()
        {
            var simulatedInput = new StringReader("( a & b ) | ( ! a & b )\n");
            Console.SetIn(simulatedInput);

            var consoleOutput = new System.IO.StringWriter();
            Console.SetOut(consoleOutput);


            Program.Main();
            StringAssert.Contains("Карта Карно для 2 переменных (СКНФ):\r\n  b | 0 | 1\r\na\\\r\n0 | 0 | 1\r\n1 | 0 | 1\r\nМинимизированное выражение: b", consoleOutput.ToString());
        }
    }
}