using System;
using System.Collections.Generic;

namespace ETLPostgres.Models;

public partial class Team
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? LocationId { get; set; }

    public virtual ICollection<BetResult> BetResultAwayTeams { get; set; } = new List<BetResult>();

    public virtual ICollection<BetResult> BetResultHomeTeams { get; set; } = new List<BetResult>();

    public virtual ICollection<Game> GameAwayTeams { get; set; } = new List<Game>();

    public virtual ICollection<Game> GameHomeTeams { get; set; } = new List<Game>();

    public virtual ICollection<GameResult> GameResults { get; set; } = new List<GameResult>();

    public virtual Location? Location { get; set; }

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();
}
