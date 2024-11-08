using System;
using System.Collections.Generic;

namespace ETLPostgres.Models;

public partial class Player
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int Age { get; set; }

    public string Nationality { get; set; } = null!;

    public int? TeamId { get; set; }

    public virtual ICollection<PlayerResult> PlayerResults { get; set; } = new List<PlayerResult>();

    public virtual Team? Team { get; set; }
}
