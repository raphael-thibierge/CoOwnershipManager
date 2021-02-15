dotnet tool install --global dotnet-ef
> dotnet ef migrations add [migration name] 
> dotnet ef database update

 rm app.db* && dotnet ef migrations remove && dotnet ef migrations add initialCommit && dotnet ef database update
