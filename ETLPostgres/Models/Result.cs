using System;
using System.Collections.Generic;

namespace ETLPostgres.Models;

public partial class Result
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string Score { get; set; } = null!;

    public virtual ICollection<BetResult> BetResults { get; set; } = new List<BetResult>();
}
