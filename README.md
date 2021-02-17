# CoOwnershipManager

This project is only for learning purpose about C# ASP.NET


## Run with `docker-compose`

```
docker-compose build
docker-compose up -d
```

Open browser at [http://localhost:8080](http://localhost:8080)


To run elasticsearch indexation : [https://localhost:8080/api/Search/Elastic](https://localhost:8080/api/Search/Elastic)


### Notes
```
dotnet aspnet-codegenerator --configuration "Debug" --project "/Users/raphael/Projects/CoOwnershipManager/CoOwnershipManager/CoOwnershipManager.csproj" controller --model Building --dataContext ApplicationDbContext -name BuildingController --no-build -outDir "/Users/raphael/Projects/CoOwnershipManager/CoOwnershipManager/Controllers" --controllerNamespace CoOwnershipManager.Controllers --restWithNoViews  --useSqlite

```