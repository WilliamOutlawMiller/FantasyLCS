using FantasyLCS.DataObjects;
using Microsoft.EntityFrameworkCore;
using FantasyLCS.DataObjects.PlayerStats;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public class AppDbContext : DbContext
{
    public DbSet<Player> Players { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<League> Leagues { get; set; }
    public DbSet<LeagueMatch> LeagueMatches { get; set; }
    public DbSet<Draft> Drafts { get; set; }
    public DbSet<DraftPlayer> DraftPlayers { get; set; }
    public DbSet<DataUpdateLog> DataUpdateLogs { get; set; }

    public DbSet<FullStats> FullStats { get; set; }
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

        modelBuilder.Entity<Match>()
            .HasMany(m => m.FullStats)
            .WithOne(fs => fs.Match)
            .HasForeignKey(fs => fs.MatchID);

        // Configure ChampionStats with ChampionID as the primary key
        modelBuilder.Entity<ChampionStats>()
            .HasKey(cs => new { cs.ChampionID, cs.PlayerID });

        modelBuilder.Entity<FullStats>()
            .HasKey(fs => new { fs.MatchID, fs.PlayerID });

        modelBuilder.Entity<User>()
            .HasOne<Team>()
            .WithOne()
            .HasForeignKey<User>(u => u.TeamID)
            .IsRequired(false);

        // Configure the foreign key relationship between User and League
        modelBuilder.Entity<User>()
            .HasOne<League>()
            .WithMany()
            .HasForeignKey(u => u.LeagueID)
            .IsRequired(false);

        modelBuilder.Entity<Team>()
            .Property(t => t.PlayerIDs)
            // convert the List<int> to a comma-separated string when saving to the database, and back to a List<int> when reading from the database.
            .HasConversion(
                v => string.Join(",", v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                      .Select(int.Parse).ToList())
            .Metadata.SetValueComparer(new ValueComparer<List<int>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));

        modelBuilder.Entity<League>()
            .Property(l => l.UserIDs)
            // convert the List<int> to a comma-separated string when saving to the database, and back to a List<int> when reading from the database.
            .HasConversion(
                v => string.Join(",", v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                      .Select(int.Parse).ToList())
            .Metadata.SetValueComparer(new ValueComparer<List<int>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));

        modelBuilder.Entity<LeagueMatch>()
            .HasOne(lm => lm.League)
            .WithMany() // Assuming a League can have many LeagueMatches
            .HasForeignKey(lm => lm.LeagueID);

        modelBuilder.Entity<LeagueMatch>()
            .HasOne(lm => lm.TeamOne)
            .WithMany() // Assuming a Team can be part of many LeagueMatches
            .HasForeignKey(lm => lm.TeamOneID);

        modelBuilder.Entity<LeagueMatch>()
            .HasOne(lm => lm.TeamTwo)
            .WithMany() // Assuming a Team can be part of many LeagueMatches
            .HasForeignKey(lm => lm.TeamTwoID);

        modelBuilder.Entity<Draft>()
            .HasMany(d => d.DraftPlayers);
    }
}
