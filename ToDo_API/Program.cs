

using Microsoft.EntityFrameworkCore;
using ToDo_API.Middleware;
using ToDo_API.Services;

namespace ToDo_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddScoped<ErrorHandlingMiddleware>();
            builder.Services.AddScoped<IToDoService, ToDoService>();
            builder.Services.AddDbContext<Entities.ToDoDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo());
            });
            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo API");
                });

            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
