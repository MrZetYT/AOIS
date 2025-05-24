using AOIS_Lab3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOIS.Tests3
{
    [TestFixture]
    public class PerformanceTests
    {
        private LogicalExpressionEvaluator evaluator;

        [SetUp]
        public void Setup()
        {
            evaluator = new LogicalExpressionEvaluator();
        }

        [Test]
        public void Performance_LargeNumberOfCombinations()
        {
            var variables = new List<string> { "a", "b", "c", "d", "e" };

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var combinations = evaluator.GenerateCombinations(variables);
            stopwatch.Stop();

            Assert.That(combinations.Count, Is.EqualTo(32));
            Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(1000));
        }

        [Test]
        public void Performance_ComplexExpressionEvaluation()
        {
            string expression = "( a & b ) | ( c & d ) | ( e & a ) | ( b & c )";
            var variables = evaluator.GetVariables(expression);

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var truthTable = evaluator.BuildTruthTable(expression, variables);
            stopwatch.Stop();

            Assert.That(truthTable.Count, Is.EqualTo(32));
            Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(1000));
        }

        [Test]
        public void Performance_QuineMcCluskeyAlgorithm()
        {
            var terms = new List<int> { 0, 1, 2, 4, 5, 6, 8, 9, 10, 12, 13, 14 };

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var primeImplicants = evaluator.GetPrimeImplicants(terms, 4);
            stopwatch.Stop();

            Assert.That(primeImplicants.Count, Is.GreaterThan(0));
            Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(5000));
        }
    }
}
