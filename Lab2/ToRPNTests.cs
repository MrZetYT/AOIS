using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOIS_Lab2;

namespace AOIS.Tests2
{
    [TestFixture]
    public class ToRPNTests
    {
        private LogicalExpressionEvaluator _evaluator;

        [SetUp]
        public void SetUp()
        {
            _evaluator = new LogicalExpressionEvaluator();
        }

        [Test]
        public void ToRPN_SimpleAndExpression_ReturnsCorrectRPN()
        {
            string expression = "a & b";
            var result = _evaluator.ToRPN(expression);
            Assert.That(result, Is.EqualTo(new List<string> { "a", "b", "&" }));
        }

        [Test]
        public void ToRPN_ExpressionWithParentheses_ReturnsCorrectRPN()
        {
            string expression = "( a & b ) | c";
            var result = _evaluator.ToRPN(expression);
            Assert.That(result, Is.EqualTo(new List<string> { "a", "b", "&", "c", "|" }));
        }

        [Test]
        public void ToRPN_NegationExpression_ReturnsCorrectRPN()
        {
            string expression = "! a";
            var result = _evaluator.ToRPN(expression);
            Assert.That(result, Is.EqualTo(new List<string> { "a", "!" }));
        }

        [Test]
        public void ToRPN_ComplexExpressionWithImplicationAndEquivalence_ReturnsCorrectRPN()
        {
            string expression = "( a -> b ) ~ c";
            var result = _evaluator.ToRPN(expression);
            Assert.That(result, Is.EqualTo(new List<string> { "a", "b", "->", "c", "~" }));
        }

        [Test]
        public void ToRPN_InvalidExpression_ThrowsException()
        {
            string expression = "( a & b";
            Assert.Throws<Exception>(() => _evaluator.ToRPN(expression));
        }
    }
}
