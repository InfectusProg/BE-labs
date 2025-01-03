
using BE_lab2.Data;
using BE_lab2.Initializer;
using Microsoft.EntityFrameworkCore;

namespace BE_lab2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string? getEnv = Environment.GetEnvironmentVariable("DefaultConnection");

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(getEnv));
            builder.Services.AddScoped<IDbInitializer, DbInitialize>();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                dbInitializer.Initialize();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
