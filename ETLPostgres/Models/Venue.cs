using System;
using System.Collections.Generic;

namespace ETLPostgres.Models;

public partial class Venue
{
    public int Id { get; set; }

    public string? City { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
