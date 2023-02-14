using ReviewGenerator.Models;
using System.Text;
using System.Text.Json.Nodes;

namespace ReviewGenerator.Services
{
    public class CreateDataModelFromJson
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="linesToRead"></param>
        /// <returns></returns>
        public List<ReviewModel> ConvertJsonReviewTextToReviewModel(string path, int linesToRead)
        {
            List<ReviewModel> model = new List<ReviewModel>();
            using(var fileStream = File.OpenRead(path))
            using(var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, 128))
            {
                string line; 
                for(int i = 0; i < linesToRead; i++) {
                    line = streamReader.ReadLine();
                    var parsedLine = JsonNode.Parse(line)["reviewText"];
                    if(parsedLine != null)
                    {
                        // Remove all unnecessary chars from text
                        string cleanedText = parsedLine.ToString().Replace(@"[^\w\s]", "");
                        model.Add(new ReviewModel { reviewText= cleanedText });
                    }
                }
                return model;
            }
        }
    }
}
