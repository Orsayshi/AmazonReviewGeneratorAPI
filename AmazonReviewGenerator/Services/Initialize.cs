using Microsoft.ML;
using ReviewGenerator.Models;
using ReviewGenerator.Services;

namespace AmazonReviewGenerator.Services
{
    public class Initialize
    {
        private CreateDataModelFromJson _json;
        private MLModel _mlModel;
        private CreateWordPossibilities _wordPossibilities;

        public Initialize(CreateDataModelFromJson json, MLModel mlModel, CreateWordPossibilities wordPossibilities)
        {
            _json = json;
            _mlModel = mlModel;
            _wordPossibilities = wordPossibilities;
        }

        public IDataView Init(MLContext mLContext, string path, int readLines)
        {
            List<ReviewModel> reviews = _json.ConvertJsonReviewTextToReviewModel(path, readLines);
            return _mlModel.TransformData(mLContext, reviews);
        }

        public Dictionary<string, TextPattern> GenerateDictTextPatter(IDataView dataView)
        {
            return _wordPossibilities.CreateDictWithWordPossibities(dataView);
        }
    }
}
