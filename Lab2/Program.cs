using System;
using System.Collections.Generic;
using System.Linq;
using AOIS_Lab2;

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
            if (!variables.Any())
            {
                Console.WriteLine("Ошибка: в выражении отсутствуют переменные.");
                return;
            }

            var truthTable = evaluator.BuildTruthTable(expression, variables);

            Console.WriteLine("Таблица истинности:");
            string headers = string.Join(" ", variables) + " | result";
            Console.WriteLine(headers);
            foreach (var row in truthTable)
            {
                string values = string.Join(" ", variables.Select(v => row.Item1[v] ? "1" : "0"));
                string result = row.Item2 ? "1" : "0";
                Console.WriteLine(values + " | " + result);
            }

            string pdnf = evaluator.GetPDNF(truthTable, variables);
            string pcnf = evaluator.GetPCNF(truthTable, variables);
            List<int> pdnfNumerical = evaluator.GetPDNFNumerical(truthTable, variables);
            List<int> pcnfNumerical = evaluator.GetPCNFNumerical(truthTable, variables);
            var (index, binary) = evaluator.GetIndexForm(truthTable);

            Console.WriteLine($"СДНФ: {pdnf}");
            Console.WriteLine($"СКНФ: {pcnf}");
            Console.WriteLine($"Числовая форма СДНФ: {(pdnfNumerical.Any() ? string.Join(", ", pdnfNumerical) : "пусто")}");
            Console.WriteLine($"Числовая форма СКНФ: {(pcnfNumerical.Any() ? string.Join(", ", pcnfNumerical) : "пусто")}");
            Console.WriteLine($"Индексная форма: {index} - {binary}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}