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

            List<Implicant> implicants = terms.Select(t => new Implicant(Convert.ToString(t, 2).PadLeft(numVariables, '0'), new List<int> { t })).ToList();
            List<Implicant> primeImplicants = new List<Implicant>();
            HashSet<string> usedPatterns = new HashSet<string>();

            while (implicants.Any())
            {
                List<Implicant> nextImplicants = new List<Implicant>();
                HashSet<string> combinedPatterns = new HashSet<string>();

                for (int i = 0; i < implicants.Count; i++)
                {
                    for (int j = i + 1; j < implicants.Count; j++)
                    {
                        if (implicants[i].CanCombineWith(implicants[j], out string? combinedPattern))
                        {
                            if (!combinedPatterns.Contains(combinedPattern))
                            {
                                combinedPatterns.Add(combinedPattern);
                                List<int> covered = implicants[i].CoveredTerms.Union(implicants[j].CoveredTerms).ToList();
                                nextImplicants.Add(new Implicant(combinedPattern, covered));
                            }
                            usedPatterns.Add(implicants[i].Pattern);
                            usedPatterns.Add(implicants[j].Pattern);
                        }
                    }
                }

                primeImplicants.AddRange(implicants.Where(imp => !usedPatterns.Contains(imp.Pattern)));
                implicants = nextImplicants;
            }

            return primeImplicants;
        }

        public string GetMinimalExpression(List<Implicant> primeImplicants, List<int> terms, List<string> variables, bool isSOP)
        {
            if (terms.Count == 0)
                return isSOP ? "0" : "1";
            if (terms.Count == 1 << variables.Count)
                return isSOP ? "1" : "0";

            List<Implicant> selectedImplicants = new List<Implicant>();
            HashSet<int> coveredTerms = new HashSet<int>();

            Dictionary<int, List<Implicant>> termCoverage = new Dictionary<int, List<Implicant>>();
            foreach (var term in terms)
            {
                termCoverage[term] = primeImplicants.Where(imp => imp.CoveredTerms.Contains(term)).ToList();
            }

            foreach (var kvp in termCoverage)
            {
                if (kvp.Value.Count == 1)
                {
                    var essential = kvp.Value[0];
                    if (!selectedImplicants.Contains(essential))
                    {
                        selectedImplicants.Add(essential);
                        coveredTerms.UnionWith(essential.CoveredTerms);
                    }
                }
            }

            while (coveredTerms.Count < terms.Count)
            {
                var remainingTerms = terms.Except(coveredTerms).ToList();
                if (!remainingTerms.Any())
                    break;

                var bestImplicant = primeImplicants
                    .Where(imp => !selectedImplicants.Contains(imp))
                    .OrderByDescending(imp => imp.CoveredTerms.Intersect(remainingTerms).Count())
                    .FirstOrDefault();

                if (bestImplicant != null)
                {
                    selectedImplicants.Add(bestImplicant);
                    coveredTerms.UnionWith(bestImplicant.CoveredTerms);
                }
                else
                {
                    break;
                }
            }

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
                string covered = string.Join(", ", imp.CoveredTerms.Intersect(terms));
                Console.WriteLine($"{imp.ToExpression(variables, isSOP)} | {covered}");
            }
        }

        public void DisplayKMap(List<bool> truthTable, int numVariables, bool forSOP)
        {
            if (numVariables < 2 || numVariables > 5)
            {
                Console.WriteLine("Карта Карно не поддерживается для данного числа переменных.");
                return;
            }

            string[] gray2 = { "00", "01", "11", "10" };

            if (numVariables == 2)
            {
                Console.WriteLine($"Карта Карно для 2 переменных ({(forSOP ? "СДНФ" : "СКНФ")}):");
                Console.WriteLine("  b | 0 | 1");
                Console.WriteLine("a\\");
                Console.WriteLine("0 | " + (forSOP == truthTable[0] ? "1" : "0") + " | " + (forSOP == truthTable[1] ? "1" : "0"));
                Console.WriteLine("1 | " + (forSOP == truthTable[2] ? "1" : "0") + " | " + (forSOP == truthTable[3] ? "1" : "0"));
            }
            else if (numVariables == 3)
            {
                Console.WriteLine($"Карта Карно для 3 переменных ({(forSOP ? "СДНФ" : "СКНФ")}):");
                Console.WriteLine("  ab | 00 | 01 | 11 | 10");
                Console.WriteLine("c\\");
                Console.WriteLine("0 | " + (forSOP == truthTable[0] ? "1" : "0") + " | " + (forSOP == truthTable[2] ? "1" : "0") + " | " + (forSOP == truthTable[6] ? "1" : "0") + " | " + (forSOP == truthTable[4] ? "1" : "0"));
                Console.WriteLine("1 | " + (forSOP == truthTable[1] ? "1" : "0") + " | " + (forSOP == truthTable[3] ? "1" : "0") + " | " + (forSOP == truthTable[7] ? "1" : "0") + " | " + (forSOP == truthTable[5] ? "1" : "0"));
            }
            else if (numVariables == 4)
            {
                Console.WriteLine($"Карта Карно для 4 переменных ({(forSOP ? "СДНФ" : "СКНФ")}):");
                Console.WriteLine("cd\\ab | 00 | 01 | 11 | 10");
                for (int r = 0; r < 4; r++)
                {
                    string rowLabel = gray2[r];
                    Console.Write(rowLabel + " | ");
                    for (int c = 0; c < 4; c++)
                    {
                        int rowBinary = r ^ r >> 1;
                        int colBinary = c ^ c >> 1;
                        int index = rowBinary << 2 | colBinary;
                        string value = forSOP == truthTable[index] ? "1" : "0";
                        Console.Write(value + " | ");
                    }
                    Console.WriteLine();
                }
            }
            else if (numVariables == 5)
            {
                Console.WriteLine($"Карта Карно для 5 переменных, e=0 ({(forSOP ? "СДНФ" : "СКНФ")}):");
                List<bool> subTruthTable_e0 = Enumerable.Range(0, 32).Where(i => (i & 1) == 0).Select(i => truthTable[i]).ToList();
                Display4VarKMap(subTruthTable_e0, forSOP);
                Console.WriteLine($"Карта Карно для 5 переменных, e=1 ({(forSOP ? "СДНФ" : "СКНФ")}):");
                List<bool> subTruthTable_e1 = Enumerable.Range(0, 32).Where(i => (i & 1) == 1).Select(i => truthTable[i]).ToList();
                Display4VarKMap(subTruthTable_e1, forSOP);
            }
        }

        private void Display4VarKMap(List<bool> values, bool forSOP)
        {
            string[] gray = { "00", "01", "11", "10" };
            Console.WriteLine("cd\\ab | 00 | 01 | 11 | 10");
            for (int r = 0; r < 4; r++)
            {
                string rowLabel = gray[r];
                Console.Write(rowLabel + " | ");
                for (int c = 0; c < 4; c++)
                {
                    int rowBinary = r ^ r >> 1;
                    int colBinary = c ^ c >> 1;
                    int index = rowBinary << 2 | colBinary;
                    string value = forSOP == values[index] ? "1" : "0";
                    Console.Write(value + " | ");
                }
                Console.WriteLine();
            }
        }
    }

}
