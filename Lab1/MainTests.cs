using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOIS_Lab1;

namespace AOIS_Lab1.Tests
{
    [TestFixture]
    public class MainTests
    {
        [Test]
        public void Main_MainMethod()
        {
            var simulatedInput = new StringReader("abc\n1\nabc\n1\n2\nabc\n1\n12\n5\n2\nabc\n2\n12\nabc\n2\n12,4\n3,4\n3\n"); // сначала невалидное, потом валидное
            Console.SetIn(simulatedInput);

            var consoleOutput = new System.IO.StringWriter();
            Console.SetOut(consoleOutput);


            Program.Main();
            StringAssert.Contains("До встречи!", consoleOutput.ToString());
        }
    }
}
