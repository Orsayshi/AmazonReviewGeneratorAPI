using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Text;
using ReviewGenerator.Models;
using System.Security.Cryptography.Xml;

namespace ReviewGenerator.Services
{
    public class MLModel
    {
        private readonly MLContext _context;
        private List<ReviewModel> _reviewModel;

        public MLModel(MLContext mLContext, List<ReviewModel> reviews) 
        {
            this._context = mLContext;
            this._reviewModel = reviews; 
        }

        /// <summary>
        /// Transform the data model 
        /// </summary>
        /// <returns>An IDataview of the transformed Data for the MLContext</returns>
        public IDataView TransformData()
        {
            var dataView = this.LoadModel();
            var textPipeline = this._context.Transforms.Text.TokenizeIntoWords("Tokens", "reviewText")
                .Append(this._context.Transforms.Conversion.MapValueToKey("Tokens"))
                .Append(this._context.Transforms.Text.ProduceNgrams("NgramFeatures",
                        "Tokens",
                        ngramLength: 3, // The length of the ngram e.g. The | hat | in 
                        useAllLengths: false,
                        weighting: NgramExtractingEstimator.WeightingCriteria.Tf));

            var textTransformer = textPipeline.Fit(dataView);
            var transformedDataView = textTransformer.Transform(dataView);

            return transformedDataView;
        }

        /// <summary>
        /// Loads the data model for the MLContext
        /// </summary>
        /// <returns>IDataView</returns>
        private IDataView LoadModel()
        {
            return this._context.Data.LoadFromEnumerable(this._reviewModel);
        }
    }
}
