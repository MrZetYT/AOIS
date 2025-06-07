using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOIS_Lab2
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
                        ((operators[token].IsLeftAssociative && operators[token].Precedence <= operators[operatorStack.Peek()].Precedence) ||
                        (!operators[token].IsLeftAssociative && operators[token].Precedence < operators[operatorStack.Peek()].Precedence)))
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
                        throw new Exception("Mismatched parentheses");
                    }
                }
                else
                {
                    throw new Exception("Invalid token: " + token);
                }
            }

            while (operatorStack.Count > 0)
            {
                if (operatorStack.Peek() == "(")
                {
                    throw new Exception("Mismatched parentheses");
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
                    bool value = (i & (1 << bitPosition)) != 0;
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

        public string GetPDNF(List<(Dictionary<string, bool>, bool)> truthTable, List<string> variables)
        {
            List<string> minterms = new List<string>();
            foreach (var row in truthTable)
            {
                if (row.Item2)
                {
                    List<string> term = new List<string>();
                    foreach (var var in variables)
                    {
                        if (row.Item1[var])
                            term.Add(var);
                        else
                            term.Add("\u00AC" + var);
                    }
                    minterms.Add("(" + string.Join(" \u2227 ", term) + ")");
                }
            }
            return minterms.Any() ? string.Join(" \u2228 ", minterms) : "0";
        }

        public string GetPCNF(List<(Dictionary<string, bool>, bool)> truthTable, List<string> variables)
        {
            List<string> maxterms = new List<string>();
            foreach (var row in truthTable)
            {
                if (!row.Item2)
                {
                    List<string> term = new List<string>();
                    foreach (var var in variables)
                    {
                        if (row.Item1[var])
                            term.Add("\u00AC" + var);
                        else
                            term.Add(var);
                    }
                    maxterms.Add("(" + string.Join(" \u2228 ", term) + ")");
                }
            }
            return maxterms.Any() ? string.Join(" \u2227 ", maxterms) : "1";
        }

        public List<int> GetPDNFNumerical(List<(Dictionary<string, bool>, bool)> truthTable, List<string> variables)
        {
            List<int> indices = new List<int>();
            for (int i = 0; i < truthTable.Count; i++)
            {
                if (truthTable[i].Item2)
                    indices.Add(i);
            }
            return indices;
        }

        public List<int> GetPCNFNumerical(List<(Dictionary<string, bool>, bool)> truthTable, List<string> variables)
        {
            List<int> indices = new List<int>();
            for (int i = 0; i < truthTable.Count; i++)
            {
                if (!truthTable[i].Item2)
                    indices.Add(i);
            }
            return indices;
        }

        public (uint index, string binary) GetIndexForm(List<(Dictionary<string, bool>, bool)> truthTable)
        {
            string binary = string.Join("", truthTable.Select(row => row.Item2 ? "1" : "0"));
            uint index = Convert.ToUInt32(binary, 2);
            return (index, binary);
        }
    }
}