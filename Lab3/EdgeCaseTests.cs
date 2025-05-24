using AOIS_Lab3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOIS.Tests3
{
    [TestFixture]
    public class EdgeCaseTests
    {
        private LogicalExpressionEvaluator evaluator;

        [SetUp]
        public void Setup()
        {
            evaluator = new LogicalExpressionEvaluator();
        }

        [Test]
        public void EdgeCase_SingleVariableExpression()
        {
            string expression = "a";
            var variables = evaluator.GetVariables(expression);
            var truthTable = evaluator.BuildTruthTable(expression, variables);

            Assert.That(variables, Is.EqualTo(new List<string> { "a" }));
            Assert.That(truthTable.Count, Is.EqualTo(2));
            Assert.That(truthTable[0].Item2, Is.False);
            Assert.That(truthTable[1].Item2, Is.True); 
        }

        [Test]
        public void EdgeCase_NegatedSingleVariable()
        {
            string expression = "! a";
            var variables = evaluator.GetVariables(expression);
            var truthTable = evaluator.BuildTruthTable(expression, variables);

            Assert.That(variables, Is.EqualTo(new List<string> { "a" }));
            Assert.That(truthTable.Count, Is.EqualTo(2));
            Assert.That(truthTable[0].Item2, Is.True);
            Assert.That(truthTable[1].Item2, Is.False);
        }

        [Test]
        public void EdgeCase_Tautology()
        {
            string expression = "a | ! a";
            var variables = evaluator.GetVariables(expression);
            var truthTable = evaluator.BuildTruthTable(expression, variables);

            Assert.That(truthTable.All(row => row.Item2), Is.True);
        }

        [Test]
        public void EdgeCase_Contradiction()
        {
            string expression = "a & ! a";
            var variables = evaluator.GetVariables(expression);
            var truthTable = evaluator.BuildTruthTable(expression, variables);

            Assert.That(truthTable.All(row => !row.Item2), Is.True);
        }

        [Test]
        public void EdgeCase_DeepNesting()
        {
            string expression = "( ( ( a & b ) | c ) & ( d | e ) )";

            Assert.DoesNotThrow(() =>
            {
                var variables = evaluator.GetVariables(expression);
                var rpn = evaluator.ToRPN(expression);
                var truthTable = evaluator.BuildTruthTable(expression, variables);

                Assert.That(variables.Count, Is.EqualTo(5));
                Assert.That(truthTable.Count, Is.EqualTo(32));
            });
        }

        [Test]
        public void EdgeCase_AllOperators()
        {
            string expression = "( a & b ) | ( c -> d ) ~ e";

            Assert.DoesNotThrow(() =>
            {
                var variables = evaluator.GetVariables(expression);
                var rpn = evaluator.ToRPN(expression);
                var truthTable = evaluator.BuildTruthTable(expression, variables);

                Assert.That(variables.Count, Is.EqualTo(5));
                Assert.That(truthTable.Count, Is.EqualTo(32));
            });
        }

        [Test]
        public void EdgeCase_ConsecutiveNegations()
        {
            string expression = "! ! a";
            var variables = evaluator.GetVariables(expression);
            var truthTable = evaluator.BuildTruthTable(expression, variables);

            Assert.That(truthTable[0].Item2, Is.False);
            Assert.That(truthTable[1].Item2, Is.True);
        }

        [Test]
        public void EdgeCase_ComplexImplication()
        {
            string expression = "( a & b ) -> ( c | d )";
            var variables = evaluator.GetVariables(expression);
            var truthTable = evaluator.BuildTruthTable(expression, variables);

            Assert.That(variables.Count, Is.EqualTo(4));
            Assert.That(truthTable.Count, Is.EqualTo(16));

            var falseCase = truthTable.FirstOrDefault(row =>
                row.Item1["a"] && row.Item1["b"] && !row.Item1["c"] && !row.Item1["d"]);
            Assert.That(falseCase.Item2, Is.False);
        }

        [Test]
        public void EdgeCase_EquivalenceSymmetry()
        {
            string expr1 = "a ~ b";
            string expr2 = "b ~ a";

            var variables = new List<string> { "a", "b" };
            var truthTable1 = evaluator.BuildTruthTable(expr1, variables);
            var truthTable2 = evaluator.BuildTruthTable(expr2, variables);

            for (int i = 0; i < truthTable1.Count; i++)
            {
                Assert.That(truthTable1[i].Item2, Is.EqualTo(truthTable2[i].Item2));
            }
        }

        [Test]
        public void EdgeCase_QuineMcCluskey_NoReduction()
        {
            var terms = new List<int> { 0, 3 };
            var primeImplicants = evaluator.GetPrimeImplicants(terms, 2);

            Assert.That(primeImplicants.Count, Is.EqualTo(2));
            Assert.That(primeImplicants.Any(p => p.Pattern == "00"), Is.True);
            Assert.That(primeImplicants.Any(p => p.Pattern == "11"), Is.True);
        }

        [Test]
        public void EdgeCase_QuineMcCluskey_MaximalReduction()
        {
            var terms = new List<int> { 0, 1, 4, 5 };
            var primeImplicants = evaluator.GetPrimeImplicants(terms, 3);

            Assert.That(primeImplicants.Count, Is.EqualTo(1));
            Assert.That(primeImplicants[0].Pattern, Is.EqualTo("-0-"));
        }

        [Test]
        public void EdgeCase_KMap_BoundaryValues()
        {
            var truthTable2Var = new List<bool> { true, true, true, true };
            var truthTable3Var = Enumerable.Repeat(true, 8).ToList();
            var truthTable4Var = Enumerable.Repeat(false, 16).ToList();
            var truthTable5Var = Enumerable.Repeat(true, 32).ToList();

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                Assert.DoesNotThrow(() => evaluator.DisplayKMap(truthTable2Var, 2, true));
                Assert.DoesNotThrow(() => evaluator.DisplayKMap(truthTable3Var, 3, true));
                Assert.DoesNotThrow(() => evaluator.DisplayKMap(truthTable4Var, 4, true));
                Assert.DoesNotThrow(() => evaluator.DisplayKMap(truthTable5Var, 5, true));

                string output = sw.ToString();
                Assert.That(output, Is.Not.Empty);
            }
        }

        [Test]
        public void EdgeCase_MinimalExpression_EssentialImplicants()
        {
            var terms = new List<int> { 0, 1, 2, 5 };
            var primeImplicants = evaluator.GetPrimeImplicants(terms, 3);
            var variables = new List<string> { "a", "b", "c" };

            var minimalSOP = evaluator.GetMinimalExpression(primeImplicants, terms, variables, true);
            var minimalPOS = evaluator.GetMinimalExpression(primeImplicants, terms, variables, false);

            Assert.That(minimalSOP, Is.Not.Empty);
            Assert.That(minimalPOS, Is.Not.Empty);
            Assert.That(minimalSOP, Is.Not.EqualTo(minimalPOS));
        }

        [Test]
        public void EdgeCase_RPNConversion_OperatorPrecedence()
        {
            string expression = "a | b & c";
            var rpn = evaluator.ToRPN(expression);

            Assert.That(rpn, Is.EqualTo(new List<string> { "a", "b", "c", "&", "|" }));
        }

        [Test]
        public void EdgeCase_RPNConversion_RightAssociative()
        {
            string expression = "a -> b -> c";
            var rpn = evaluator.ToRPN(expression);

            Assert.That(rpn, Is.EqualTo(new List<string> { "a", "b", "c", "->", "->" }));
        }

        [Test]
        public void EdgeCase_CoverageTable_NoIntersection()
        {
            var primeImplicants = new List<Implicant>
            {
                new Implicant("000", new List<int> { 0 }),
                new Implicant("111", new List<int> { 7 })
            };
            var terms = new List<int> { 1, 2 };
            var variables = new List<string> { "a", "b", "c" };

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                evaluator.PrintCoverageTable(primeImplicants, terms, variables, true);
                string output = sw.ToString();

                Assert.That(output, Contains.Substring("a & b & c"));
            }
        }

        [TearDown]
        public void TearDown()
        {
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        }
    }
}
