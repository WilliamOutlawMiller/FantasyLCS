using FantasyLCS.DataObjects;
using Microsoft.EntityFrameworkCore;
using PlayerStats;

public class AppDbContext : DbContext
{
    public DbSet<Player> Players { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<DataUpdateLog> DataUpdateLogs { get; set; }

    public DbSet<GeneralStats> GeneralStats { get; set; }
    public DbSet<ChampionStats> ChampionStats { get; set; }
    public DbSet<AggressionStats> AggressionStats { get; set; }
    public DbSet<EarlyGameStats> EarlyGameStats { get; set; }
    public DbSet<VisionStats> VisionStats { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=appdata.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure one-to-one relationships
        modelBuilder.Entity<Player>()
            .HasOne(p => p.AggressionStats)
            .WithOne(a => a.Player)
            .HasForeignKey<AggressionStats>(a => a.PlayerID)
            .IsRequired();


        modelBuilder.Entity<AggressionStats>()
            .HasKey(a => a.PlayerID);

        modelBuilder.Entity<EarlyGameStats>()
            .HasKey(a => a.PlayerID);

        modelBuilder.Entity<VisionStats>()
            .HasKey(a => a.PlayerID);

        modelBuilder.Entity<GeneralStats>()
            .HasKey(a => a.PlayerID);

        // Configure one-to-many relationship
        modelBuilder.Entity<Player>()
            .HasMany(p => p.ChampionStats)
            .WithOne(c => c.Player)
            .HasForeignKey(c => c.PlayerID);

        // Configure ChampionStats with ChampionID as the primary key
        modelBuilder.Entity<ChampionStats>()
            .HasKey(c => c.ChampionID);

        modelBuilder.Entity<User>()
        .HasKey(u => u.ID);

        modelBuilder.Entity<User>()
            .Property(u => u.ID)
            .ValueGeneratedOnAdd();
    }
}
