using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using CoOwnershipManager.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;
using CoOwnershipManager.Authorization;
using Microsoft.AspNetCore.Http;
using React.AspNet;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using JavaScriptEngineSwitcher.ChakraCore;
using CoOwnershipManager.Extensions;

namespace CoOwnershipManager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("LocalPostgresConnection")));


            
            // Configure default ASP.NET Core Identity to use our custom model ApplicationUser
            services.AddDefaultIdentity<ApplicationUser>(
                // disable account confirmation by email
                options => options.SignIn.RequireConfirmedAccount = false)
                // map database context
                .AddEntityFrameworkStores<ApplicationDbContext>()
                // 
                .AddDefaultTokenProviders();


            // React
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddReact();
            // Make sure a JS engine is registered, or you will get an error!
            services.AddJsEngineSwitcher(options => options.DefaultEngineName = ChakraCoreJsEngine.EngineName)
              .AddChakraCore();

            services.AddControllersWithViews();
            services.AddRazorPages();





            // Doc : https://docs.microsoft.com/fr-fr/aspnet/core/security/authorization/introduction?view=aspnetcore-3.1
            // Configure custom authorizations policies
            services.AddAuthorization(options =>
            {
                // fallback policy : user is authenticated
                // Apply authentication required over all app (except login/register)
                //
                //options.FallbackPolicy = new AuthorizationPolicyBuilder()
                //    .RequireAuthenticatedUser()
                //    .Build();

                // IsAdmin policy
                options.AddPolicy("IsAdmin", policy =>
                    policy.AddRequirements(new UserIsAdminRequirements())  
                );
            });

            // Register IsAdmin policy handler
            // TODO : `services.AddScoped` VS `services.AddSingleton`
            services.AddScoped<IAuthorizationHandler, UserIsAdminAuthorizationHandler>();


            // Configure elasticsearch
            services.AddElasticsearch(Configuration);   
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // upgrade to net5
                app.UseMigrationsEndPoint(); 
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();



            // Initialise ReactJS.NET. Must be before static files.
            app.UseReact(config =>
            {
                // If you want to use server-side rendering of React components,
                // add all the necessary JavaScript files here. This includes
                // your components as well as all of their dependencies.
                // See http://reactjs.net/ for more information. Example:
                config
                    .AddScript("~/js/SearchAddress.jsx");
                //    .AddScript("~/js/Second.jsx");

                // If you use an external build too (for example, Babel, Webpack,
                // Browserify or Gulp), you can improve performance by disabling
                // ReactJS.NET's version of Babel and loading the pre-transpiled
                // scripts. Example:
                //config
                //    .SetLoadBabel(false)
                //    .AddScriptWithoutTransform("~/Scripts/bundle.server.js");
            });

            // Allow to sever statics files, stored in wwwroot, such as images, js, css, etc...
            app.UseStaticFiles();

            // https://docs.microsoft.com/fr-fr/aspnet/core/fundamentals/routing?view=aspnetcore-3.1
            // UseRouting adds route matching to the middleware pipeline.
            // This middleware looks at the set of endpoints defined in the app,
            // and selects the best match based on the request.
            app.UseRouting();


            // https://docs.microsoft.com/fr-fr/aspnet/core/security/authentication/?view=aspnetcore-3.1
            // - Call UseAuthentication before any middleware that depends on users being authenticated
            // - After UseRouting, so that route information is available for authentication decisions.
            // - Before UseEndpoints, so that users are authenticated before accessing the endpoints.
            //
            // It enables the authentication middleware, and details are define in ConfigureServices method
            //
            // TODO : Identity Scaffolding https://docs.microsoft.com/fr-fr/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-3.1&tabs=visual-studio#scaffold-identity-into-a-razor-project-with-authorization
            app.UseAuthentication();


            // Enables authorization middleware to use roles and policies
            app.UseAuthorization();

            // https://docs.microsoft.com/fr-fr/aspnet/core/fundamentals/routing?view=aspnetcore-3.1
            // UseEndpoints adds endpoint execution to the middleware pipeline.
            // It runs the delegate associated with the selected endpoint.
            app.UseEndpoints(endpoints =>
            {
                // define path pattern to match the good controller's method
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
