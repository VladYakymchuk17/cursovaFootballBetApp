using System;
using System.Collections.Generic;

namespace ETLPostgres.Models;

public partial class Period
{
    public int Id { get; set; }

    public string? PeriodName { get; set; }

    public virtual ICollection<GameResult> GameResults { get; set; } = new List<GameResult>();
}
