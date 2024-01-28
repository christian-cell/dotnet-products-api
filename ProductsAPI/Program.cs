using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProductsAPI.Helpers;
using ProductsRepository.Abstractions;
using ProductsRepository.Implemetations;
using ProductsServices.Abstractions;
using ProductsServices.Configurations;
using ProductsServices.Services;

namespace ProductsAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
// Add services to the container.

            builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
/*add IProductService ProductService in dependencies container*/
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<AuthHelper>();

            builder.Services.AddDbContext<ProductDbContext>( options => 
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddSingleton(new ProductServiceConfig()
            {
                DefaultConnection = builder.Configuration.GetConnectionString("DefaultConnection")
            });

            string? tokenKeyString = builder.Configuration.GetSection("AppSettings:TokenKey").Value;

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer( options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                            tokenKeyString != null ? tokenKeyString : ""
                        )),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            var app = builder.Build();

// Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}