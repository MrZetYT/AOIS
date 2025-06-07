using System;
using System.Collections.Generic;
using System.Linq;

namespace AOIS_Lab3
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("Введите логическое выражение с пробелами между токенами, например, ( a | b ) & ! c");
            string? expression = Console.ReadLine();

            try
            {
                LogicalExpressionEvaluator evaluator = new LogicalExpressionEvaluator();
                var variables = evaluator.GetVariables(expression);
                int numVariables = variables.Count;
                if (numVariables > 5)
                {
                    Console.WriteLine("Слишком много переменных. Максимум 5.");
                    return;
                }

                var truthTable = evaluator.BuildTruthTable(expression, variables);
                Console.WriteLine("Таблица истинности:");
                string headers = string.Join(" ", variables) + " | F";
                Console.WriteLine(headers);
                for (int i = 0; i < truthTable.Count; i++)
                {
                    var row = truthTable[i];
                    string values = string.Join(" ", variables.Select(v => row.Item1[v] ? "1" : "0"));
                    string result = row.Item2 ? "1" : "0";
                    Console.WriteLine(values + " | " + result);
                }

                List<bool> truthValues = truthTable.Select(t => t.Item2).ToList();
                List<int> minterms = new List<int>();
                List<int> maxterms = new List<int>();
                for (int i = 0; i < truthValues.Count; i++)
                {
                    if (truthValues[i])
                        minterms.Add(i);
                    else
                        maxterms.Add(i);
                }

                Console.WriteLine("\nМинимизация СДНФ (SOP):");

                Console.WriteLine("\nРасчетный метод:");
                var sopPrimeImplicants = evaluator.GetPrimeImplicants(minterms, numVariables);
                Console.WriteLine("Простые импликанты: " + string.Join(", ", sopPrimeImplicants.Select(imp => imp.ToExpression(variables, true))));
                var minimalSOP = evaluator.GetMinimalExpression(sopPrimeImplicants, minterms, variables, true);
                Console.WriteLine("Минимизированное выражение: " + minimalSOP);

                Console.WriteLine("\nРасчетно-табличный метод:");
                evaluator.PrintCoverageTable(sopPrimeImplicants, minterms, variables, true);
                Console.WriteLine("Минимизированное выражение: " + minimalSOP);

                Console.WriteLine("\nТабличный метод (Карта Карно):");
                evaluator.DisplayKMap(truthValues, numVariables, true);
                Console.WriteLine("Минимизированное выражение: " + minimalSOP);

                Console.WriteLine("\nМинимизация СКНФ (POS):");

                Console.WriteLine("\nРасчетный метод:");
                var posPrimeImplicants = evaluator.GetPrimeImplicants(maxterms, numVariables);
                Console.WriteLine("Простые импликаты: " + string.Join(", ", posPrimeImplicants.Select(imp => imp.ToExpression(variables, false))));
                var minimalPOS = evaluator.GetMinimalExpression(posPrimeImplicants, maxterms, variables, false);
                Console.WriteLine("Минимизированное выражение: " + minimalPOS);

                Console.WriteLine("\nРасчетно-табличный метод:");
                evaluator.PrintCoverageTable(posPrimeImplicants, maxterms, variables, false);
                Console.WriteLine("Минимизированное выражение: " + minimalPOS);

                Console.WriteLine("\nТабличный метод (Карта Карно):");
                evaluator.DisplayKMap(truthValues.Select(v => !v).ToList(), numVariables, false);
                Console.WriteLine("Минимизированное выражение: " + minimalPOS);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }
    }
}