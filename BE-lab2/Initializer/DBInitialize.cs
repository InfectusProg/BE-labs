using BE_lab2.Data;
using BE_lab2.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_lab2.Initializer;

public class DbInitialize : IDbInitializer
{
    private readonly AppDbContext _db;
    public DbInitialize(AppDbContext db)
    {
        _db = db;
    }

    public void Initialize()
    {
        try
        {
            if (_db.Database.GetPendingMigrations().Any())
            {
                _db.Database.Migrate();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error during migration: " + ex.Message);
        }

        if (_db.Currencies.Any())
        {
            return;
        }

        Currency currency = new Currency()
        {
            Id = 1,
            Name = "USD"
        };
        _db.Currencies.Add(currency);
        _db.SaveChanges();
    }
}
