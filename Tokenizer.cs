using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Minuto
{
    public class Tokenizer
    {
        private Regex[] _regexArr;

        private IList<string> _stopWords;

        public Tokenizer()
        {
            this._regexArr = new Regex[] {
                    // new Regex("(\\p{L}+[\\d]+)|([\\d]+\\p{L}+)", RegexOptions.IgnoreCase|RegexOptions.Multiline),
                    // new Regex("(?:R*\\p{Sc}\\p{Z}*)*(?:\\d+(?:[\\.,:]\\d)*)+", RegexOptions.IgnoreCase|RegexOptions.Multiline),
                    // new Regex("\\p{L}+-\\p{L}+", RegexOptions.IgnoreCase|RegexOptions.Multiline),
                    new Regex("[\\w]{2,}", RegexOptions.IgnoreCase|RegexOptions.Multiline)
                };
            this._stopWords = new List<string>(new[] { "a", "à", "ao", "aos", "aquela", "aquelas", "aquele", "aqueles", "aquilo", "as", "às", "até", "com", "como", "da", "das", "de", "dela", "delas", "dele", "deles", "depois", "do", "dos", "e", "ela", "elas", "ele", "eles", "em", "entre", "essa", "essas", "esse", "esses", "esta", "estas", "este", "estes", "eu", "isso", "isto", "já", "lhe", "lhes", "mais", "mas", "me", "mesmo", "meu", "meus", "minha", "minhas", "muito", "muitos", "na", "não", "nas", "nem", "no", "nos", "nós", "nossa", "nossas", "nosso", "nossos", "num", "nuns", "numa", "numas", "o", "os", "ou", "para", "pela", "pelas", "pelo", "pelos", "por", "quais", "qual", "quando", "que", "quem", "se", "sem", "seu", "seus", "só", "sua", "suas", "também", "te", "teu", "teus", "tu", "tua", "tuas", "um", "uma", "umas", "você", "vocês", "vos", "vosso", "vossos", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "_" });
        }
        public List<string> ExtractTokens(string input, int ngram = 1)
        {
            //input = Sanatize(input);

            var tokens = new List<string>();
            string bla = "";
            while (input.Length > 0)
            {
                var lastPosition = input.Length;
                Match firstMatch = null;
                foreach (var re in _regexArr)
                {
                    var match = re.Match(input);
                    if (match.Success && lastPosition > match.Index)
                    {
                        lastPosition = match.Index;
                        firstMatch = match;
                    }
                }
                if (firstMatch == null)
                {
                    bla += input;
                    break;
                }
                else
                {
                    bla += input.Substring(0, firstMatch.Index);
                    input = input.Substring(firstMatch.Index + firstMatch.Length);
                    if (!this._stopWords.Contains(firstMatch.Value.ToLower()))
                    {
                        tokens.Add(firstMatch.Value.ToLower());
                    }
                }
            }

            return ngram > 1 ? NGram(tokens, ngram) : tokens;


            // if (bla.Length != input.Length)
            //     yield return bla;
        }

        private string Sanatize(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public void AddStopWord(string word)
        {
            _stopWords.Add(word);
        }

        private List<string> NGram(IEnumerable<string> tokens, int v)
        {
            var origin = tokens.ToList();
            var list = new List<string>();

            for (int n = 1; n <= v; n++)
            {
                for (int i = 0; i < origin.Count(); i++)
                {
                    var gram = string.Join(" ", origin.Skip(i).Take(n));
                    list.Add(gram);
                }
            }

            return list;
        }
    }
}