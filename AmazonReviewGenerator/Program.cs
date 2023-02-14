using AmazonReviewGenerator.Services;
using Microsoft.ML;
using ReviewGenerator.Models;
using ReviewGenerator.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<CreateDataModelFromJson>();
builder.Services.AddTransient<MLModel>();
builder.Services.AddTransient<CreateWordPossibilities>();
builder.Services.AddTransient<Initialize>();

var app = builder.Build();

var initService = app.Services.GetService<Initialize>();
Dictionary<string, TextPattern> nGramDict = null; 

app.Lifetime.ApplicationStarted.Register(() =>
{
    var dataModel = initService.Init(new MLContext(), @"Data/Appliances.json", 1000);
    nGramDict = initService.GenerateDictTextPatter(dataModel);
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//CreateDataModelFromJson dataModel = new CreateDataModelFromJson();
//var model = dataModel.ConvertJsonReviewTextToReviewModel(@"Data/Appliances.json", 10000);

//MLContext mLContext = new MLContext();
//MLModel m = new MLModel(mLContext, model);
//var nGramFeatures = m.TransformData();

app.MapGet("/api/generate", () =>
{
    return GenerateReview.GenerateNewReview(nGramDict, 50);
});

app.Run();