using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Text;
using ReviewGenerator.Models;
using System.Security.Cryptography.Xml;

namespace ReviewGenerator.Services
{
    public class MLModel
    {
        public MLModel() { }

        /// <summary>
        /// Transform the data model 
        /// </summary>
        /// <returns>An IDataview of the transformed Data for the MLContext</returns>
        public IDataView TransformData(MLContext context, List<ReviewModel> reviewModel)
        {
            var dataView = this.LoadModel(context, reviewModel);
            var textPipeline = context.Transforms.Text.TokenizeIntoWords("Tokens", "reviewText")
                .Append(context.Transforms.Conversion.MapValueToKey("Tokens"))
                .Append(context.Transforms.Text.ProduceNgrams("NgramFeatures",
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
        private IDataView LoadModel(MLContext context, List<ReviewModel> reviewModel)
        {
            return context.Data.LoadFromEnumerable(reviewModel);
        }
    }
}
