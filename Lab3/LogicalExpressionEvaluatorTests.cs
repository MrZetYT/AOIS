using AOIS_Lab3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOIS.Tests3
{
    [TestFixture]
    public class LogicalExpressionEvaluatorTests
    {
        private LogicalExpressionEvaluator evaluator;

        [SetUp]
        public void Setup()
        {
            evaluator = new LogicalExpressionEvaluator();
        }

        [Test]
        public void GetVariables_SimpleExpression_ReturnsOrderedVariables()
        {
            string expression = "a & b | c";

            var variables = evaluator.GetVariables(expression);

            Assert.That(variables, Is.EqualTo(new List<string> { "a", "b", "c" }));
        }

        [Test]
        public void GetVariables_DuplicateVariables_ReturnsUniqueOrderedVariables()
        {
            string expression = "a & a | b & b";

            var variables = evaluator.GetVariables(expression);

            Assert.That(variables, Is.EqualTo(new List<string> { "a", "b" }));
        }

        [Test]
        public void GetVariables_NoVariables_ReturnsEmptyList()
        {
            string expression = "1 | 0";

            var variables = evaluator.GetVariables(expression);

            Assert.That(variables, Is.Empty);
        }

        [Test]
        public void ToRPN_SimpleExpression()
        {
            string expression = "a & b";

            var rpn = evaluator.ToRPN(expression);

            Assert.That(rpn, Is.EqualTo(new List<string> { "a", "b", "&" }));
        }

        [Test]
        public void ToRPN_ComplexExpression()
        {
            string expression = "a & b | c";

            var rpn = evaluator.ToRPN(expression);

            Assert.That(rpn, Is.EqualTo(new List<string> { "a", "b", "&", "c", "|" }));
        }

        [Test]
        public void ToRPN_WithParentheses()
        {
            string expression = "( a | b ) & c";

            var rpn = evaluator.ToRPN(expression);

            Assert.That(rpn, Is.EqualTo(new List<string> { "a", "b", "|", "c", "&" }));
        }

        [Test]
        public void ToRPN_WithNegation()
        {
            string expression = "! a & b";

            var rpn = evaluator.ToRPN(expression);

            Assert.That(rpn, Is.EqualTo(new List<string> { "a", "!", "b", "&" }));
        }

        [Test]
        public void ToRPN_WithImplication()
        {
            string expression = "a -> b";

            var rpn = evaluator.ToRPN(expression);

            Assert.That(rpn, Is.EqualTo(new List<string> { "a", "b", "->" }));
        }

        [Test]
        public void ToRPN_WithEquivalence()
        {
            string expression = "a ~ b";

            var rpn = evaluator.ToRPN(expression);

            Assert.That(rpn, Is.EqualTo(new List<string> { "a", "b", "~" }));
        }

        [Test]
        public void ToRPN_UnbalancedParentheses_ThrowsException()
        {
            string expression = "( a & b";

            Assert.Throws<Exception>(() => evaluator.ToRPN(expression));
        }

        [Test]
        public void ToRPN_ExtraClosingParentheses_ThrowsException()
        {
            string expression = "a & b )";

            Assert.Throws<Exception>(() => evaluator.ToRPN(expression));
        }

        [Test]
        public void ToRPN_InvalidToken_ThrowsException()
        {
            string expression = "a & x";

            Assert.Throws<Exception>(() => evaluator.ToRPN(expression));
        }

        [Test]
        public void EvaluateRPN_SimpleAnd()
        {
            var rpn = new List<string> { "a", "b", "&" };
            var values = new Dictionary<string, bool> { { "a", true }, { "b", false } };

            bool result = evaluator.EvaluateRPN(rpn, values);

            Assert.That(result, Is.False);
        }

        [Test]
        public void EvaluateRPN_SimpleOr()
        {
            var rpn = new List<string> { "a", "b", "|" };
            var values = new Dictionary<string, bool> { { "a", true }, { "b", false } };

            bool result = evaluator.EvaluateRPN(rpn, values);

            Assert.That(result, Is.True);
        }

        [Test]
        public void EvaluateRPN_Negation()
        {
            var rpn = new List<string> { "a", "!" };
            var values = new Dictionary<string, bool> { { "a", true } };

            bool result = evaluator.EvaluateRPN(rpn, values);

            Assert.That(result, Is.False);
        }

        [Test]
        public void EvaluateRPN_Implication()
        {
            var rpn = new List<string> { "a", "b", "->" };
            var values = new Dictionary<string, bool> { { "a", true }, { "b", false } };

            bool result = evaluator.EvaluateRPN(rpn, values);

            Assert.That(result, Is.False);
        }

        [Test]
        public void EvaluateRPN_Equivalence()
        {
            var rpn = new List<string> { "a", "b", "~" };
            var values = new Dictionary<string, bool> { { "a", true }, { "b", true } };

            bool result = evaluator.EvaluateRPN(rpn, values);

            Assert.That(result, Is.True);
        }

        [Test]
        public void GenerateCombinations_TwoVariables()
        {
            var variables = new List<string> { "a", "b" };

            var combinations = evaluator.GenerateCombinations(variables);

            Assert.That(combinations.Count, Is.EqualTo(4));
            Assert.That(combinations[0]["a"], Is.False);
            Assert.That(combinations[0]["b"], Is.False);
            Assert.That(combinations[1]["a"], Is.False);
            Assert.That(combinations[1]["b"], Is.True);
            Assert.That(combinations[2]["a"], Is.True);
            Assert.That(combinations[2]["b"], Is.False);
            Assert.That(combinations[3]["a"], Is.True);
            Assert.That(combinations[3]["b"], Is.True);
        }

        [Test]
        public void GenerateCombinations_SingleVariable()
        {
            var variables = new List<string> { "a" };

            var combinations = evaluator.GenerateCombinations(variables);

            Assert.That(combinations.Count, Is.EqualTo(2));
            Assert.That(combinations[0]["a"], Is.False);
            Assert.That(combinations[1]["a"], Is.True);
        }

        [Test]
        public void GenerateCombinations_EmptyVariables()
        {
            var variables = new List<string>();

            var combinations = evaluator.GenerateCombinations(variables);

            Assert.That(combinations.Count, Is.EqualTo(1));
            Assert.That(combinations[0], Is.Empty);
        }

        [Test]
        public void BuildTruthTable_SimpleExpression()
        {
            string expression = "a & b";
            var variables = new List<string> { "a", "b" };

            var truthTable = evaluator.BuildTruthTable(expression, variables);

            Assert.That(truthTable.Count, Is.EqualTo(4));
            Assert.That(truthTable[0].Item2, Is.False);
            Assert.That(truthTable[1].Item2, Is.False);
            Assert.That(truthTable[2].Item2, Is.False);
            Assert.That(truthTable[3].Item2, Is.True);
        }

        [Test]
        public void GetPrimeImplicants_EmptyTerms_ReturnsEmpty()
        {
            var terms = new List<int>();

            var primeImplicants = evaluator.GetPrimeImplicants(terms, 3);

            Assert.That(primeImplicants, Is.Empty);
        }

        [Test]
        public void GetPrimeImplicants_AllTerms_ReturnsSingleDontCare()
        {
            var terms = new List<int> { 0, 1, 2, 3 };

            var primeImplicants = evaluator.GetPrimeImplicants(terms, 2);

            Assert.That(primeImplicants.Count, Is.EqualTo(1));
            Assert.That(primeImplicants[0].Pattern, Is.EqualTo("--"));
        }

        [Test]
        public void GetPrimeImplicants_SingleTerm()
        {
            var terms = new List<int> { 3 };

            var primeImplicants = evaluator.GetPrimeImplicants(terms, 2);

            Assert.That(primeImplicants.Count, Is.EqualTo(1));
            Assert.That(primeImplicants[0].Pattern, Is.EqualTo("11"));
        }

        [Test]
        public void GetPrimeImplicants_TwoAdjacentTerms()
        {
            var terms = new List<int> { 2, 3 };

            var primeImplicants = evaluator.GetPrimeImplicants(terms, 2);

            Assert.That(primeImplicants.Count, Is.EqualTo(1));
            Assert.That(primeImplicants[0].Pattern, Is.EqualTo("1-"));
        }

        [Test]
        public void GetPrimeImplicants_ComplexCase()
        {
            var terms = new List<int> { 0, 1, 2, 4 };

            var primeImplicants = evaluator.GetPrimeImplicants(terms, 3);

            Assert.That(primeImplicants.Count, Is.GreaterThan(0));
        }

        [Test]
        public void GetMinimalExpression_EmptyTerms_SOP_ReturnsZero()
        {
            var primeImplicants = new List<Implicant>();
            var terms = new List<int>();
            var variables = new List<string> { "a", "b" };

            string result = evaluator.GetMinimalExpression(primeImplicants, terms, variables, true);

            Assert.That(result, Is.EqualTo("0"));
        }

        [Test]
        public void GetMinimalExpression_EmptyTerms_POS_ReturnsOne()
        {
            var primeImplicants = new List<Implicant>();
            var terms = new List<int>();
            var variables = new List<string> { "a", "b" };

            string result = evaluator.GetMinimalExpression(primeImplicants, terms, variables, false);

            Assert.That(result, Is.EqualTo("1"));
        }

        [Test]
        public void GetMinimalExpression_AllTerms_SOP_ReturnsOne()
        {
            var primeImplicants = new List<Implicant> { new Implicant("--", new List<int> { 0, 1, 2, 3 }) };
            var terms = new List<int> { 0, 1, 2, 3 };
            var variables = new List<string> { "a", "b" };

            string result = evaluator.GetMinimalExpression(primeImplicants, terms, variables, true);

            Assert.That(result, Is.EqualTo("1"));
        }

        [Test]
        public void GetMinimalExpression_AllTerms_POS_ReturnsZero()
        {
            var primeImplicants = new List<Implicant> { new Implicant("--", new List<int> { 0, 1, 2, 3 }) };
            var terms = new List<int> { 0, 1, 2, 3 };
            var variables = new List<string> { "a", "b" };

            string result = evaluator.GetMinimalExpression(primeImplicants, terms, variables, false);

            Assert.That(result, Is.EqualTo("0"));
        }

        [Test]
        public void GetMinimalExpression_SingleImplicant()
        {
            var primeImplicants = new List<Implicant> { new Implicant("10", new List<int> { 2 }) };
            var terms = new List<int> { 2 };
            var variables = new List<string> { "a", "b" };

            string sopResult = evaluator.GetMinimalExpression(primeImplicants, terms, variables, true);
            string posResult = evaluator.GetMinimalExpression(primeImplicants, terms, variables, false);

            Assert.That(sopResult, Is.EqualTo("a & !b"));
            Assert.That(posResult, Is.EqualTo("!a | b"));
        }

        [Test]
        public void DisplayKMap_TwoVariables_CapturesOutput()
        {
            var truthTable = new List<bool> { true, false, false, true };

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                evaluator.DisplayKMap(truthTable, 2, true);
                string output = sw.ToString();

                Assert.That(output, Contains.Substring("Карта Карно для 2 переменных"));
            }
        }

        [Test]
        public void DisplayKMap_ThreeVariables_CapturesOutput()
        {
            var truthTable = new List<bool> { true, false, false, true, false, true, true, false };

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                evaluator.DisplayKMap(truthTable, 3, true);
                string output = sw.ToString();

                Assert.That(output, Contains.Substring("Карта Карно для 3 переменных"));
            }
        }

        [Test]
        public void DisplayKMap_UnsupportedVariableCount_ShowsErrorMessage()
        {
            var truthTable = new List<bool> { true };

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                evaluator.DisplayKMap(truthTable, 1, true);
                string output = sw.ToString();

                Assert.That(output, Contains.Substring("Карта Карно не поддерживается"));
            }
        }

        [Test]
        public void PrintCoverageTable_EmptyImplicants_ShowsMessage()
        {
            var primeImplicants = new List<Implicant>();
            var terms = new List<int> { 0, 1 };
            var variables = new List<string> { "a", "b" };

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                evaluator.PrintCoverageTable(primeImplicants, terms, variables, true);
                string output = sw.ToString();

                Assert.That(output, Contains.Substring("Нет простых импликантов"));
            }
        }

        [Test]
        public void PrintCoverageTable_WithImplicants_ShowsTable()
        {
            var primeImplicants = new List<Implicant>
            {
                new Implicant("10", new List<int> { 2 }),
                new Implicant("11", new List<int> { 3 })
            };
            var terms = new List<int> { 2, 3 };
            var variables = new List<string> { "a", "b" };

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                evaluator.PrintCoverageTable(primeImplicants, terms, variables, true);
                string output = sw.ToString();

                Assert.That(output, Contains.Substring("Простой импликант"));
                Assert.That(output, Contains.Substring("Покрываемые термины"));
            }
        }

        [TestCase("a & b", "a", "b", ExpectedResult = true)]
        [TestCase("a | b", "a", "b", ExpectedResult = true)]
        [TestCase("! a", "a", ExpectedResult = false)]
        [TestCase("a -> b", "a", "b", ExpectedResult = true)]
        [TestCase("a ~ b", "a", "b", ExpectedResult = true)]
        public bool IntegrationTest_CompleteEvaluation(string expression, params string[] variableValues)
        {
            var variables = evaluator.GetVariables(expression);
            var values = new Dictionary<string, bool>();

            for (int i = 0; i < variables.Count; i++)
            {
                values[variables[i]] = variableValues[i] == "1" || variableValues[i] == "true" || variableValues[i] == variables[i];
            }

            var rpn = evaluator.ToRPN(expression);
            return evaluator.EvaluateRPN(rpn, values);
        }

        [Test]
        public void IntegrationTest_FullWorkflow_SimpleExpression()
        {
            string expression = "a & b";
            var variables = evaluator.GetVariables(expression);
            var truthTable = evaluator.BuildTruthTable(expression, variables);

            Assert.That(truthTable.Count, Is.EqualTo(4));

            var minterms = truthTable
                .Select((row, index) => new { Index = index, Result = row.Item2 })
                .Where(x => x.Result)
                .Select(x => x.Index)
                .ToList();

            Assert.That(minterms, Is.EqualTo(new List<int> { 3 }));

            var primeImplicants = evaluator.GetPrimeImplicants(minterms, variables.Count);
            Assert.That(primeImplicants.Count, Is.EqualTo(1));
            Assert.That(primeImplicants[0].Pattern, Is.EqualTo("11"));

            var minimalExpression = evaluator.GetMinimalExpression(primeImplicants, minterms, variables, true);
            Assert.That(minimalExpression, Is.EqualTo("a & b"));
        }

        [Test]
        public void StressTest_FiveVariables()
        {
            var variables = new List<string> { "a", "b", "c", "d", "e" };
            var combinations = evaluator.GenerateCombinations(variables);

            Assert.That(combinations.Count, Is.EqualTo(32));

            var uniqueCombinations = new HashSet<string>();
            foreach (var combo in combinations)
            {
                string key = string.Join("", variables.Select(v => combo[v] ? "1" : "0"));
                Assert.That(uniqueCombinations.Add(key), Is.True, $"Duplicate combination: {key}");
            }
        }

        [Test]
        public void EdgeCase_ComplexNesting()
        {
            string expression = "( ( a & b ) | ( c & d ) ) -> ( e | ( ! a & ! b ) )";

            Assert.DoesNotThrow(() =>
            {
                var variables = evaluator.GetVariables(expression);
                var rpn = evaluator.ToRPN(expression);
                var truthTable = evaluator.BuildTruthTable(expression, variables);

                Assert.That(variables.Count, Is.EqualTo(5));
                Assert.That(truthTable.Count, Is.EqualTo(32));
            });
        }

        [TearDown]
        public void TearDown()
        {
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        }
    }
}
