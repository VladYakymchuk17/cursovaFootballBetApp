using System;
using System.Collections.Generic;

namespace ETLPostgres.Models;

public partial class GameResult
{
    public int GameId { get; set; }

    public int PeriodId { get; set; }

    public int DateId { get; set; }

    public int TeamId { get; set; }

    public int Goals { get; set; }

    public int Assists { get; set; }

    public int Fouls { get; set; }

    public int RedCards { get; set; }

    public int YellowCards { get; set; }

    public int Substitutions { get; set; }

    public virtual Date Date { get; set; } = null!;

    public virtual Game Game { get; set; } = null!;

    public virtual Period Period { get; set; } = null!;

    public virtual Team Team { get; set; } = null!;
}
