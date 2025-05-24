using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOIS_Lab3;

namespace AOIS.Tests3
{
    [TestFixture]
    public class ProgramTests
    {
        [Test]
        public void Main_WithValidInput_CompletesSuccessfully()
        {
            string input = "a & b\n";
            using (var sr = new StringReader(input))
            using (var sw = new StringWriter())
            {
                Console.SetIn(sr);
                Console.SetOut(sw);

                Assert.DoesNotThrow(() => Program.Main());

                string output = sw.ToString();
                Assert.That(output, Contains.Substring("Таблица истинности"));
                Assert.That(output, Contains.Substring("Минимизация СДНФ"));
                Assert.That(output, Contains.Substring("Минимизация СКНФ"));
            }
        }

        [Test]
        public void Main_WithTooManyVariables_ShowsError()
        {
            string input = "a & b & c & d & e & f\n";
            using (var sr = new StringReader(input))
            using (var sw = new StringWriter())
            {
                Console.SetIn(sr);
                Console.SetOut(sw);

                Assert.DoesNotThrow(() => Program.Main());

                string output = sw.ToString();
                Assert.That(output, Contains.Substring("Ошибка: Недопустимый токен: f"));
            }
        }

        [Test]
        public void Main_WithInvalidExpression_ShowsError()
        {
            string input = "a & & b\n";
            using (var sr = new StringReader(input))
            using (var sw = new StringWriter())
            {
                Console.SetIn(sr);
                Console.SetOut(sw);

                Assert.DoesNotThrow(() => Program.Main());

                string output = sw.ToString();
                Assert.That(output, Contains.Substring("Ошибка"));
            }
        }

        [TearDown]
        public void TearDown()
        {
            Console.SetIn(new StreamReader(Console.OpenStandardInput()));
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        }
    }
}
