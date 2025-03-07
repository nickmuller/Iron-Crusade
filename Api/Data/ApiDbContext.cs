using System.ComponentModel.DataAnnotations;
using IronCrusade.Shared.Types;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class ApiDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<CategorieType>().HaveConversion<string>();
    }
    
    public DbSet<WorkoutLog> WorkoutLogs { get; set; }
    public DbSet<PersoonlijkeGegevensLog> PersoonlijkeGegevensLogs { get; set; }
}

public class WorkoutLog
{
    public int Id { get; set; }
    
    [MaxLength(500)]
    public string Username { get; set; } = null!;
    
    public CategorieType Categorie { get; set; }
    public DateTime WorkoutStart { get; set; }
    public DateTime? WorkoutEind { get; set; }
}

public class PersoonlijkeGegevensLog
{
    public int Id { get; set; }
    
    [MaxLength(500)]
    public string Username { get; set; } = null!;
    
    public DateTime Datum { get; set; }
    public decimal Gewicht { get; set; }
}