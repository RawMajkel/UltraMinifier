using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UltraMinifier.ConsoleApp
{
    public class Stylesheet
    {
        public string Path { get; private set; }
        public string Name { get; private set; }
        public string RawValue { get; private set; }
        public string MinifiedValue { get; private set; }
        public string UltraMinifiedValue { get; private set; }

        private string _foundRawStyle;
        private List<KeyValuePair<string, string>> _foundMediaQueries = new();

        public Stylesheet(string filePath)
        {
            RawValue = GetRawValue(filePath);
            Minify();
            UltraMinify();
        }

        public void UltraMinify()
        {
            DivideQueriesAndRawStyle();
            JoinMinifiedStyles();
        }

        public void Minify()
        {
            var minified = CssMinifier.MinifyCss(RawValue).Result;
            MinifiedValue = minified.Replace("@media (", "@media(");
        }

        private static string GetRawValue(string filePath)
            => File.Exists(filePath) ? File.ReadAllText(filePath) : string.Empty;

        private void DivideQueriesAndRawStyle()
        {
            var regex = new Regex(@"@media([^{]+)\{([\s\S]+?})\s*}");
            var matches = regex.Matches(MinifiedValue);

            var found = matches.Select(x => x.ToString()).ToList();
            var rest = MinifiedValue;

            foreach (string item in found)
            {
                rest = rest.Replace(item, string.Empty);
            }
            _foundRawStyle = rest;

            foreach (Match match in matches)
            {
                if (match.Groups.Count == 3)
                {
                    _foundMediaQueries.Add(new KeyValuePair<string, string>(match.Groups[1].Value, match.Groups[2].Value));
                }
            }
        }

        private void JoinMinifiedStyles()
        {
            var grouped = _foundMediaQueries
                .GroupBy(x => x.Key)
                .Select(x => new 
                { 
                    x.Key, 
                    Values = x.Select(y => y.Value).ToList() 
                })
                .ToList();

            var stringBuilder = new StringBuilder();
            stringBuilder.Append(_foundRawStyle);

            foreach (var item in grouped)
            {
                stringBuilder.Append("@media");
                stringBuilder.Append(item.Key);
                stringBuilder.Append('{');
                stringBuilder.Append(string.Join(char.MinValue, item.Values));
                stringBuilder.Append('}');
            }

            UltraMinifiedValue = stringBuilder.ToString();
        }
    }
}
