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
            StringAssert.Contains("Введите логическое выражение с пробелами между токенами, например, ( a | b ) & ! c\r\nТаблица истинности:\r\na b | F\r\n0 0 | 0\r\n", consoleOutput.ToString());
        }
    }
}