using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOIS_Lab2;

namespace AOIS.Tests2
{
    [TestFixture]
    public class GenerateCombinationsTests
    {
        private LogicalExpressionEvaluator _evaluator;

        [SetUp]
        public void SetUp()
        {
            _evaluator = new LogicalExpressionEvaluator();
        }

        [Test]
        public void GenerateCombinations_TwoVariables_ReturnsAllCombinations()
        {
            List<string> variables = new List<string> { "a", "b" };
            var result = _evaluator.GenerateCombinations(variables);
            var expected = new List<Dictionary<string, bool>>
            {
                new Dictionary<string, bool> { { "a", false }, { "b", false } },
                new Dictionary<string, bool> { { "a", false }, { "b", true } },
                new Dictionary<string, bool> { { "a", true }, { "b", false } },
                new Dictionary<string, bool> { { "a", true }, { "b", true } }
            };
            Assert.That(result.Count, Is.EqualTo(expected.Count));
            for (int i = 0; i < expected.Count; i++)
            {
                CollectionAssert.AreEqual(expected[i], result[i]);
            }
        }

        [Test]
        public void GenerateCombinations_NoVariables_ReturnsEmptyCombination()
        {
            List<string> variables = new List<string>();
            var result = _evaluator.GenerateCombinations(variables);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Count, Is.EqualTo(0));
        }
    }
}
