using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDF_Force
{
    internal class DitionaryCreator
    {
        public List<string> GenerateForKeywords(List<string> keywords, int lenght)
        {
            if (keywords is null)
            {
                throw new ArgumentNullException(nameof(keywords));
            }
            if (keywords.Count < 1)
            {
                throw new ArgumentException("Должно быть задано хотя бы одно ключевое слово.");
            }
            List<string> keywordsDictionary = new List<string>();
            Generate_Permutations(lenght, keywords, ref keywordsDictionary);
            return keywordsDictionary;
        }
        public List<string> GenerateForBirthday(DateTime startDate, DateTime finishDate)
        {
            List<string> birthdayDictionary = new List<string>();
            birthdayDictionary.Add(startDate.ToString("ddMMyyyy"));
            while(startDate < finishDate)
            {
                startDate = startDate.AddDays(1);
                birthdayDictionary.Add(startDate.ToString(""));
            }
            return birthdayDictionary;
        }
        public List<string> FirstCharUp(List<string> dictionary)
        {
            if (dictionary is null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }
            if (dictionary.Count < 1)
            {
                throw new ArgumentException("Список не может быть пустым.");
            }
            List<string> result = new List<string>();
            StringBuilder stringBuilder = new StringBuilder();
            result.AddRange(dictionary);
            foreach (var word in dictionary)
            {
                var firstChar = word[0].ToString().ToUpper();
                stringBuilder.Append(firstChar);
                stringBuilder.Append(word.Remove(0, 1));
                result.Add(stringBuilder.ToString());
                stringBuilder.Clear();
            }
            return result;
        }
        private void Generate_Permutations(int n, List<string> keywords, ref List<string> result, List<string> prefix = null)
        {
            prefix = prefix ?? new List<string>();
            if (n == 0)
            {
                result.Add(string.Join("", prefix));
                return;
            }
            foreach (var words in keywords)
            {
                if (prefix.Any(w => w.StartsWith(words)))
                {
                    continue;
                }
                prefix.Add(words);
                Generate_Permutations(n - 1, keywords, ref result, prefix);
                prefix.Remove(words);
            }
        }
    }
}
