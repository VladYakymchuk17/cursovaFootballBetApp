using System;
using System.Collections.Generic;

namespace ETLPostgres.Models;

public partial class Game
{
    public int Id { get; set; }

    public int? LeagueId { get; set; }

    public int? VenueId { get; set; }

    public int? HomeTeamId { get; set; }

    public int? AwayTeamId { get; set; }

    public int? DateId { get; set; }

    public virtual Team? AwayTeam { get; set; }

    public virtual ICollection<BetResult> BetResults { get; set; } = new List<BetResult>();

    public virtual Date? Date { get; set; }

    public virtual ICollection<GameResult> GameResults { get; set; } = new List<GameResult>();

    public virtual Team? HomeTeam { get; set; }

    public virtual League? League { get; set; }

    public virtual ICollection<PlayerResult> PlayerResults { get; set; } = new List<PlayerResult>();

    public virtual Venue? Venue { get; set; }
}
