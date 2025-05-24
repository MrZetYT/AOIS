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
                    newPattern += '-';
                else if (Pattern[i] == '-' || other.Pattern[i] == '-')
                    return false;
                else if (Pattern[i] == other.Pattern[i])
                    newPattern += Pattern[i];
                else
                {
                    diffCount++;
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
            string term = "";
            char op = isSOP ? '&' : '|';
            for (int i = 0; i < Pattern.Length; i++)
            {
                if (Pattern[i] != '-')
                {
                    string var = variables[i];
                    bool negate = isSOP && Pattern[i] == '0' || !isSOP && Pattern[i] == '1';
                    if (negate)
                        term += "!";
                    term += var + " " + op + " ";
                }
            }
            if (term.Length > 0)
                term = term.Substring(0, term.Length - 3);
            return term;
        }
    }

}
