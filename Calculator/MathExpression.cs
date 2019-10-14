using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Calculator
{
    public class MathExpression
    {
        public const string WRONG_STRING_FORMAT = "Wrong string format";

        private const string MATH_SYMBOLS = @"[(|)|+|\-|*|/]";
        private const string REPEATING_SYMBOLS = @"[+|\-|*|/]{2}";
        private const string ALLOWED_SYMBOLS = @"[\d|,|(|)|+|\-|*|/]";
        private const string NUMBER = @"\d+,?\d*";

        private readonly string expression;
        public bool Validated => expression != null;
        public MathExpression(string rawString)
        {
            expression = CheckString(rawString) ? rawString : null;
        }
        ///<summary>
        ///Method determining whether a string is a mathematical expression
        ///</summary>
        private static bool CheckString(string expression)
        {
            Regex useAllowedSymbols = new Regex(ALLOWED_SYMBOLS + "{" + $"{expression.Length}" + "}");
            Regex repeatSymbols = new Regex(REPEATING_SYMBOLS);
            if (useAllowedSymbols.IsMatch(expression)
                && !repeatSymbols.IsMatch(expression)
                && !expression.Contains("()"))
            {
                return true;
            }
            else
            {
                Console.WriteLine(WRONG_STRING_FORMAT);
                return false;
            }
        }

        ///<summary>
        ///Method returning queue of all numbers in expression
        ///</summary>
        public bool TryGetNumbers(out Queue<float> numbers)
        {
            numbers = new Queue<float>();
            Regex regex = new Regex(MATH_SYMBOLS);
            foreach (string number in regex.Split(expression))
            {
                if (number.Length != 0)
                {
                    if (float.TryParse(number, out float parsedValue))
                    {
                        numbers.Enqueue(parsedValue);
                    }
                    else
                    {
                        Console.WriteLine(WRONG_STRING_FORMAT);
                        return false;
                    }
                }
            }
            return true;
        }
        ///<summary>
        ///Method replases all numbers by "N" and adds notation marker "#" at the end and beginning 
        ///</summary>
        public string TransformToPolishNotation()
        {
            Regex x = new Regex(NUMBER);
            return "#" + x.Replace(expression, "N") + "#";
        }
    }
}
