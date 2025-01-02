
using System.Globalization;

namespace BE_lab1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapGet("/healthcheck", () => Results.Ok(new
            {
                status = "Healthy",
                time = DateTime.Now.ToString(" yyyy MMMM dd HH:mm:ss K", CultureInfo.InvariantCulture)
            }));


            app.Run();
        }
    }
}
