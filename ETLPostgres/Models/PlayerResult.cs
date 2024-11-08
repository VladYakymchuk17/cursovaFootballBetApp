using System;
using System.Collections.Generic;

namespace ETLPostgres.Models;

public partial class PlayerResult
{
    public int GameId { get; set; }

    public int PlayerId { get; set; }

    public int DateId { get; set; }

    public int Goals { get; set; }

    public int Assists { get; set; }

    public int Fouls { get; set; }

    public int Time { get; set; }

    public virtual Date Date { get; set; } = null!;

    public virtual Game Game { get; set; } = null!;

    public virtual Player Player { get; set; } = null!;
}
