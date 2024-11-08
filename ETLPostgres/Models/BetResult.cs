using System;
using System.Collections.Generic;

namespace ETLPostgres.Models;

public partial class BetResult
{
    public int GameId { get; set; }

    public int ResultId { get; set; }

    public int DateId { get; set; }

    public int HomeTeamId { get; set; }

    public int AwayTeamId { get; set; }

    public int NumberOfBets { get; set; }

    public int Income { get; set; }

    public int Outcome { get; set; }

    public int BetResult1 { get; set; }

    public double Coef { get; set; }

    public virtual Team AwayTeam { get; set; } = null!;

    public virtual Date Date { get; set; } = null!;

    public virtual Game Game { get; set; } = null!;

    public virtual Team HomeTeam { get; set; } = null!;

    public virtual Result Result { get; set; } = null!;
}
