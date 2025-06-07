using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOIS_Lab3;

namespace AOIS_Lab3
{
    public class LogicalExpressionEvaluator
    {
        private struct OperatorInfo
        {
            public int Precedence;
            public bool IsLeftAssociative;
        }

        private readonly Dictionary<string, OperatorInfo> operators = new Dictionary<string, OperatorInfo>
    {
        { "!", new OperatorInfo { Precedence = 4, IsLeftAssociative = false } },
        { "&", new OperatorInfo { Precedence = 3, IsLeftAssociative = true } },
        { "|", new OperatorInfo { Precedence = 2, IsLeftAssociative = true } },
        { "->", new OperatorInfo { Precedence = 1, IsLeftAssociative = false } },
        { "~", new OperatorInfo { Precedence = 0, IsLeftAssociative = false } }
    };

        private bool IsVariable(string token)
        {
            return token.Length == 1 && token[0] >= 'a' && token[0] <= 'e';
        }

        private bool IsOperator(string token)
        {
            return operators.ContainsKey(token);
        }

        public List<string> GetVariables(string expression)
        {
            var tokens = expression.Split(' ');
            var variables = new HashSet<string>();
            foreach (var token in tokens)
            {
                if (IsVariable(token))
                {
                    variables.Add(token);
                }
            }
            return variables.OrderBy(v => v).ToList();
        }

        public List<string> ToRPN(string expression)
        {
            var tokens = expression.Split(' ');
            var output = new List<string>();
            var operatorStack = new Stack<string>();

            foreach (var token in tokens)
            {
                if (IsVariable(token))
                {
                    output.Add(token);
                }
                else if (IsOperator(token))
                {
                    while (operatorStack.Count > 0 && IsOperator(operatorStack.Peek()) &&
                        (operators[token].IsLeftAssociative && operators[token].Precedence <= operators[operatorStack.Peek()].Precedence ||
                        !operators[token].IsLeftAssociative && operators[token].Precedence < operators[operatorStack.Peek()].Precedence))
                    {
                        output.Add(operatorStack.Pop());
                    }
                    operatorStack.Push(token);
                }
                else if (token == "(")
                {
                    operatorStack.Push(token);
                }
                else if (token == ")")
                {
                    while (operatorStack.Count > 0 && operatorStack.Peek() != "(")
                    {
                        output.Add(operatorStack.Pop());
                    }
                    if (operatorStack.Count > 0 && operatorStack.Peek() == "(")
                    {
                        operatorStack.Pop();
                    }
                    else
                    {
                        throw new Exception("Несбалансированные скобки");
                    }
                }
                else
                {
                    throw new Exception("Недопустимый токен: " + token);
                }
            }

            while (operatorStack.Count > 0)
            {
                if (operatorStack.Peek() == "(")
                {
                    throw new Exception("Несбалансированные скобки");
                }
                output.Add(operatorStack.Pop());
            }

            return output;
        }

        public bool EvaluateRPN(List<string> rpn, Dictionary<string, bool> values)
        {
            Stack<bool> stack = new Stack<bool>();
            foreach (var token in rpn)
            {
                if (IsVariable(token))
                {
                    stack.Push(values[token]);
                }
                else if (token == "!")
                {
                    bool operand = stack.Pop();
                    stack.Push(!operand);
                }
                else if (token == "&")
                {
                    bool right = stack.Pop();
                    bool left = stack.Pop();
                    stack.Push(left && right);
                }
                else if (token == "|")
                {
                    bool right = stack.Pop();
                    bool left = stack.Pop();
                    stack.Push(left || right);
                }
                else if (token == "->")
                {
                    bool right = stack.Pop();
                    bool left = stack.Pop();
                    stack.Push(!left || right);
                }
                else if (token == "~")
                {
                    bool right = stack.Pop();
                    bool left = stack.Pop();
                    stack.Push(left == right);
                }
            }
            return stack.Pop();
        }

        public List<Dictionary<string, bool>> GenerateCombinations(List<string> variables)
        {
            int n = variables.Count;
            int combinations = 1 << n;
            List<Dictionary<string, bool>> result = new List<Dictionary<string, bool>>();
            for (int i = 0; i < combinations; i++)
            {
                Dictionary<string, bool> combo = new Dictionary<string, bool>();
                for (int j = 0; j < n; j++)
                {
                    int bitPosition = n - 1 - j;
                    bool value = (i & 1 << bitPosition) != 0;
                    combo[variables[j]] = value;
                }
                result.Add(combo);
            }
            return result;
        }

        public List<(Dictionary<string, bool>, bool)> BuildTruthTable(string expression, List<string> variables)
        {
            List<string> rpn = ToRPN(expression);
            List<Dictionary<string, bool>> combinations = GenerateCombinations(variables);
            List<(Dictionary<string, bool>, bool)> truthTable = new List<(Dictionary<string, bool>, bool)>();
            foreach (var combo in combinations)
            {
                bool result = EvaluateRPN(rpn, combo);
                truthTable.Add((combo, result));
            }
            return truthTable;
        }

        public List<Implicant> GetPrimeImplicants(List<int> terms, int numVariables)
        {
            if (terms.Count == 0)
                return new List<Implicant>();
            if (terms.Count == 1 << numVariables)
                return new List<Implicant> { new Implicant(new string('-', numVariables), terms) };

            List<Implicant> currentImplicants = terms.Select(t =>
                new Implicant(Convert.ToString(t, 2).PadLeft(numVariables, '0'), new List<int> { t })).ToList();

            List<Implicant> allPrimeImplicants = new List<Implicant>();

            while (currentImplicants.Any())
            {
                List<Implicant> nextLevelImplicants = new List<Implicant>();
                HashSet<int> usedIndices = new HashSet<int>();

                for (int i = 0; i < currentImplicants.Count; i++)
                {
                    for (int j = i + 1; j < currentImplicants.Count; j++)
                    {
                        if (currentImplicants[i].CanCombineWith(currentImplicants[j], out string? combinedPattern))
                        {
                            var combinedTerms = currentImplicants[i].CoveredTerms.Union(currentImplicants[j].CoveredTerms).ToList();
                            var newImplicant = new Implicant(combinedPattern, combinedTerms);

                            if (!nextLevelImplicants.Any(imp => imp.Pattern == combinedPattern))
                            {
                                nextLevelImplicants.Add(newImplicant);
                            }

                            usedIndices.Add(i);
                            usedIndices.Add(j);
                        }
                    }
                }

                for (int i = 0; i < currentImplicants.Count; i++)
                {
                    if (!usedIndices.Contains(i))
                    {
                        allPrimeImplicants.Add(currentImplicants[i]);
                    }
                }

                currentImplicants = nextLevelImplicants;
            }

            return allPrimeImplicants;
        }

        public string GetMinimalExpression(List<Implicant> primeImplicants, List<int> terms, List<string> variables, bool isSOP)
        {
            if (terms.Count == 0)
                return isSOP ? "0" : "1";
            if (terms.Count == 1 << variables.Count)
                return isSOP ? "1" : "0";

            if (!primeImplicants.Any())
                return isSOP ? "0" : "1";

            List<Implicant> selectedImplicants = new List<Implicant>();
            HashSet<int> coveredTerms = new HashSet<int>();

            foreach (var term in terms)
            {
                var coveringImplicants = primeImplicants.Where(imp => imp.CoveredTerms.Contains(term)).ToList();
                if (coveringImplicants.Count == 1)
                {
                    var essential = coveringImplicants[0];
                    if (!selectedImplicants.Contains(essential))
                    {
                        selectedImplicants.Add(essential);
                        foreach (var coveredTerm in essential.CoveredTerms.Where(t => terms.Contains(t)))
                        {
                            coveredTerms.Add(coveredTerm);
                        }
                    }
                }
            }

            while (coveredTerms.Count < terms.Count)
            {
                var remainingTerms = terms.Where(t => !coveredTerms.Contains(t)).ToList();
                if (!remainingTerms.Any())
                    break;

                var bestImplicant = primeImplicants
                    .Where(imp => !selectedImplicants.Contains(imp))
                    .Where(imp => imp.CoveredTerms.Any(t => remainingTerms.Contains(t)))
                    .OrderByDescending(imp => imp.CoveredTerms.Count(t => remainingTerms.Contains(t)))
                    .FirstOrDefault();

                if (bestImplicant != null)
                {
                    selectedImplicants.Add(bestImplicant);
                    foreach (var coveredTerm in bestImplicant.CoveredTerms.Where(t => terms.Contains(t)))
                    {
                        coveredTerms.Add(coveredTerm);
                    }
                }
                else
                {
                    break;
                }
            }

            if (!selectedImplicants.Any())
                return isSOP ? "0" : "1";

            List<string> expressionTerms = selectedImplicants.Select(imp => imp.ToExpression(variables, isSOP)).ToList();
            string joinOp = isSOP ? " | " : " & ";
            return string.Join(joinOp, expressionTerms);
        }

        public void PrintCoverageTable(List<Implicant> primeImplicants, List<int> terms, List<string> variables, bool isSOP)
        {
            if (!primeImplicants.Any())
            {
                Console.WriteLine("Нет простых импликантов.");
                return;
            }

            Console.WriteLine("Простой импликант | Покрываемые термины");
            Console.WriteLine("------------------|-------------------");
            foreach (var imp in primeImplicants)
            {
                string covered = string.Join(", ", imp.CoveredTerms.Where(t => terms.Contains(t)));
                Console.WriteLine($"{imp.ToExpression(variables, isSOP),-17} | {covered}");
            }
        }

        public void DisplayKMap(List<bool> truthTable, int numVariables, bool forSOP)
        {
            if (numVariables < 2 || numVariables > 5)
            {
                Console.WriteLine("Карта Карно не поддерживается для данного числа переменных.");
                return;
            }

            if (numVariables == 2)
            {
                Console.WriteLine($"Карта Карно для 2 переменных ({(forSOP ? "СДНФ" : "СКНФ")}):");
                Console.WriteLine("    b");
                Console.WriteLine("a\\  | 0 | 1");
                Console.WriteLine("----+---+---");
                Console.WriteLine($"0   | {(forSOP == truthTable[0] ? "1" : "0")} | {(forSOP == truthTable[1] ? "1" : "0")}");
                Console.WriteLine($"1   | {(forSOP == truthTable[2] ? "1" : "0")} | {(forSOP == truthTable[3] ? "1" : "0")}");
            }
            else if (numVariables == 3)
            {
                Console.WriteLine($"Карта Карно для 3 переменных ({(forSOP ? "СДНФ" : "СКНФ")}):");
                Console.WriteLine("      bc");
                Console.WriteLine("a\\    | 00 | 01 | 11 | 10");
                Console.WriteLine("------+----+----+----+----");
                Console.WriteLine($"0     | {(forSOP == truthTable[0] ? "1" : "0")}  | {(forSOP == truthTable[1] ? "1" : "0")}  | {(forSOP == truthTable[3] ? "1" : "0")}  | {(forSOP == truthTable[2] ? "1" : "0")}");
                Console.WriteLine($"1     | {(forSOP == truthTable[4] ? "1" : "0")}  | {(forSOP == truthTable[5] ? "1" : "0")}  | {(forSOP == truthTable[7] ? "1" : "0")}  | {(forSOP == truthTable[6] ? "1" : "0")}");
            }
            else if (numVariables == 4)
            {
                Console.WriteLine($"Карта Карно для 4 переменных ({(forSOP ? "СДНФ" : "СКНФ")}):");
                Display4VarKMap(truthTable, forSOP);
            }
            else if (numVariables == 5)
            {
                Console.WriteLine($"Карта Карно для 5 переменных, e=0 ({(forSOP ? "СДНФ" : "СКНФ")}):");
                var subTruthTable_e0 = new List<bool>();
                for (int i = 0; i < 16; i++)
                {
                    subTruthTable_e0.Add(truthTable[i]);
                }
                Display4VarKMap(subTruthTable_e0, forSOP);

                Console.WriteLine($"\nКарта Карно для 5 переменных, e=1 ({(forSOP ? "СДНФ" : "СКНФ")}):");
                var subTruthTable_e1 = new List<bool>();
                for (int i = 16; i < 32; i++)
                {
                    subTruthTable_e1.Add(truthTable[i]);
                }
                Display4VarKMap(subTruthTable_e1, forSOP);
            }
        }

        private void Display4VarKMap(List<bool> values, bool forSOP)
        {
            Console.WriteLine("        cd");
            Console.WriteLine("ab\\     | 00 | 01 | 11 | 10");
            Console.WriteLine("--------+----+----+----+----");

            int[] grayOrder = { 0, 1, 3, 2 };
            string[] grayLabels = { "00", "01", "11", "10" };

            for (int r = 0; r < 4; r++)
            {
                Console.Write($"{grayLabels[r]}      |");
                for (int c = 0; c < 4; c++)
                {
                    int index = grayOrder[r] * 4 + grayOrder[c];
                    string value = forSOP == values[index] ? "1" : "0";
                    Console.Write($" {value}  |");
                }
                Console.WriteLine();
            }
        }
    }
}