using System;

namespace Calculator
{
    internal class Program
    {
        private const string READ_EXPRESSION = "Wrong number of console arguments. Input math expression";

        private static void Main(string[] args)
        {
            MathExpression expression =
                args.Length == 1 ?
                new MathExpression(args[0]) :
                new MathExpression(InputExpression());
            PolishNotationCalc calc = new PolishNotationCalc();
            calc.Solve(expression);
        }
        private static string InputExpression()
        {
            Console.WriteLine(READ_EXPRESSION);
            return Console.ReadLine();
        }
    }
}
