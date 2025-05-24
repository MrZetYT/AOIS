using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOIS_Lab2;

namespace AOIS.Tests2
{
    [TestFixture]
    public class OtherTests
    {
        private LogicalExpressionEvaluator _evaluator;

        [SetUp]
        public void SetUp()
        {
            _evaluator = new LogicalExpressionEvaluator();
        }

        [Test]
        public void GetIndexForm_AndExpression_ReturnsCorrectIndexForm()
        {
            string expression = "a & b";
            List<string> variables = new List<string> { "a", "b" };
            var truthTable = _evaluator.BuildTruthTable(expression, variables);
            var (index, binary) = _evaluator.GetIndexForm(truthTable);
            Assert.That(index, Is.EqualTo(1));
            Assert.That(binary, Is.EqualTo("0001"));
        }

        [Test]
        public void Program_ReturnsCorrectInfo()
        {
            var simulatedInput = new StringReader("( a | b ) & ! c\n");
            Console.SetIn(simulatedInput);

            var consoleOutput = new System.IO.StringWriter();
            Console.SetOut(consoleOutput);


            Program.Main();
            StringAssert.Contains("Индексная форма: 42 - 00101010", consoleOutput.ToString());
        }
    }
}
