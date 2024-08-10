using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Store.Core.Entities.Identity;
using Store.Repository.Data;
using Store.Repository.Identity;
using Talabat.APIs.Extensions;
using Store.APIs.MiddleWares;
using Store.API.Extensions;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure services
            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddSwaggerServices();

            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            }); 

            // config service for cashing
            builder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });

            builder.Services.AddApplicationServices();  // add all services dependinces and validate model user error

            builder.Services.AddIdentityServices(builder.Configuration);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", options =>
                {
                    options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });

            #endregion

            var app = builder.Build();

            using var scope=app.Services.CreateScope();

            var services=scope.ServiceProvider; // used to get service with any type in app

            var _dbContext=services.GetRequiredService<StoreContext>();
            //Ask CLR for creating object from Context explicitly

            var _identityDbContext = services.GetRequiredService<AppIdentityDbContext>();

            var loggerFactory =services.GetRequiredService<ILoggerFactory>();

            try
            {
                await _dbContext.Database.MigrateAsync(); //Updata-Database
                await StoreContextSeed.SeedAsync(_dbContext); //Data Seeding

                await _identityDbContext.Database.MigrateAsync(); //Updata-Database
                var _userManager = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUsersAsync(_userManager); //Data Seeding

            }
            catch (Exception ex)
            {
                var logger=loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An Error has been occured during applying the migration");
            }

            #region Configure Middlewares

            app.UseMiddleware<ExceptionMiddleWare>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddlewares();
            }

            app.UseStatusCodePagesWithRedirects("/errors/{0}"); // used to handle unexpected error happens with status code 400 (syntax error) 404 (not found) 500 (internal server error )

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCors("MyPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            
            #endregion

            app.Run();
        }
    }
}
