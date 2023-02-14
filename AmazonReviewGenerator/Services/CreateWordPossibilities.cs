using Microsoft.ML;
using Microsoft.ML.Data;
using ReviewGenerator.Models;
using System.Linq;

namespace ReviewGenerator.Services
{
    public static class CreateWordPossibilities
    {
        /// <summary>
        /// Generates a Dictionary where the key is a word 
        /// and the values are all the possible words that come after it.
        /// </summary>
        /// <param name="_transformedModel"></param>
        /// <returns>A dictionary with key value pairs for possible words that can be used to generate a review.</returns>
        public static Dictionary<string, TextPattern> CreateDictWithWordPossibities(IDataView _transformedModel)
        {
            Dictionary<string, TextPattern> wordPossibilities = new Dictionary<string, TextPattern>();
            VBuffer<ReadOnlyMemory<char>> slotNames = default;
            var transformedDataView = _transformedModel;
            transformedDataView.Schema["NgramFeatures"].GetSlotNames(ref slotNames);
            var nGramFeatureColumns = transformedDataView.GetColumn<VBuffer<float>>(transformedDataView.Schema["NgramFeatures"]);
            var slots = slotNames.GetValues();
            foreach (var featureRow in nGramFeatureColumns)
            {
                foreach(var item in featureRow.Items())
                {
                    string nGram = slots[item.Key].ToString();
                    var words = nGram.Split('|');
                    if (!wordPossibilities.ContainsKey(words[0]))
                    {
                        wordPossibilities[words[0]] = new TextPattern() { Next = new List<string> { words[1], words[2] } };
                    }
                    else
                    {
                        if (!chkIfKeyValueContainsWord(wordPossibilities, words[0], words[1]))
                        {
                            wordPossibilities[words[0]].Next.Add(words[1]);
                        }
                        if (!chkIfKeyValueContainsWord(wordPossibilities, words[0], words[2]))
                        {
                            wordPossibilities[words[0]].Next.Add(words[2]);
                        }
                    }
                }
            }
            return wordPossibilities;
        }

        /// <summary>
        /// Checks if value already exits for a given key.
        /// </summary>
        /// <param name="words"></param>
        /// <param name="key"></param>
        /// <param name="word"></param>
        /// <returns>True/False based on if value is already in the Keys value array</returns>
        private static bool chkIfKeyValueContainsWord(Dictionary<string, TextPattern> words, string key ,string word)
        {
            if (words[key].Next.Contains(word))
            {
                return true;
            }
            return false;
        }
    }
}
