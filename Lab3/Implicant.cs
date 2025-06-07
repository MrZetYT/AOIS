using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOIS_Lab3;

namespace AOIS_Lab3
{
    public class Implicant
    {
        public string Pattern { get; set; }
        public List<int> CoveredTerms { get; set; }
        public bool IsEssential { get; set; } = false;

        public Implicant(string pattern, List<int> coveredTerms)
        {
            Pattern = pattern;
            CoveredTerms = coveredTerms;
        }

        public bool CanCombineWith(Implicant other, out string? combinedPattern)
        {
            combinedPattern = null;
            int diffCount = 0;
            string newPattern = "";

            for (int i = 0; i < Pattern.Length; i++)
            {
                if (Pattern[i] == '-' && other.Pattern[i] == '-')
                {
                    newPattern += '-';
                }
                else if (Pattern[i] == '-' || other.Pattern[i] == '-')
                {
                    return false;
                }
                else if (Pattern[i] == other.Pattern[i])
                {
                    newPattern += Pattern[i];
                }
                else
                {
                    diffCount++;
                    if (diffCount > 1)
                        return false;
                    newPattern += '-';
                }
            }

            if (diffCount == 1)
            {
                combinedPattern = newPattern;
                return true;
            }
            return false;
        }

        public string ToExpression(List<string> variables, bool isSOP)
        {
            if (Pattern.All(c => c == '-'))
            {
                return isSOP ? "1" : "0";
            }

            List<string> literals = new List<string>();

            for (int i = 0; i < Pattern.Length; i++)
            {
                if (Pattern[i] != '-')
                {
                    string var = variables[i];
                    if (isSOP)
                    {
                        if (Pattern[i] == '0')
                            literals.Add("!" + var);
                        else
                            literals.Add(var);
                    }
                    else
                    {
                        if (Pattern[i] == '0')
                            literals.Add(var);
                        else
                            literals.Add("!" + var);
                    }
                }
            }

            if (!literals.Any())
                return isSOP ? "1" : "0";

            if (isSOP)
            {
                return string.Join(" & ", literals);
            }
            else
            {
                if (literals.Count == 1)
                    return literals[0];
                return "( " + string.Join(" | ", literals) + " )";
            }
        }
    }
}