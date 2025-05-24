using NUnit.Framework;
using System.Collections.Generic;
using System;
using AOIS_Lab2;

namespace AOIS.Tests2
{
    [TestFixture]
    public class GetVariablesTests
    {
        private LogicalExpressionEvaluator _evaluator;

        [SetUp]
        public void SetUp()
        {
            _evaluator = new LogicalExpressionEvaluator();
        }

        [Test]
        public void GetVariables_MultipleVariables_ReturnsSortedList()
        {
            string expression = "( a | b ) & ! c";
            var result = _evaluator.GetVariables(expression);
            Assert.That(result, Is.EqualTo(new List<string> { "a", "b", "c" }));
        }

        [Test]
        public void GetVariables_SingleVariable_ReturnsListWithOneVariable()
        {
            string expression = "a";
            var result = _evaluator.GetVariables(expression);
            Assert.That(result, Is.EqualTo(new List<string> { "a" }));
        }

        [Test]
        public void GetVariables_RepeatedVariables_ReturnsUniqueVariables()
        {
            string expression = "a & a";
            var result = _evaluator.GetVariables(expression);
            Assert.That(result, Is.EqualTo(new List<string> { "a" }));
        }

        [Test]
        public void GetVariables_VariablesOutOfOrder_ReturnsSortedList()
        {
            string expression = "b & a";
            var result = _evaluator.GetVariables(expression);
            Assert.That(result, Is.EqualTo(new List<string> { "a", "b" }));
        }

        [Test]
        public void GetVariables_AllVariables_ReturnsSortedList()
        {
            string expression = "a & b & c & d & e";
            var result = _evaluator.GetVariables(expression);
            Assert.That(result, Is.EqualTo(new List<string> { "a", "b", "c", "d", "e" }));
        }
    }
}