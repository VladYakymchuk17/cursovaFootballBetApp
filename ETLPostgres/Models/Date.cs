using System;
using System.Collections.Generic;

namespace ETLPostgres.Models;

public partial class Date
{
    public int Id { get; set; }

    public DateOnly? Day { get; set; }

    public int? Month { get; set; }

    public int? Year { get; set; }

    public string? DayOfWeek { get; set; }

    public virtual ICollection<BetResult> BetResults { get; set; } = new List<BetResult>();

    public virtual ICollection<GameResult> GameResults { get; set; } = new List<GameResult>();

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();

    public virtual ICollection<PlayerResult> PlayerResults { get; set; } = new List<PlayerResult>();
}
