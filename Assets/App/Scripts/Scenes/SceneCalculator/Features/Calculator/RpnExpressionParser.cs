using App.Scripts.Scenes.SceneCalculator.Features.Calculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.App.Scripts.Scenes.SceneCalculator.Features.Calculator
{
    public class RpnExpressionParser
    {

        private Dictionary<string, int> priority = new Dictionary<string, int>
                {
                    { "+", 1 }, { "-", 1 },
                    { "*", 2 }, { "/", 2 }
                };

        private void ProcessOperator(Stack<string> operatorStack, List<string> outputQueue, string currentOperator)
        {
            while (operatorStack.Count > 0)
            {
                var topOperator = operatorStack.Peek();
                if (priority.ContainsKey(topOperator) && priority[topOperator] >= priority[currentOperator])
                {
                    outputQueue.Add(operatorStack.Pop().ToString());
                }
                else
                {
                    break;
                }
            }
            operatorStack.Push(currentOperator);
        }

        public List<string> GetRPN(string expression)
        {

            var tokens = Tokenize(expression);
            var result = new List<string>();
            var operatorStack = new Stack<string>();

            foreach (var token in tokens)
            {
                if (int.TryParse(token, out _))
                {
                    result.Add(token);
                }
                else if (token == "(")
                {
                    operatorStack.Push(token);
                }

                else if (token == ")")
                {
                    while (operatorStack.Count > 0)
                    {
                        if (operatorStack.Count == 1 && operatorStack.Peek() != "(")
                        {
                            throw new ExceptionExecuteExpression("The expression contains unbalanced parentheses ().");
                        }
                        var oper = operatorStack.Pop();
                        if (oper == "(")
                        {
                            break;
                        }
                        result.Add(oper);
                    }
                }
                else
                {
                    ProcessOperator(operatorStack, result, token);
                }
            }
            while (operatorStack.Count > 0)
            {
                var token = operatorStack.Pop();
                if (token == "(" || token == ")")
                {
                    throw new ExceptionExecuteExpression("The expression contains unbalanced parentheses ().");
                }
                result.Add(token);
            }
            return result;
        }

        private List<string> Tokenize(string expression)
        {
            var tokens = new List<string>();
            var currentNumber = new StringBuilder();
            string operators = "+-*/()";

            for (int i = 0; i < expression.Length; i++)
            {
                char c = expression[i];

                if (c == '-' && (i == 0 || expression[i - 1] == '('))
                {
                    if (i == 0)
                    {
                        currentNumber.Append(c);
                    }
                    else if (i > 0)
                    {
                        if (expression[i - 1] == '(')
                        {
                            currentNumber.Append(c);
                        }
                    }
                }
                else if (char.IsDigit(c))
                {
                    currentNumber.Append(c);
                }
                else
                {
                    if (currentNumber.Length > 0)
                    {
                        tokens.Add(currentNumber.ToString());
                        currentNumber.Clear();
                    }
                    if (operators.Contains(c.ToString()))
                    {
                        tokens.Add(c.ToString());
                    }

                }
            }

            if (currentNumber.Length > 0)
            {
                tokens.Add(currentNumber.ToString());
            }

            return tokens;
        }
    }
}
