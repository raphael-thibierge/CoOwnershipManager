# TO LEARN

- [x] Claims-based authorization
- [x] Role-based authorization
- [x] Policy-based authorization
- [x] Resource-based authorization
- [ ] Best practices for Data Model Vs View Model Vs Basic? Model
- [ ] Policies handler registration : understand difference between `AddSingleton` and `AddScoped`
- [ ] Request validation / Model validation
- [ ] Unit testing
- [ ] Async task and best practices
- [x] Understand differences between core 3.1 vs 5.0 to avoid confusion between versions and namings
- [ ] Env management
- [ ] entity relation and navigation in depth
- [ ] LINQ in depth




aspnet-codegenerator --configuration "Debug" --project "/Users/raphael/Projects/CoOwnershipManager/CoOwnershipManager/CoOwnershipManager.csproj" controller --model Address --dataContext ApplicationDbContext  --referenceScriptLibraries  --useDefaultLayout -name AddressAdminController --no-build -outDir "/Users/raphael/Projects/CoOwnershipManager/CoOwnershipManager/Controllers/AdminControllers" --controllerNamespace CoOwnershipManager.Controllers.AdminControllers --useSqlite

dotnet aspnet-codegenerator --configuration "Debug" --project "/Users/raphael/Projects/CoOwnershipManager/CoOwnershipManager/CoOwnershipManager.csproj" controller --model Apartment --dataContext ApplicationDbContext  --referenceScriptLibraries  --useDefaultLayout -name ApartmentAdminController --no-build -outDir "/Users/raphael/Projects/CoOwnershipManager/CoOwnershipManager/Controllers/AdminControllers" --controllerNamespace CoOwnershipManager.Controllers.AdminControllers --useSqlite


dotnet aspnet-codegenerator --configuration "Debug" --project "/Users/raphael/Projects/CoOwnershipManager/CoOwnershipManager/CoOwnershipManager.csproj" controller --model Building --dataContext ApplicationDbContext  --referenceScriptLibraries  --useDefaultLayout -name BuildingAdminController --no-build -outDir "/Users/raphael/Projects/CoOwnershipManager/CoOwnershipManager/Controllers/AdminControllers" --controllerNamespace CoOwnershipManager.Controllers.AdminControllers --useSqlite
dotnet aspnet-codegenerator --configuration "Debug" --project "/Users/raphael/Projects/CoOwnershipManager/CoOwnershipManager/CoOwnershipManager.csproj" controller --model Post --dataContext ApplicationDbContext  --referenceScriptLibraries  --useDefaultLayout -name PostAdminController --no-build -outDir "/Users/raphael/Projects/CoOwnershipManager/CoOwnershipManager/Controllers/AdminControllers" --controllerNamespace CoOwnershipManager.Controllers.AdminControllers --useSqlite
dotnet aspnet-codegenerator --configuration "Debug" --project "/Users/raphael/Projects/CoOwnershipManager/CoOwnershipManager/CoOwnershipManager.csproj" controller --model ApplicationUser --dataContext ApplicationDbContext  --referenceScriptLibraries  --useDefaultLayout -name ApplicationUserAdminController --no-build -outDir "/Users/raphael/Projects/CoOwnershipManager/CoOwnershipManager/Controllers/AdminControllers" --controllerNamespace CoOwnershipManager.Controllers.AdminControllers --useSqlite
