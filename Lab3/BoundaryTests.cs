using AOIS_Lab3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOIS.Tests3
{
    [TestFixture]
    public class BoundaryTests
    {
        private LogicalExpressionEvaluator evaluator;

        [SetUp]
        public void Setup()
        {
            evaluator = new LogicalExpressionEvaluator();
        }

        [Test]
        public void Boundary_EmptyExpression_ThrowsException()
        {
            Assert.Throws<Exception>(() => evaluator.ToRPN(""));
        }

        [Test]
        public void Boundary_OnlySpaces_ReturnsEmpty()
        {
            var variables = evaluator.GetVariables("   ");
            Assert.That(variables, Is.Empty);
        }

        [Test]
        public void Boundary_MaxVariables()
        {
            string expression = "a & b & c & d & e";
            var variables = evaluator.GetVariables(expression);
            var combinations = evaluator.GenerateCombinations(variables);

            Assert.That(variables.Count, Is.EqualTo(5));
            Assert.That(combinations.Count, Is.EqualTo(32));
        }

        [Test]
        public void Boundary_MinVariables()
        {
            string expression = "a";
            var variables = evaluator.GetVariables(expression);
            var combinations = evaluator.GenerateCombinations(variables);

            Assert.That(variables.Count, Is.EqualTo(1));
            Assert.That(combinations.Count, Is.EqualTo(2));
        }

        [Test]
        public void Boundary_AllZeroTruthTable()
        {
            string expression = "a & ! a";
            var variables = evaluator.GetVariables(expression);
            var truthTable = evaluator.BuildTruthTable(expression, variables);
            var maxterms = truthTable.Select((row, index) => new { Index = index, Result = row.Item2 })
                                    .Where(x => !x.Result)
                                    .Select(x => x.Index)
                                    .ToList();

            Assert.That(maxterms.Count, Is.EqualTo(2));

            var primeImplicants = evaluator.GetPrimeImplicants(new List<int>(), variables.Count);
            var minimalExpression = evaluator.GetMinimalExpression(primeImplicants, new List<int>(), variables, true);

            Assert.That(minimalExpression, Is.EqualTo("0"));
        }

        [Test]
        public void Boundary_AllOneTruthTable()
        {
            string expression = "a | ! a";
            var variables = evaluator.GetVariables(expression);
            var truthTable = evaluator.BuildTruthTable(expression, variables);
            var minterms = truthTable.Select((row, index) => new { Index = index, Result = row.Item2 })
                                    .Where(x => x.Result)
                                    .Select(x => x.Index)
                                    .ToList();

            Assert.That(minterms.Count, Is.EqualTo(2));

            var primeImplicants = evaluator.GetPrimeImplicants(minterms, variables.Count);
            var minimalExpression = evaluator.GetMinimalExpression(primeImplicants, minterms, variables, true);

            Assert.That(minimalExpression, Is.EqualTo("1"));
        }

        [Test]
        public void Boundary_ExtremeNesting()
        {
            string expression = "( ( ( ( a ) ) ) )";
            var variables = evaluator.GetVariables(expression);
            var rpn = evaluator.ToRPN(expression);
            var truthTable = evaluator.BuildTruthTable(expression, variables);

            Assert.That(variables, Is.EqualTo(new List<string> { "a" }));
            Assert.That(rpn, Is.EqualTo(new List<string> { "a" }));
            Assert.That(truthTable.Count, Is.EqualTo(2));
        }

        [Test]
        public void Boundary_LargeTermSet()
        {
            var terms = Enumerable.Range(0, 15).ToList();
            var primeImplicants = evaluator.GetPrimeImplicants(terms, 4);
            var variables = new List<string> { "a", "b", "c", "d" };

            Assert.DoesNotThrow(() =>
            {
                var minimalExpression = evaluator.GetMinimalExpression(primeImplicants, terms, variables, true);
                Assert.That(minimalExpression, Is.Not.Empty);
            });
        }

        [Test]
        public void Boundary_AlternatingPattern()
        {
            var terms = Enumerable.Range(0, 8).Where(x => x % 2 == 0).ToList();
            var primeImplicants = evaluator.GetPrimeImplicants(terms, 3);

            Assert.That(primeImplicants.Count, Is.EqualTo(1));
            Assert.That(primeImplicants[0].Pattern, Is.EqualTo("--0"));
        }
    }
}
