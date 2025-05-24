using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOIS_Lab2;

namespace AOIS.Tests2
{
    [TestFixture]
    public class GetPDNFTests
    {
        private LogicalExpressionEvaluator _evaluator;

        [SetUp]
        public void SetUp()
        {
            _evaluator = new LogicalExpressionEvaluator();
        }
        [Test]
        public void GetPDNF_AndExpression_ReturnsCorrectPDNF()
        {
            string expression = "a & b";
            List<string> variables = new List<string> { "a", "b" };
            var truthTable = _evaluator.BuildTruthTable(expression, variables);
            string result = _evaluator.GetPDNF(truthTable, variables);
            Assert.That(result, Is.EqualTo("(a ∧ b)"));
        }

        [Test]
        public void GetPDNF_OrExpression_ReturnsCorrectPDNF()
        {
            string expression = "a | b";
            List<string> variables = new List<string> { "a", "b" };
            var truthTable = _evaluator.BuildTruthTable(expression, variables);
            string result = _evaluator.GetPDNF(truthTable, variables);
            string expected = "(¬a ∧ b) ∨ (a ∧ ¬b) ∨ (a ∧ b)";
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GetPDNFNumerical_AndExpression_ReturnsCorrectIndices()
        {
            string expression = "a & b";
            List<string> variables = new List<string> { "a", "b" };
            var truthTable = _evaluator.BuildTruthTable(expression, variables);
            var result = _evaluator.GetPDNFNumerical(truthTable, variables);
            Assert.That(result, Is.EqualTo(new List<int> { 3 }));
        }
    }
}
