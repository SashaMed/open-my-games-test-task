using Assets.App.Scripts.Scenes.SceneCalculator.Features.Calculator;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace App.Scripts.Scenes.SceneCalculator.Features.Calculator
{
    public class CalculatorExpression : ICalculatorExpression
    {

        private MathExpressionEvaluator evaluator = new MathExpressionEvaluator();
        private ExpressionManager manager = new ExpressionManager();

        public int Execute(string expression)
        {
            var substitutedExpression = manager.SubstituteKeys(expression, new HashSet<string>());
            return evaluator.Evaluate(substitutedExpression);
        }

        public void SetExpression(string expressionKey, string expression)
        {
            manager.SetExpression(expressionKey, expression);
        }

        public int Get(string expressionKey)
        {
            return Execute(expressionKey);
        }
    }
}