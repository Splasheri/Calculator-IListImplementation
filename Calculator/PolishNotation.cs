using System;
using System.Collections.Generic;

namespace Calculator
{
    public class PolishNotationCalc
    {
        ///<summary>
        ///Queue with start numbers.
        ///Using for easy reading float variables
        ///</summary>
        private Queue<float> numbers;
        ///<summary>
        ///Queue with operands.
        ///Should be already transformed to polish notation
        ///</summary>
        private Queue<char> operands;
        ///<summary>
        ///Operands reserve using for ordering operations
        ///</summary>
        private readonly Stack<char> reserve;
        ///<summary>
        ///Number reserve using for ordering number operations.        
        ///</summary>
        private readonly Stack<float> numberReserve;
        public PolishNotationCalc()
        {
            numbers = new Queue<float>();
            operands = new Queue<char>();
            reserve = new Stack<char>();
            numberReserve = new Stack<float>();
        }
        public void Solve(MathExpression expression)
        {
            if (expression.Validated)
            {
                if (expression.TryGetNumbers(out numbers))
                {
                    operands = new Queue<char>(expression.TransformToPolishNotation());
                    try
                    {
                        Compute();
                    }
                    catch (Exception ex)
                    {
                        if (ex is System.ArithmeticException)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        if (ex is System.DivideByZeroException)
                        {
                            Console.WriteLine("Divide by zero exception");
                        }
                    }
                }
            }
        }

        ///<summary>
        ///Continiously analyse operands queue; 
        ///</summary>
        private void Compute()
        {
            while (operands.TryDequeue(out char symbol))
            {
                AnalyzeSymbol(symbol);
            }
        }

        private void AnalyzeSymbol(char symbol)
        {
            switch (symbol)
            {
                case '#':
                    NotationMarker();
                    break;
                case 'N':
                    AddNum();
                    break;
                case '(':
                    Startbracket();
                    break;
                case ')':
                    Endbracket();
                    break;
                case '+':
                    LowerOperands(symbol);
                    break;
                case '-':
                    LowerOperands(symbol);
                    break;
                case '*':
                    HighOperands(symbol);
                    break;
                case '/':
                    HighOperands(symbol);
                    break;

            }
        }

        #region OperandFunctions
        ///<summary>
        ///Add number from number queue to number reserve stack
        ///</summary>
        private void AddNum()
        {
            numberReserve.Push(numbers.Dequeue());
        }
        ///<summary>
        ///Complete all operations in brackets
        ///</summary>
        private void Endbracket()
        {
            if (reserve.Peek() == '#')
            {
                throw new ArithmeticException(MathExpression.WRONG_STRING_FORMAT);
            }
            else if (reserve.Peek() != '(')
            {
                ApplyOperation(reserve.Pop());
                Endbracket();
            }
            else
            {
                reserve.Pop();
            }
        }
        ///<summary>
        ///Starts brackets
        ///</summary>
        private void Startbracket()
        {
            reserve.Push('(');
        }
        ///<summary>
        ///Start and end expression marker
        ///</summary>
        private void NotationMarker()
        {
            if (reserve.Count == 0)
            {
                reserve.Push('#');
            }
            else if (reserve.Peek() == '(')
            {
                throw new ArithmeticException(MathExpression.WRONG_STRING_FORMAT);
            }
            else if (reserve.Peek() == '#')
            {
                if (numberReserve.Count == 1)
                {
                    Console.WriteLine(numberReserve.Pop());
                }
                else
                {
                    throw new ArithmeticException(MathExpression.WRONG_STRING_FORMAT);
                }
            }
            else
            {
                ApplyOperation(reserve.Pop());
                NotationMarker();
            }
        }
        ///<summary>
        ///Division and multiply operands. Computing after lower operands
        ///</summary>
        private void HighOperands(char operand)
        {
            if (reserve.Peek() != '#' && reserve.Peek() != '('
             && reserve.Peek() != '+' && reserve.Peek() != '-')
            {
                ApplyOperation(reserve.Pop());
            }
            reserve.Push(operand);
        }
        ///<summary>
        ///Addition and substruction operands. 
        ///</summary>
        private void LowerOperands(char operand)
        {
            if (reserve.Peek() != '#' && reserve.Peek() != '(')
            {
                ApplyOperation(reserve.Pop());
            }
            reserve.Push(operand);
        }
        ///<summary>
        ///Applying operation demands on operand parameter.
        ///Pop two numbers from number reserve stack and then push result into number reserve.
        ///Generates arithmetic exception when reserve contains less then two numbers before computing.
        ///</summary>
        private void ApplyOperation(char operand)
        {
            if (numberReserve.Count >= 2)
            {
                float b = numberReserve.Pop();
                float a = numberReserve.Pop();
                numberReserve.Push(
                    operand switch
                    {
                        '+' => a + b,
                        '-' => a - b,
                        '*' => a * b,
                        '/' => a / b,
                    }
                );
            }
            else
            {
                throw new System.ArithmeticException(MathExpression.WRONG_STRING_FORMAT);
            }
        }
        #endregion
    }
}
