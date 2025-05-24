using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOIS_Lab2;

namespace AOIS.Tests2
{
    [TestFixture]
    public class EvaluateRPNTests
    {
        private LogicalExpressionEvaluator _evaluator;

        [SetUp]
        public void SetUp()
        {
            _evaluator = new LogicalExpressionEvaluator();
        }

        [Test]
        public void EvaluateRPN_AndExpressionTrue_ReturnsTrue()
        {
            List<string> rpn = new List<string> { "a", "b", "&" };
            Dictionary<string, bool> values = new Dictionary<string, bool> { { "a", true }, { "b", true } };
            bool result = _evaluator.EvaluateRPN(rpn, values);
            Assert.IsTrue(result);
        }

        [Test]
        public void EvaluateRPN_AndExpressionFalse_ReturnsFalse()
        {
            List<string> rpn = new List<string> { "a", "b", "&" };
            Dictionary<string, bool> values = new Dictionary<string, bool> { { "a", true }, { "b", false } };
            bool result = _evaluator.EvaluateRPN(rpn, values);
            Assert.IsFalse(result);
        }

        [Test]
        public void EvaluateRPN_NegationExpression_ReturnsCorrectValue()
        {
            List<string> rpn = new List<string> { "a", "!" };
            Dictionary<string, bool> values = new Dictionary<string, bool> { { "a", false } };
            bool result = _evaluator.EvaluateRPN(rpn, values);
            Assert.IsTrue(result);
        }

        [Test]
        public void EvaluateRPN_ImplicationExpression_ReturnsCorrectValue()
        {
            List<string> rpn = new List<string> { "a", "b", "->" };
            Dictionary<string, bool> values = new Dictionary<string, bool> { { "a", true }, { "b", false } };
            bool result = _evaluator.EvaluateRPN(rpn, values);
            Assert.IsFalse(result);
        }
    }
}
