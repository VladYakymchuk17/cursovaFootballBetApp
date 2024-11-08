using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ETLPostgres.Models;

public partial class FootballContext : DbContext
{
    public FootballContext()
    {
    }

    public FootballContext(DbContextOptions<FootballContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BetResult> BetResults { get; set; }

    public virtual DbSet<Date> Dates { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GameResult> GameResults { get; set; }

    public virtual DbSet<League> Leagues { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Period> Periods { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<PlayerResult> PlayerResults { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<Venue> Venues { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Football;Username=postgres;Password=16042004Db;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BetResult>(entity =>
        {
            entity.HasKey(e => new { e.GameId, e.ResultId, e.HomeTeamId, e.AwayTeamId, e.DateId }).HasName("bet_result_pkey");

            entity.ToTable("bet_result");

            entity.Property(e => e.GameId).HasColumnName("game_id");
            entity.Property(e => e.ResultId).HasColumnName("result_id");
            entity.Property(e => e.HomeTeamId).HasColumnName("home_team_id");
            entity.Property(e => e.AwayTeamId).HasColumnName("away_team_id");
            entity.Property(e => e.DateId).HasColumnName("date_id");
            entity.Property(e => e.BetResult1).HasColumnName("bet_result");
            entity.Property(e => e.Coef).HasColumnName("coef");
            entity.Property(e => e.Income).HasColumnName("income");
            entity.Property(e => e.NumberOfBets).HasColumnName("number_of_bets");
            entity.Property(e => e.Outcome).HasColumnName("outcome");

            entity.HasOne(d => d.AwayTeam).WithMany(p => p.BetResultAwayTeams)
                .HasForeignKey(d => d.AwayTeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_away_team");

            entity.HasOne(d => d.Date).WithMany(p => p.BetResults)
                .HasForeignKey(d => d.DateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_date");

            entity.HasOne(d => d.Game).WithMany(p => p.BetResults)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_game");

            entity.HasOne(d => d.HomeTeam).WithMany(p => p.BetResultHomeTeams)
                .HasForeignKey(d => d.HomeTeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_home_team");

            entity.HasOne(d => d.Result).WithMany(p => p.BetResults)
                .HasForeignKey(d => d.ResultId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_result");
        });

        modelBuilder.Entity<Date>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("date_pkey");

            entity.ToTable("date");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Day).HasColumnName("day");
            entity.Property(e => e.DayOfWeek)
                .HasMaxLength(15)
                .HasColumnName("day_of_week");
            entity.Property(e => e.Month).HasColumnName("month");
            entity.Property(e => e.Year).HasColumnName("year");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_pkey");

            entity.ToTable("game");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AwayTeamId).HasColumnName("away_team_id");
            entity.Property(e => e.DateId).HasColumnName("date_id");
            entity.Property(e => e.HomeTeamId).HasColumnName("home_team_id");
            entity.Property(e => e.LeagueId).HasColumnName("league_id");
            entity.Property(e => e.VenueId).HasColumnName("venue_id");

            entity.HasOne(d => d.AwayTeam).WithMany(p => p.GameAwayTeams)
                .HasForeignKey(d => d.AwayTeamId)
                .HasConstraintName("fk_away_team");

            entity.HasOne(d => d.Date).WithMany(p => p.Games)
                .HasForeignKey(d => d.DateId)
                .HasConstraintName("fk_date");

            entity.HasOne(d => d.HomeTeam).WithMany(p => p.GameHomeTeams)
                .HasForeignKey(d => d.HomeTeamId)
                .HasConstraintName("fk_home_team");

            entity.HasOne(d => d.League).WithMany(p => p.Games)
                .HasForeignKey(d => d.LeagueId)
                .HasConstraintName("fk_league");

            entity.HasOne(d => d.Venue).WithMany(p => p.Games)
                .HasForeignKey(d => d.VenueId)
                .HasConstraintName("fk_venue");
        });

        modelBuilder.Entity<GameResult>(entity =>
        {
            entity.HasKey(e => new { e.GameId, e.PeriodId, e.TeamId, e.DateId }).HasName("game_result_pkey");

            entity.ToTable("game_result");

            entity.Property(e => e.GameId).HasColumnName("game_id");
            entity.Property(e => e.PeriodId).HasColumnName("period_id");
            entity.Property(e => e.TeamId).HasColumnName("team_id");
            entity.Property(e => e.DateId).HasColumnName("date_id");
            entity.Property(e => e.Assists).HasColumnName("assists");
            entity.Property(e => e.Fouls).HasColumnName("fouls");
            entity.Property(e => e.Goals).HasColumnName("goals");
            entity.Property(e => e.RedCards).HasColumnName("red_cards");
            entity.Property(e => e.Substitutions).HasColumnName("substitutions");
            entity.Property(e => e.YellowCards).HasColumnName("yellow_cards");

            entity.HasOne(d => d.Date).WithMany(p => p.GameResults)
                .HasForeignKey(d => d.DateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_date");

            entity.HasOne(d => d.Game).WithMany(p => p.GameResults)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_game");

            entity.HasOne(d => d.Period).WithMany(p => p.GameResults)
                .HasForeignKey(d => d.PeriodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_period");

            entity.HasOne(d => d.Team).WithMany(p => p.GameResults)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_team");
        });

        modelBuilder.Entity<League>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("league_pkey");

            entity.ToTable("league");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Round)
                .HasMaxLength(50)
                .HasColumnName("round");
            entity.Property(e => e.Season).HasColumnName("season");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("location_pkey");

            entity.ToTable("location");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Germany'::character varying")
                .HasColumnName("country");
            entity.Property(e => e.State)
                .HasMaxLength(100)
                .HasColumnName("state");
        });

        modelBuilder.Entity<Period>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("period_pkey");

            entity.ToTable("period");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PeriodName)
                .HasMaxLength(20)
                .HasColumnName("period_name");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("player_pkey");

            entity.ToTable("player");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.Nationality)
                .HasMaxLength(50)
                .HasColumnName("nationality");
            entity.Property(e => e.TeamId).HasColumnName("team_id");

            entity.HasOne(d => d.Team).WithMany(p => p.Players)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("fk_team");
        });

        modelBuilder.Entity<PlayerResult>(entity =>
        {
            entity.HasKey(e => new { e.GameId, e.PlayerId, e.DateId }).HasName("player_result_pkey");

            entity.ToTable("player_result");

            entity.Property(e => e.GameId).HasColumnName("game_id");
            entity.Property(e => e.PlayerId).HasColumnName("player_id");
            entity.Property(e => e.DateId).HasColumnName("date_id");
            entity.Property(e => e.Assists).HasColumnName("assists");
            entity.Property(e => e.Fouls).HasColumnName("fouls");
            entity.Property(e => e.Goals).HasColumnName("goals");
            entity.Property(e => e.Time).HasColumnName("time");

            entity.HasOne(d => d.Date).WithMany(p => p.PlayerResults)
                .HasForeignKey(d => d.DateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_date");

            entity.HasOne(d => d.Game).WithMany(p => p.PlayerResults)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_game");

            entity.HasOne(d => d.Player).WithMany(p => p.PlayerResults)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_player");
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("result_pkey");

            entity.ToTable("result");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
            entity.Property(e => e.Score)
                .HasMaxLength(100)
                .HasColumnName("score");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("team_pkey");

            entity.ToTable("team");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.HasOne(d => d.Location).WithMany(p => p.Teams)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("fk_location");
        });

        modelBuilder.Entity<Venue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("venue_pkey");

            entity.ToTable("venue");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .HasColumnName("city");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
