using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Assets.App.Scripts.Scenes.SceneCalculator.Features.Calculator
{
    public class ExpressionManager
    {
        private Dictionary<string, string> expressions = new Dictionary<string, string>();

        public void SetExpression(string key, string expression)
        {
            if (expression.Contains(key))
            {
                throw new Exception($"Expression for '{key}' cannot reference itself.");
            }
            expressions[key] = expression;
        }

        public string SubstituteKeys(string key, HashSet<string> visited)
        {
            if (!expressions.ContainsKey(key))
            {
                return key;
            }

            if (!visited.Add(key))
            {
                throw new Exception($"Cyclic dependency detected involving '{key}'.");
            }

            var expression = expressions[key];
            var regex = new Regex(@"\b[a-zA-Z]+\b");
            var matches = regex.Matches(expression);

            foreach (Match match in matches)
            {
                var matchedKey = match.Value;
                var substituted = SubstituteKeys(matchedKey, new HashSet<string>(visited));
                expression = expression.Replace(matchedKey, $"({substituted})");
            }

            return expression;
        }

    }
}
