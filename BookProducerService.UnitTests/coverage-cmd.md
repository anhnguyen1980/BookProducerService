remove-item ..\CoverageResults\coverage.json

dotnet test /p:CollectCoverage=true /p:CoverletOutput=../CoverageResults/ /p:Exclude=[*]BookProducer.Core.Entities.*%2c[*]BookProducerService.Infrastructure.Database.*%2c[BookProducerService.Infrastructure.]*%2c[*]BookProducerService.Migrations.*%2c[*]BookProducerService.Models.*  /p:MergeWith=../CoverageResults/coverage.json /p:CoverletOutputFormat=opencover%2cjson -m:1

dotnet tool install -g dotnet-reportgenerator-globaltool //run once
reportgenerator -reports:.\CoverageResults\coverage.opencover.xml -targetdir:.\CoverageResults