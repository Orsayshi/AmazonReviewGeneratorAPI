using Microsoft.ML;
using ReviewGenerator.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

CreateDataModelFromJson dataModel = new CreateDataModelFromJson();
var model = dataModel.ConvertJsonReviewTextToReviewModel(@"Data/Appliances.json", 10000);

MLContext mLContext = new MLContext();
MLModel m = new MLModel(mLContext, model);
var nGramFeatures = m.TransformData();

var nGramDict = CreateWordPossibilities.CreateDictWithWordPossibities(nGramFeatures);

app.MapGet("/api/generate", () =>
{
    return GenerateReview.GenerateNewReview(nGramDict, 50);
});

app.Run();