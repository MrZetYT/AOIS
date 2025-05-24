using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOIS_Lab2;

namespace AOIS.Tests2
{
    [TestFixture]
    public class BuildTruthTable
    {
        private LogicalExpressionEvaluator _evaluator;

        [SetUp]
        public void SetUp()
        {
            _evaluator = new LogicalExpressionEvaluator();
        }
        [Test]
        public void BuildTruthTable_AndExpression_ReturnsCorrectTruthTable()
        {
            string expression = "a & b";
            List<string> variables = new List<string> { "a", "b" };
            var result = _evaluator.BuildTruthTable(expression, variables);
            var expected = new List<(Dictionary<string, bool>, bool)>
            {
                (new Dictionary<string, bool> { { "a", false }, { "b", false } }, false),
                (new Dictionary<string, bool> { { "a", false }, { "b", true } }, false),
                (new Dictionary<string, bool> { { "a", true }, { "b", false } }, false),
                (new Dictionary<string, bool> { { "a", true }, { "b", true } }, true)
            };
            Assert.That(result.Count, Is.EqualTo(expected.Count));
            for (int i = 0; i < expected.Count; i++)
            {
                CollectionAssert.AreEqual(expected[i].Item1, result[i].Item1);
                Assert.That(result[i].Item2, Is.EqualTo(expected[i].Item2));
            }
        }
    }
}
