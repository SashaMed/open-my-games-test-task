using App.Scripts.Scenes.SceneCalculator.Features.Calculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.App.Scripts.Scenes.SceneCalculator.Features.Calculator
{
    public class RpnEvaluator
    {
        public int EvaluateRPN(List<string> expression)
        {
            var numbers = new Stack<int>();
            foreach (var token in expression)
            {
                if (int.TryParse(token, out int number))
                {
                    numbers.Push(number);
                }
                else
                {
                    int rightOperand = numbers.Pop();
                    int leftOperand = numbers.Pop();
                    numbers.Push(CalculateOperation(token, leftOperand, rightOperand));
                }
            }

            if (numbers.Count != 1)
            {
                throw new ExceptionExecuteExpression("The postfix expression is invalid.");
            }

            return numbers.Pop();
        }


        private int CalculateOperation(string token, int leftOperand, int rightOperand)
        {
            switch (token)
            {
                case "+":
                    return (leftOperand + rightOperand);
                case "-":
                    return (leftOperand - rightOperand);
                case "*":
                    return (leftOperand * rightOperand);
                case "/":
                    if (rightOperand == 0)
                    {
                        throw new ExceptionExecuteExpression("Cannot divide by zero.");
                    }
                    return (leftOperand / rightOperand);
                default:
                    throw new ExceptionExecuteExpression($"Unexpected token: {token}");
            }
        }
    }
}
