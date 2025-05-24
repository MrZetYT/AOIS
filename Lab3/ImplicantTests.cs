using AOIS_Lab3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOIS.Tests3
{
    [TestFixture]
    public class ImplicantTests
    {
        [Test]
        public void Constructor_SetsProperties()
        {
            var coveredTerms = new List<int> { 0, 1, 2 };
            var implicant = new Implicant("10-", coveredTerms);

            Assert.That(implicant.Pattern, Is.EqualTo("10-"));
            Assert.That(implicant.CoveredTerms, Is.EqualTo(coveredTerms));
            Assert.That(implicant.IsEssential, Is.False);
        }

        [Test]
        public void CanCombineWith_SameBitsDifferByOne_ReturnsTrue()
        {
            var imp1 = new Implicant("10", new List<int> { 2 });
            var imp2 = new Implicant("11", new List<int> { 3 });

            bool canCombine = imp1.CanCombineWith(imp2, out string combinedPattern);

            Assert.That(canCombine, Is.True);
            Assert.That(combinedPattern, Is.EqualTo("1-"));
        }

        [Test]
        public void CanCombineWith_SameBitsDifferByMoreThanOne_ReturnsFalse()
        {
            var imp1 = new Implicant("10", new List<int> { 2 });
            var imp2 = new Implicant("01", new List<int> { 1 });

            bool canCombine = imp1.CanCombineWith(imp2, out string combinedPattern);

            Assert.That(canCombine, Is.False);
            Assert.That(combinedPattern, Is.Null);
        }

        [Test]
        public void CanCombineWith_SameBits_ReturnsFalse()
        {
            var imp1 = new Implicant("10", new List<int> { 2 });
            var imp2 = new Implicant("10", new List<int> { 2 });

            bool canCombine = imp1.CanCombineWith(imp2, out string combinedPattern);

            Assert.That(canCombine, Is.False);
            Assert.That(combinedPattern, Is.Null);
        }

        [Test]
        public void CanCombineWith_WithDontCarePositions_ReturnsTrue()
        {
            var imp1 = new Implicant("1-0", new List<int> { 4 });
            var imp2 = new Implicant("1-1", new List<int> { 5 });

            bool canCombine = imp1.CanCombineWith(imp2, out string combinedPattern);

            Assert.That(canCombine, Is.True);
            Assert.That(combinedPattern, Is.EqualTo("1--"));
        }

        [Test]
        public void CanCombineWith_MismatchedDontCare_ReturnsFalse()
        {
            var imp1 = new Implicant("1-0", new List<int> { 4 });
            var imp2 = new Implicant("10-", new List<int> { 4 });

            bool canCombine = imp1.CanCombineWith(imp2, out string combinedPattern);

            Assert.That(canCombine, Is.False);
        }

        [Test]
        public void ToExpression_SOP_AllDontCare_ReturnsOne()
        {
            var implicant = new Implicant("---", new List<int>());
            var variables = new List<string> { "a", "b", "c" };

            string expression = implicant.ToExpression(variables, true);

            Assert.That(expression, Is.EqualTo("1"));
        }

        [Test]
        public void ToExpression_POS_AllDontCare_ReturnsZero()
        {
            var implicant = new Implicant("---", new List<int>());
            var variables = new List<string> { "a", "b", "c" };

            string expression = implicant.ToExpression(variables, false);

            Assert.That(expression, Is.EqualTo("0"));
        }

        [Test]
        public void ToExpression_SOP_WithOnesAndZeros()
        {
            var implicant = new Implicant("10-", new List<int>());
            var variables = new List<string> { "a", "b", "c" };

            string expression = implicant.ToExpression(variables, true);

            Assert.That(expression, Is.EqualTo("a & !b"));
        }

        [Test]
        public void ToExpression_POS_WithOnesAndZeros()
        {
            var implicant = new Implicant("10-", new List<int>());
            var variables = new List<string> { "a", "b", "c" };

            string expression = implicant.ToExpression(variables, false);

            Assert.That(expression, Is.EqualTo("!a | b"));
        }

        [Test]
        public void ToExpression_SingleBit()
        {
            var implicant = new Implicant("1", new List<int>());
            var variables = new List<string> { "a" };

            string sopExpression = implicant.ToExpression(variables, true);
            string posExpression = implicant.ToExpression(variables, false);

            Assert.That(sopExpression, Is.EqualTo("a"));
            Assert.That(posExpression, Is.EqualTo("!a"));
        }

        [Test]
        public void ToExpression_NegatedSingleBit()
        {
            var implicant = new Implicant("0", new List<int>());
            var variables = new List<string> { "a" };

            string sopExpression = implicant.ToExpression(variables, true);
            string posExpression = implicant.ToExpression(variables, false);

            Assert.That(sopExpression, Is.EqualTo("!a"));
            Assert.That(posExpression, Is.EqualTo("a"));
        }
    }
}
