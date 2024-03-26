using App.Scripts.Scenes.SceneCalculator.Features.Calculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Assets.App.Scripts.Scenes.SceneCalculator.Features.Calculator
{
    public class MathExpressionEvaluator
    {
        public int Evaluate(string expression)
        {
            var processedExpression = expression.Replace(" ", "");
            var parser = new RpnExpressionParser();
            var rpnExpression = parser.GetRPN(processedExpression);
            var evaluator = new RpnEvaluator();
            return evaluator.EvaluateRPN(rpnExpression);
        }
    }
}
