# CoOwnershipManager

This project is only for learning purpose about C# ASP.NET.
It might have some weird things because I wanted to try things 
or because i'm not an expert and I need to learn best practices.

It use a Postgres database and ElasticSearch.

A live demo will be available soon...
Note : There is only parisian adresses stored in database.

## Run with `docker-compose`
```
docker-compose build
docker-compose up -d
```
Check docker logs , the SQL script might take some time to load..

Open browser at [http://localhost:8080](http://localhost:8080) when completed.

Elasticsearch indexation is required to search addresses in home page,
open this URL to start it : [https://localhost:8080/api/Search/Elastic](https://localhost:8080/api/Search/Elastic)