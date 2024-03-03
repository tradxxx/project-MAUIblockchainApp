
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebApiBlockChain.Data;
using WebApiBlockChain.Models;
using WebApiBlockChain.Service;

namespace WebApiBlockChain
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddDbContext<BlockchainContext>(options =>
			{
				options.UseSqlServer(builder.Configuration["ConnectionStrings:DbConnection"]);
			});
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/login"; // путь к странице логина
        });
            builder.Services.AddScoped<IBlockService,BlockService>();
            builder.Services.AddScoped<EntityGateway>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			
			app.UseCors("AllowAll");
			app.UseRouting();

			app.UseHttpsRedirection();

            app.UseAuthentication(); // добавляем middleware для аутентификации
            app.UseAuthorization(); // добавляем middleware для авторизации

            app.MapControllers();

			app.Run();
		}
	}
}