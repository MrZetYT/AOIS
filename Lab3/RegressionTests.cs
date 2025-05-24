using AOIS_Lab3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOIS.Tests3
{
    [TestFixture]
    public class RegressionTests
    {
        private LogicalExpressionEvaluator evaluator;

        [SetUp]
        public void Setup()
        {
            evaluator = new LogicalExpressionEvaluator();
        }

        [Test]
        public void Regression_ImplicationTruthTable()
        {
            string expression = "a -> b";
            var variables = evaluator.GetVariables(expression);
            var truthTable = evaluator.BuildTruthTable(expression, variables);

            Assert.That(truthTable[0].Item2, Is.True);
            Assert.That(truthTable[1].Item2, Is.True);
            Assert.That(truthTable[2].Item2, Is.False);
            Assert.That(truthTable[3].Item2, Is.True);
        }

        [Test]
        public void Regression_EquivalenceTruthTable()
        {
            string expression = "a ~ b";
            var variables = evaluator.GetVariables(expression);
            var truthTable = evaluator.BuildTruthTable(expression, variables);

            Assert.That(truthTable[0].Item2, Is.True);
            Assert.That(truthTable[1].Item2, Is.False);
            Assert.That(truthTable[2].Item2, Is.False);
            Assert.That(truthTable[3].Item2, Is.True);
        }

        [Test]
        public void Regression_ComplexCombination()
        {
            string expression = "( a & b ) | ( ! c -> ( d ~ e ) )";

            Assert.DoesNotThrow(() =>
            {
                var variables = evaluator.GetVariables(expression);
                var truthTable = evaluator.BuildTruthTable(expression, variables);
                var minterms = truthTable.Select((row, index) => new { Index = index, Result = row.Item2 })
                                        .Where(x => x.Result)
                                        .Select(x => x.Index)
                                        .ToList();

                if (minterms.Any())
                {
                    var primeImplicants = evaluator.GetPrimeImplicants(minterms, variables.Count);
                    var minimalExpression = evaluator.GetMinimalExpression(primeImplicants, minterms, variables, true);

                    Assert.That(minimalExpression, Is.Not.Empty);
                }
            });
        }

        [Test]
        public void Regression_DisplayOutput_DoesNotCrash()
        {
            var primeImplicants = new List<Implicant>
            {
                new Implicant("10-", new List<int> { 4, 5 })
            };
            var terms = new List<int> { 4, 5 };
            var variables = new List<string> { "a", "b", "c" };
            var truthTable = new List<bool> { false, true, false, true, true, true, false, false };

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                Assert.DoesNotThrow(() =>
                {
                    evaluator.PrintCoverageTable(primeImplicants, terms, variables, true);
                    evaluator.DisplayKMap(truthTable, 3, true);
                });

                string output = sw.ToString();
                Assert.That(output, Is.Not.Empty);
            }
        }

        [TearDown]
        public void TearDown()
        {
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        }
    }
}
