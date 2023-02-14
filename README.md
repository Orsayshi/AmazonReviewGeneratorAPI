# AmazonReviewGeneratorAPI
Amazon Review Generator API


## Requirements
.Net Core 6 - [Download Here](https://dotnet.microsoft.com/en-us/download)  
Visual Studio 2022 Community - [Download Here](https://visualstudio.microsoft.com/vs/)  
Docker - [Download Here](https://www.docker.com/products/docker-desktop/)

## Description
Amazon Review Generator API that uses nGram model to generate amazon reviews. 
NGrams are generated using ML .NET - [Documentation](https://learn.microsoft.com/en-us/dotnet/api/microsoft.ml.textcatalog.producengrams?view=ml-dotnet)  
NGrams are created in 3s e.g. This | person | is, is | driving | a, a | blue | car.  
Training data can be downloaded from [Here](https://cseweb.ucsd.edu/~jmcauley/datasets/amazon_v2/) althought not necessary because project already contains a data json file. 

*NOTE: Training data only uses the first 5000 lines of the Appliances.json file.*

## Endpoints 
| Request | Endpoint      |
|---------|---------------|
| GET     | /api/generate |

GET /api/generate => generates a random review defaulted to 50 words and returns a `AmazonReviewModel` which contains `Text(Review Text)` and `ReviewScore`.  

`Text` is generated using a Dictionary where the Key is the first word and the value is any array of possible next words.  
`Review Score` is generated by using Sentiment Analysis to determine if the text is positve, neutral or negataive. Sentiment Analysis is done using [VaderSharp2 Nuget package](https://github.com/BobLd/vadersharp)


## Using Docker
1. Open command prompt and navigate to the project directory  
    e.g `C:\User\ProjectFolder\AmazonReviewGeneratorAPI`
2. Run the following command to create a Docker Container  
    `docker build -t {ImageName} -f Dockerfile .`
3. After the container has successfully built, run the following command
    `docker run -d --name {name} -p 8080:80 {ImageName}`
4. To see docker container run the following command  
    `docker ps -a`
5. Open your browser and navigate to http://localhost:8080/api/generate
